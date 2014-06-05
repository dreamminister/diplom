// SSSproject.cpp:
//

#include "stdafx.h"
#include <stdlib.h>
#include "sss.h"
#include "ssssbox.h"
#include "sssmultab.h"
#include <iostream>

#define NumberOfExtraPatterns 8

/* some useful macros -- machine independent little-endian 2-byte words */
#define B(x,i) ((UCHAR)(((x) >> (8*i)) & 0xFF))
#define BYTE2WORD(b) ( \
	(((WORD)((b)[1]) & 0xFF)<<8) | \
	(((WORD)((b)[0]) & 0xFF)) \
)
#define WORD2BYTE(w, b) { \
	(b)[1] = B(w,1); \
	(b)[0] = B(w,0); \
}
#define XORWORD(w, b) { \
	(b)[1] ^= B(w,1); \
	(b)[0] ^= B(w,0); \
}

/* key-dependent Sbox function -- after the model used by Turing */
WORD
Sfunc(UCHAR *Key, int KeyLength, WORD w)
{
    register int	i;
    WORD		t;
    UCHAR		b;

    t = 0;
    b = HIGHBYTE(w);
    for (i = 0; i < KeyLength; ++i) {
	b = ftable[b ^ Key[i]];
	t ^= ROTL(Qbox[b], i);
    }
    return ((b << (WORDBITS-8)) | (t & LOWMASK)) ^ (w & LOWMASK);
}

/* cycle the contents of the shift register */
static void
cycle(sss_ctx *State, WORD ctxt)
{
    register int    i;

    for (i = 0; i < N-1; ++i)
	State->ShiftRegister[i] = State->ShiftRegister[i+1];
    State->ShiftRegister[16] = ctxt;
    State->ShiftRegister[14] += SBoxFromWord(State, ROTR(ctxt, 8));
    State->ShiftRegister[12] = SBoxFromWord(State, State->ShiftRegister[12]);
    State->ShiftRegister[1] = ROTR(State->ShiftRegister[1], 8);
}

/*
 * Return a non-linear function of some parts of the register.
 * The positions of the state bytes form a maximal span full positive
 * difference set, and are 0, 1, 6, 13, 16.
 */
static WORD
nltap(sss_ctx *c)
{
    register WORD	t;

    t = c->ShiftRegister[0] + c->ShiftRegister[16];
    //printf("OK? %04x\n",t);
    t = SBoxFromWord(c,t) + c->ShiftRegister[1] + c->ShiftRegister[6] + c->ShiftRegister[13];
    t = ROTR(t, 8);
    //printf("inputlastf %04x\n",t); 
    return SBoxFromWord(c,t) ^ c->ShiftRegister[0];
}

/*
 * Add a MAC-text word to the CRC register.
 * When the transmission is plaintext, the corresponding ciphertext is
 * calculated and fed back into the stream cipher, and also accumulated
 * into the CRC. When the transmission is ciphertext, it is decrypted
 * as usual, and plaintext is accumulated into the CRC. Whatever goes into
 * the CRC is something the adversary didn't intercept.
 */
static void
crccycle(sss_ctx *c, WORD w)
{
    register int	i;

    w ^= (c->CRC[0] << 8) ^ tab500F[(c->CRC[0] >> 8) & 0xFF] ^
	    c->CRC[4] ^ c->CRC[15];
    for (i = 0; i < N-1; ++i)
	c->CRC[i] = c->CRC[i+1];
    c->CRC[N-1] = w;
}

/*
 * Set up key for SSS.
 * There are at least four different ways to go about this.
 * 1. Save the key for later and calculate Sbox entries on the fly.
 *    (This gives greatest key agility, smallest memory, slow execution.)
 * 2. Fill in a table of all possible Sbox entries at key setup time.
 *    (Less key agility, more memory, fastest streaming.)
 * 3. Fill in table entries when they are first used (lazy evaluation).
 *    (Most memory, good key agility, possible side-channel vulnerability.)
 * 4. Calculate the Sbox at build time, burn into ROM.
 *    (Most efficient, no key agility at all, good for embedded devices.)
 * We implement either 1 or 2, depending on SBOX_PRECOMP.
 */
#if SBOX_PRECOMP
/*
 * Precompute the key-dependent Sbox for later efficiency.
 */
void
sss_key(sss_ctx *State, UCHAR Key[], int KeyLength)
{
    int i;
    if (KeyLength > MAXKEY)
	abort();
    for (i = 0; i < 256; ++i)
	State->SBox[i] = Sfunc(Key, KeyLength, (WORD)(i << (WORDBITS-8))) ^ (i << (WORDBITS-8));

    /* in case no nonce... */
    sss_nonce(State, (UCHAR *)0, 0);
}
#else
/*
 * Just save the key for later Sbox on-the-fly generation
 */
void
sss_key(sss_ctx *c, UCHAR key[], int keylen)
{
    int	    i;

    if ((c->keylen = keylen) > MAXKEY)
	abort();
    for (i = 0; i < keylen; ++i)
	c->key[i] = key[i];

    /* in case no nonce... */
    sss_nonce(c, (UCHAR *)0, 0);
}
#endif /*SBOX_PRECOMP*/

/*
 * Fold in the nonce.
 * Start the two registers as all zeros.
 * (TWEAKABILITY: values other than zero at this point yield distinct, but
 * equally secure, ciphers.)
 * Next treat nonce as ciphertext input (MAC and decrypt), until used up.
 * Then MAC N words of zeros (completely diffuses nonce).
 */
void
sss_nonce(sss_ctx *State, UCHAR nonce[], int nlen)
{
    int		i;
    UCHAR	nb[2];

    if ((nlen % WORDBYTES) != 0)
	abort();
    /* first fill both registers with zeros */
    for (i = 0; i < N; ++i)
	State->ShiftRegister[i] = State->CRC[i] = 0;

    /* now process words of the nonce */
    for (i = 0; i < nlen; i += WORDBYTES) {
	nb[0] = nonce[i];
	nb[1] = nonce[i+1];
	sss_decrypt(State, nb, WORDBYTES);
    }

    /* now MAC N words of zeros */
    nb[0] = nb[1] = 0;
    for (i = 0; i < N; ++i) {
	sss_maconly(State, nb, WORDBYTES);
    }

    State->NumberOfBitsBuffered = 0;
}

/* encryption.
 * (Don't mix with MAC operations)
 */
void
sss_enconly(sss_ctx *c, UCHAR *buf, int nbytes)
{
    WORD	t = 0;

    /* handle any previously buffered bytes */
    if (c->NumberOfBitsBuffered != 0) {
	while (c->NumberOfBitsBuffered != 0 && nbytes != 0) {
	    *buf ^= c->StreamBuf & 0xFF;
	    c->CipherTextBuf ^= *buf << (WORDBITS - c->NumberOfBitsBuffered);
	    c->StreamBuf >>= 8;
	    ++buf;
	    c->NumberOfBitsBuffered -= 8;
	    --nbytes;
	}
	if (c->NumberOfBitsBuffered != 0) /* still not a whole word yet */
	    return;
	/* Accrue that ciphertext word */
	cycle(c, c->CipherTextBuf);
    }

    /* handle whole words */
    while (nbytes >= WORDBYTES)
    {
	t = nltap(c) ^ BYTE2WORD(buf);
	WORD2BYTE(t, buf);
	cycle(c, t);
	buf += WORDBYTES;
	nbytes -= WORDBYTES;
    }

    /* handle any trailing bytes */
    if (nbytes != 0) {
	c->StreamBuf = nltap(c);
	c->CipherTextBuf = 0;
	c->NumberOfBitsBuffered = WORDBITS;
	while (c->NumberOfBitsBuffered != 0 && nbytes != 0) {
	    *buf ^= c->StreamBuf & 0xFF;
	    c->StreamBuf >>= 8;
	    c->CipherTextBuf ^= *buf << (WORDBITS - c->NumberOfBitsBuffered);
	    c->NumberOfBitsBuffered -= 8;
	    --nbytes;
	}
    }
}

/* decryption.
 * (Don't mix with MAC operations)
 */
void
sss_deconly(sss_ctx *State, UCHAR *CipherTextBuffer, int CipherTextLength)
{
    WORD	t = 0, t2 = 0;

    /* handle any previously buffered bytes */
    if (State->NumberOfBitsBuffered != 0) {
	while (State->NumberOfBitsBuffered != 0 && CipherTextLength != 0) {
	    State->CipherTextBuf ^= *CipherTextBuffer << (WORDBITS - State->NumberOfBitsBuffered);
	    *CipherTextBuffer ^= State->StreamBuf & 0xFF;
	    State->StreamBuf >>= 8;
	    State->NumberOfBitsBuffered -= 8;
	    ++CipherTextBuffer;
	    --CipherTextLength;
	}
	if (State->NumberOfBitsBuffered != 0) /* still not a whole word yet */
	    return;
	/* Accrue that ciphertext word */
	cycle(State, State->CipherTextBuf);
    }

    /* handle whole words */
    while (CipherTextLength >= WORDBYTES)
    {
      t = nltap(State); 
	t2 = BYTE2WORD(CipherTextBuffer);
	cycle(State, t2);
	t ^= t2;
	WORD2BYTE(t, CipherTextBuffer);
	CipherTextBuffer += WORDBYTES;
	CipherTextLength -= WORDBYTES;
    }

    /* handle any trailing bytes */
    if (CipherTextLength != 0) {
	State->StreamBuf = nltap(State);
	State->CipherTextBuf = 0;
	State->NumberOfBitsBuffered = WORDBITS;
	while (State->NumberOfBitsBuffered != 0 && CipherTextLength != 0) {
	    State->CipherTextBuf ^= *CipherTextBuffer << (WORDBITS - State->NumberOfBitsBuffered);
	    *CipherTextBuffer ^= State->StreamBuf & 0xFF;
	    State->StreamBuf >>= 8;
	    State->NumberOfBitsBuffered -= 8;
	    --CipherTextLength;
	}
    }
}

/* encrypt and MAC.
 * Note that plaintext is accumulated for MAC.
 */
void
sss_encrypt(sss_ctx *c, UCHAR *buf, int nbytes)
{
    WORD	t = 0;

    /* handle any previously buffered bytes */
    if (c->NumberOfBitsBuffered != 0) {
	while (c->NumberOfBitsBuffered != 0 && nbytes != 0) {
	    c->MacBuf ^= *buf << (WORDBITS - c->NumberOfBitsBuffered);
	    *buf ^= c->StreamBuf & 0xFF;
	    c->CipherTextBuf ^= *buf << (WORDBITS - c->NumberOfBitsBuffered);
	    c->StreamBuf >>= 8;
	    ++buf;
	    c->NumberOfBitsBuffered -= 8;
	    --nbytes;
	}
	if (c->NumberOfBitsBuffered != 0) /* still not a whole word yet */
	    return;
	/* Accrue that ciphertext word */
	cycle(c, c->CipherTextBuf);
	crccycle(c, c->MacBuf);
    }

    /* handle whole words */
    while (nbytes >= WORDBYTES)
    {
	t = BYTE2WORD(buf);
	crccycle(c, t);
	t ^= nltap(c);
	WORD2BYTE(t, buf);
	cycle(c, t);
	buf += WORDBYTES;
	nbytes -= WORDBYTES;
    }

    /* handle any trailing bytes */
    if (nbytes != 0) {
	c->MacBuf = 0;
	c->StreamBuf = nltap(c);
	c->CipherTextBuf = 0;
	c->NumberOfBitsBuffered = WORDBITS;
	while (c->NumberOfBitsBuffered != 0 && nbytes != 0) {
	    c->MacBuf ^= *buf << (WORDBITS - c->NumberOfBitsBuffered);
	    *buf ^= c->StreamBuf & 0xFF;
	    c->CipherTextBuf ^= *buf << (WORDBITS - c->NumberOfBitsBuffered);
	    c->StreamBuf >>= 8;
	    c->NumberOfBitsBuffered -= 8;
	    --nbytes;
	    ++buf;
	}
    }
}

/* decrypt and accumulate MAC.
 * Note that plaintext is accumulated for MAC.
 */
void
sss_decrypt(sss_ctx *c, UCHAR *buf, int nbytes)
{
    WORD	t = 0, t2 = 0;;

    /* handle any previously buffered bytes */
    if (c->NumberOfBitsBuffered != 0) {
	while (c->NumberOfBitsBuffered != 0 && nbytes != 0) {
	    c->CipherTextBuf ^= *buf << (WORDBITS - c->NumberOfBitsBuffered);
	    *buf ^= c->StreamBuf & 0xFF;
	    c->MacBuf ^= *buf << (WORDBITS - c->NumberOfBitsBuffered);
	    ++buf;
	    c->StreamBuf >>= 8;
	    c->NumberOfBitsBuffered -= 8;
	    --nbytes;
	}
	if (c->NumberOfBitsBuffered != 0) /* still not a whole word yet */
	    return;
	/* Accrue that ciphertext word */
	cycle(c, c->CipherTextBuf);
	crccycle(c, c->MacBuf);
    }

    /* handle whole words */
    while (nbytes >= WORDBYTES)
    {
	t = BYTE2WORD(buf);
	t2 = nltap(c);
	cycle(c, t);
	t ^= t2;
	crccycle(c, t);
	WORD2BYTE(t, buf);
	buf += WORDBYTES;
	nbytes -= WORDBYTES;
    }

    /* handle any trailing bytes */
    if (nbytes != 0) {
	c->StreamBuf = nltap(c);
	c->CipherTextBuf = 0;
	c->MacBuf = 0;
	c->NumberOfBitsBuffered = WORDBITS;
	while (c->NumberOfBitsBuffered != 0 && nbytes != 0) {
	    c->CipherTextBuf ^= *buf << (WORDBITS - c->NumberOfBitsBuffered);
	    *buf ^= c->StreamBuf & 0xFF;
	    c->MacBuf ^= *buf << (WORDBITS - c->NumberOfBitsBuffered);
	    c->StreamBuf >>= 8;
	    ++buf;
	    c->NumberOfBitsBuffered -= 8;
	    --nbytes;
	}
    }
}

/* Accumulate words into MAC without encryption
 * Note that ciphertext is accumulated for both feedback and MAC.
 */
void
sss_maconly(sss_ctx *c, UCHAR *buf, int nbytes)
{
    WORD	t = 0;
    UCHAR	ct;

    /* handle any previously buffered bytes */
    if (c->NumberOfBitsBuffered != 0) {
	while (c->NumberOfBitsBuffered != 0 && nbytes != 0) {
	    ct = *buf ^ (c->StreamBuf & 0xFF);
	    c->CipherTextBuf ^= ct << (WORDBITS - c->NumberOfBitsBuffered);
	    c->MacBuf ^= ct << (WORDBITS - c->NumberOfBitsBuffered);
	    ++buf;
	    c->StreamBuf >>= 8;
	    c->NumberOfBitsBuffered -= 8;
	    --nbytes;
	}
	if (c->NumberOfBitsBuffered != 0) /* still not a whole word yet */
	    return;
	/* Accrue that ciphertext word */
	cycle(c, c->CipherTextBuf);
	crccycle(c, c->MacBuf);
    }

    /* handle whole words */
    while (nbytes >= WORDBYTES)
    {
	t = BYTE2WORD(buf) ^ nltap(c);
	cycle(c, t);
	crccycle(c, t);
	/* discard t */
	buf += WORDBYTES;
	nbytes -= WORDBYTES;
    }

    /* handle any trailing bytes */
    if (nbytes != 0) {
	c->StreamBuf = nltap(c);
	c->CipherTextBuf = 0;
	c->MacBuf = 0;
	c->NumberOfBitsBuffered = WORDBITS;
	while (c->NumberOfBitsBuffered != 0 && nbytes != 0) {
	    ct = *buf ^ (c->StreamBuf & 0xFF);
	    c->CipherTextBuf ^= ct << (WORDBITS - c->NumberOfBitsBuffered);
	    c->MacBuf ^= ct << (WORDBITS - c->NumberOfBitsBuffered);
	    c->StreamBuf >>= 8;
	    ++buf;
	    c->NumberOfBitsBuffered -= 8;
	    --nbytes;
	}
    }
}

/* Having accumulated a MAC, finish processing and return it.
 * Note that any unprocessed bytes are treated as if
 * they were maconly zero bytes, so ciphertext is accumulated.
 */
void
sss_finish(sss_ctx *c, UCHAR *buf, int nbytes)
{
    int		i;
    WORD	w = c->NumberOfBitsBuffered;
    UCHAR	ct;

    /* handle any previously buffered bytes */
    if (c->NumberOfBitsBuffered != 0) {
	while (c->NumberOfBitsBuffered != 0) {
	    ct = c->StreamBuf & 0xFF; /* as if encrypted zero byte */
	    c->CipherTextBuf ^= ct << (WORDBITS - c->NumberOfBitsBuffered);
	    c->MacBuf ^= ct << (WORDBITS - c->NumberOfBitsBuffered);
	    c->StreamBuf >>= 8;
	    c->NumberOfBitsBuffered -= 8;
	}
	/* Accrue that ciphertext and MAC word */
	cycle(c, c->CipherTextBuf);
	crccycle(c, c->MacBuf);
    }
    
    /* Perturb only the stream cipher state to mark end of input,
     * by effectively encrypting a word with the previous number of
     * buffered bits in it.
     * Note that only the nonlinear part is updated, not the CRC.
     * Desynchronizing the registers this way is an
     * action that can't be duplicated by passing in plaintext, hence
     * defeating any kind of extension attack.
     */
    w ^= nltap(c);
    cycle(c, w);

    /* Now process the CRC words in reverse order as if they were plaintext */
    for (i = N; --i >= 0; i)
	cycle(c, (WORD)(c->CRC[i] ^ nltap(c)));

    /* To produce the MAC, just encrypt a stream of zeros */
    while (nbytes >= WORDBYTES) {
	w = nltap(c);
	cycle(c, w);
	WORD2BYTE(w, buf);
	buf += WORDBYTES;
	nbytes -= WORDBYTES;
    }
    if (nbytes > 0) {
	/* one last partial word... */
	w = nltap(c);
	while (nbytes > 0) {
	    *buf++ ^= w;
	    w >>= 8;
	    --nbytes;
	}
    }
}


int _tmain(int argc, _TCHAR* argv[])
{
  sss_ctx State;      // the State we are trying to determine
  sss_ctx StateGuess; // our guess for the State 
  UCHAR CipherText[36*256 + 36*NumberOfExtraPatterns]; // our chosen CipherText, which will be always the same to recover any secret key
  UCHAR PlainText[36*256 + 36*NumberOfExtraPatterns];  // the decrypted PlainText corresponding to our CipherText
  int i,j,CipherTextLength,correct;
  int ctr_aL, ctr_SaL;
  UCHAR aL, aH, aHg1,aHg2;
  UCHAR SboxInput;
  WORD SboxGuess;
  WORD a, Rotated_a, a_plus_i, Rotated_a_plus_i;
  int EverythingCorrect;

  CipherTextLength = 36*256 + 36*NumberOfExtraPatterns;

  //We use a zero key and print the actual secret key-dependent SBox - You can put any other key here. 
  UCHAR ZeroKey[16] = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
  sss_key(&State, ZeroKey, 16);
  //  for (i=0;i<256;i++) printf("%02x \t %04x\n",i,State.SBox[i]);
  
  // Now we choose our CipherText
  for (i = 0; i < CipherTextLength; i++) 
	  CipherText[i] = 0; // all values init to 0

  for (i = 0; i < 256; i++)            
	  CipherText[36*i + 26] = i;        // 255 modifications in LSB of r[13]

  for (i = 0; i < NumberOfExtraPatterns; i++)         
	  CipherText[256*36 + 36*i + 27] = i; // NumberOfExtraPatterns modifications in MSB of r[13]

  // And obtain the corresponding PlainText
  for (i = 0; i < CipherTextLength; i++) 
	  PlainText[i] = CipherText[i];

  sss_deconly(&State, PlainText, CipherTextLength);

  for (ctr_aL = 0; ctr_aL < 256; ctr_aL++)
  {
    //for (ctr_aL=0x11;ctr_aL<0x12;ctr_aL++){
    aL = ctr_aL;    
    // determine a_H
    aH = 0;
    //for (i=0;i<256;i++){ // try all possible aH
    for (i = 0; i < 256; i++)
	{ // try all possible aH
      aHg1 = i;
      correct = 1;
      for (j = 0; j < NumberOfExtraPatterns; j++)
	  {
		aHg2 = aHg1 + j;
		if ( (PlainText[34]^PlainText[256*36 + 36*j + 34]) != (aHg1^aHg2) ) 
			correct = 0; //{correct=0; printf("probleem\n");} else printf("geen probleem!\n");
      }
      if (correct)
		  aH = aHg1; //{aH=aHg1; printf("%02x\n",aH);}
    }
    a = (aH<<8)^aL;
    Rotated_a = (aL<<8)^aH;
    for (ctr_SaL = 0; ctr_SaL < 65536; ctr_SaL++)
	{
      //for (ctr_SaL=0x4cd9;ctr_SaL<0x4cda;ctr_SaL++){
      // Set the guess back on the beginning values
      for (i = 0; i < 256; i++) 
		  StateGuess.SBox[i] = 0;

      StateGuess.SBox[aL] = ctr_SaL;

      // determine the entire SBox
      for (i = 1; i < 256; i++)
	  {
		a_plus_i = a+i; 
		Rotated_a_plus_i = ( (a_plus_i & 0xff) << 8) ^ ( (a_plus_i & 0xff00) >> 8);
		//printf("a_plus_i %04x\t rotated %04x\n",a_plus_i,Rotated_a_plus_i);
		SboxInput = (aL + i); //printf("%d\n",SboxInput);
		StateGuess.SBox[SboxInput] = (PlainText[35]<<8)^PlainText[34]^(PlainText[36*i+35]<<8)^PlainText[36*i+34]^StateGuess.SBox[aL]^Rotated_a^Rotated_a_plus_i;
      }      
      //for (i=1;i<256;i++) printf("%02x\t Actual: %04x \t Guess: %04x\n",i,State.SBox[i],StateGuess.SBox[i]);      
      // Print the solution if it's the correct one:
      EverythingCorrect = 1;

      for (i=0;i<256;i++)
	  {
		if (State.SBox[i] != StateGuess.SBox[i])
			{
				EverythingCorrect = 0; 		
				i = 256;
			}
	  }
      
	  if (EverythingCorrect)
	  {
		printf("The key has been recovered entirely!\n"); 
		for (i = 0; i < 256; i++) 
			std::cout<<i<<"\t"<<State.SBox[i]<<"\t"<<StateGuess.SBox[i]<<std::endl;
			//printf("%02x\t Actual: %04d \t Guess: %04d\n",i,State.SBox[i],StateGuess.SBox[i]);
		ctr_aL = 256; 
		ctr_SaL = 65536; // we end here // - we now don't do it to estimate the time...
      }

    }
  }
  getchar();
  return 1;
}

