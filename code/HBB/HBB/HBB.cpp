// HBB.cpp: определяет точку входа для консольного приложения.
//
#include "stdafx.h"
#include <stdio.h>
#include <string.h>
#include <time.h>
#include <cstdlib>

/*  Constatns START*/
#define RULE00 0x80ffaf46
#define RULE01 0x977969e9
#define RULE02 0x71553bb5
#define RULE03 0x99be6b2b
#define RULE04 0x4b337295
#define RULE05 0x2308c787
#define RULE06 0xb84c7cce
#define RULE07 0x36d501e6

#define RULE10 0xdd18c62b
#define RULE11 0x153df31a
#define RULE12 0xc98e86c1
#define RULE13 0x910fee24
#define RULE14 0x2942d51b
#define RULE15 0x4201eb3d
#define RULE16 0xc1d1a85f
#define RULE17 0x57b8919b

#define MASK0 (0x55555555)
#define MASK1 (0x33333333)
/*  Constatns END*/


/* Macros START*/

#define FOLD \
	{ \
	if (KEYLEN == 256) { \
	tmp0 = KEY[0] ^ KEY[2] ^ KEY[4] ^ KEY[6]; \
	tmp1 = KEY[1] ^ KEY[3] ^ KEY[5] ^ KEY[7]; \
	NLC0.word = tmp0; NLC1.word = tmp1; \
	NLC2.word = ~tmp0; NLC3.word = ~tmp1; \
	} \
	else { \
	tmp0 = KEY[0] ^ KEY[2]; \
	tmp1 = KEY[1] ^ KEY[3]; \
	NLC0.word = tmp0; NLC1.word = tmp1; \
	NLC2.word = ~tmp0; NLC3.word = ~tmp1; \
	} \
	}

#define EXP \
	{ \
	if (KEYLEN == 256) { \
	state00 = KEY[0]; state01 = KEY[1]; state02 = KEY[2]; state03 = KEY[3]; \
	state04 = KEY[4]; state05 = KEY[5]; state06 = KEY[6]; state07 = KEY[7]; \
	state10 = ~KEY[0]; state11 = ~KEY[1]; state12 = ~KEY[2]; state13 = ~KEY[3]; \
	state14 = ~KEY[4]; state15 = ~KEY[5]; state16 = ~KEY[6]; state17 = ~KEY[7]; \
	} \
	else { \
	state00 = state14 = KEY[0]; state01 = state15 = KEY[1]; \
	state02 = state16 = KEY[2]; state03 = state17 = KEY[3]; \
	state04 = state10 = ~KEY[0]; state05 = state11 = ~KEY[1]; \
	state06 = state12 = ~KEY[2]; state07 = state13 = ~KEY[3]; \
	} \
	}

#define CLShift(A,I) (((A) >> (32 - I)) ^ ((A) << (I)))

#define EvolveCA256(ca) \
	{\
	tmp0 = ((state##ca##0 << 1) ^ (state##ca##1 >> 31)) ^ (RULE##ca##0 & state##ca##0) ^ (state##ca##0 >> 1);\
	tmp1 = ((state##ca##1 << 1) ^ (state##ca##2 >> 31)) ^ (RULE##ca##1 & state##ca##1) ^ ((state##ca##1 >> 1) ^ (state##ca##0 << 31));\
	tmp2 = ((state##ca##2 << 1) ^ (state##ca##3 >> 31)) ^ (RULE##ca##2 & state##ca##2) ^ ((state##ca##2 >> 1) ^ (state##ca##1 << 31));\
	tmp3 = ((state##ca##3 << 1) ^ (state##ca##4 >> 31)) ^ (RULE##ca##3 & state##ca##3) ^ ((state##ca##3 >> 1) ^ (state##ca##2 << 31));\
	tmp4 = ((state##ca##4 << 1) ^ (state##ca##5 >> 31)) ^ (RULE##ca##4 & state##ca##4) ^ ((state##ca##4 >> 1) ^ (state##ca##3 << 31));\
	tmp5 = ((state##ca##5 << 1) ^ (state##ca##6 >> 31)) ^ (RULE##ca##5 & state##ca##5) ^ ((state##ca##5 >> 1) ^ (state##ca##4 << 31));\
	tmp6 = ((state##ca##6 << 1) ^ (state##ca##7 >> 31)) ^ (RULE##ca##6 & state##ca##6) ^ ((state##ca##6 >> 1) ^ (state##ca##5 << 31));\
	tmp7 = (state##ca##7 << 1) ^ (RULE##ca##7 & state##ca##7) ^ ((state##ca##7 >> 1) ^ (state##ca##6 << 31));\
	state##ca##0 = tmp0; state##ca##1 = tmp1; state##ca##2 = tmp2; state##ca##3 = tmp3; \
	state##ca##4 = tmp4; state##ca##5 = tmp5; state##ca##6 = tmp6; state##ca##7 = tmp7; \
	}

#define transpose32(arr0,arr1,arr2,arr3)\
	{\
	tmp0 = arr0 & MASK0; tmp1 = arr0 & ~MASK0; tmp2 = arr1 & MASK0; tmp3 = arr1 & ~MASK0; \
	tmp0 = tmp0 << 1; tmp3 = tmp3 >> 1; arr0 = tmp1 ^ tmp3; arr1 = tmp0 ^ tmp2; \
	\
	tmp0 = arr2 & MASK0; tmp1 = arr2 & ~MASK0; tmp2 = arr3 & MASK0; tmp3 = arr3 & ~MASK0; \
	tmp0 = tmp0 << 1; tmp3 = tmp3 >> 1; arr2 = tmp1 ^ tmp3; arr3 = tmp0 ^ tmp2; \
	\
	tmp0 = arr0 & MASK1; tmp1 = arr0 & ~MASK1; tmp2 = arr2 & MASK1; tmp3 = arr2 & ~MASK1; \
	tmp0 = tmp0 << 2; tmp3 = tmp3 >> 2; arr0 = tmp1 ^ tmp3; arr2 = tmp0 ^ tmp2; \
	\
	tmp0 = arr1 & MASK1; tmp1 = arr1 & ~MASK1; tmp2 = arr3 & MASK1; tmp3 = arr3 & ~MASK1; \
	tmp0 = tmp0 << 2; tmp3 = tmp3 >> 2; arr1 = tmp1 ^ tmp3; arr3 = tmp0 ^ tmp2; \
	}

#define Round \
	{ \
	NLSub; \
	tmp0 = NLC0.word ^ NLC1.word ^ NLC2.word ^ NLC3.word; \
	NLC0.word = CLShift(tmp0^NLC0.word,4); \
	NLC1.word = CLShift(tmp0^NLC1.word,12); \
	NLC2.word = CLShift(tmp0^NLC2.word,20); \
	NLC3.word = CLShift(tmp0^NLC3.word,28); \
	transpose32(NLC0.word,NLC1.word,NLC2.word,NLC3.word); \
	NLSub; \
	EvolveCA256(0);\
	EvolveCA256(1);\
	KEYSTREAM0 = NLC0.word ^ state00;\
	KEYSTREAM1 = NLC1.word ^ state07;\
	KEYSTREAM2 = NLC2.word ^ state10;\
	KEYSTREAM3 = NLC3.word ^ state17;\
	NLC0.word = NLC0.word ^ state03;\
	NLC1.word = NLC1.word ^ state04;\
	NLC2.word = NLC2.word ^ state13;\
	NLC3.word = NLC3.word ^ state14;\
	}

#define NLSub \
	NLC0.bytes.byte0 = byteSub[NLC0.bytes.byte0];\
	NLC0.bytes.byte1 = byteSub[NLC0.bytes.byte1];\
	NLC0.bytes.byte2 = byteSub[NLC0.bytes.byte2];\
	NLC0.bytes.byte3 = byteSub[NLC0.bytes.byte3];\
	NLC1.bytes.byte0 = byteSub[NLC1.bytes.byte0];\
	NLC1.bytes.byte1 = byteSub[NLC1.bytes.byte1];\
	NLC1.bytes.byte2 = byteSub[NLC1.bytes.byte2];\
	NLC1.bytes.byte3 = byteSub[NLC1.bytes.byte3];\
	NLC2.bytes.byte0 = byteSub[NLC2.bytes.byte0];\
	NLC2.bytes.byte1 = byteSub[NLC2.bytes.byte1];\
	NLC2.bytes.byte2 = byteSub[NLC2.bytes.byte2];\
	NLC2.bytes.byte3 = byteSub[NLC2.bytes.byte3];\
	NLC3.bytes.byte0 = byteSub[NLC3.bytes.byte0];\
	NLC3.bytes.byte1 = byteSub[NLC3.bytes.byte1];\
	NLC3.bytes.byte2 = byteSub[NLC3.bytes.byte2];\
	NLC3.bytes.byte3 = byteSub[NLC3.bytes.byte3];\
/* Macros END*/

typedef union {
	unsigned long word;
	struct {
		unsigned char byte0, byte1, byte2, byte3;
	} bytes;
} NLCWORD;

int KEYLEN;
unsigned long KBLK; // number of keystream blocks
unsigned long KEY[8]; // secret key read from file

// S-Box
static const unsigned char byteSub[256] = 
{
0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76,
0xca, 0x82, 0xc9, 0x7b, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0,
0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15,
0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75,
0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84,
0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xd1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf,
0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8,
0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2,
0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73,
0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb,
0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79,
0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08,
0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a,
0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e,
0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf,
0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16
};

int _tmain(int argc, char* argv[])
{
	register NLCWORD NLC0, NLC1, NLC2, NLC3;
	register unsigned long state00=0, state01=0, state02=0, state03=0;
	register unsigned long state04=0, state05=0, state06=0, state07=0;
	register unsigned long state10=0, state11=0, state12=0, state13=0;
	register unsigned long state14=0, state15=0, state16=0, state17=0;

	register unsigned long KEYSTREAM0, KEYSTREAM1, KEYSTREAM2, KEYSTREAM3;
	register unsigned long tmp0=0, tmp1=0, tmp2=0, tmp3=0, tmp4=0, tmp5=0, tmp6=0, tmp7=0;
	unsigned long T[4][4], C[4], M[4];
	int i;
	clock_t t1,t2;
	FILE *fp;

	if (argc < 3)
	{
		printf("Usage: %s <# of key blocks> <key file name>\n", argv[0]); 
		exit(0);
	}

	KBLK = atol(argv[1]);
	printf("KBLK = %ld\n", KBLK);

	fp = fopen(argv[2],"r");
	if (fp == NULL) 
	{
		printf("Cannot open %s\n", argv[2]); 
		exit(0);
	}

	fscanf(fp, "%d\n",&KEYLEN);
	if ((KEYLEN != 128)&&(KEYLEN != 256))
	{
		printf("invalid length of secret key\n");
		exit(0);
	}

	for (i=0; i<KEYLEN/32; i++)
		fscanf(fp, "%x",&KEY[i]);
	KEYSTREAM0 = KEYSTREAM1 = KEYSTREAM2 = KEYSTREAM3 = 0;

	// start of key setup
	EXP FOLD
	for (i=0; i<=12; i++) Round

	T[0][0] = KEYSTREAM0; T[0][1] = KEYSTREAM1; T[0][2] = KEYSTREAM2; T[0][3] = KEYSTREAM3;
	Round
	T[1][0] = KEYSTREAM0; T[1][1] = KEYSTREAM1; T[1][2] = KEYSTREAM2; T[1][3] = KEYSTREAM3;
	Round
	T[2][0] = KEYSTREAM0; T[2][1] = KEYSTREAM1; T[2][2] = KEYSTREAM2; T[2][3] = KEYSTREAM3;
	Round
	T[3][0] = KEYSTREAM0; T[3][1] = KEYSTREAM1; T[3][2] = KEYSTREAM2; T[3][3] = KEYSTREAM3;
	Round

	state00 = state00 ^ T[3][0]; state01 = state01 ^ T[3][1];
	state02 = state02 ^ T[3][2]; state03 = state03 ^ T[3][3];
	state04 = state04 ^ T[2][0]; state05 = state05 ^ T[2][1];
	state06 = state06 ^ T[2][2]; state07 = state07 ^ T[2][3];
	state10 = state10 ^ T[1][0]; state11 = state11 ^ T[1][1];
	state12 = state12 ^ T[1][2]; state13 = state13 ^ T[1][3];
	state14 = state14 ^ T[0][0]; state15 = state15 ^ T[0][1];
	state16 = state16 ^ T[0][2]; state17 = state17 ^ T[0][3];
	// END of key setup

	t1 = clock();
	
	// key generation
	for (i=0; i<KBLK; i++)
	{
		Round
		// encryption
		C[0] = M[0] ^ KEYSTREAM0;
		C[1] = M[1] ^ KEYSTREAM1;
		C[2] = M[2] ^ KEYSTREAM2;
		C[3] = M[3] ^ KEYSTREAM3;
	} // end of key generation

	t2 = clock();

	printf("CLK_TCK = %d, CLOCKS_PER_SEC = %d\n", CLK_TCK, CLOCKS_PER_SEC);
	printf("clocks elapsed = %d, time elaplsed = %f\n",t2-t1,(float)(t2-t1)/(float)CLOCKS_PER_SEC);

	return 0;
}

