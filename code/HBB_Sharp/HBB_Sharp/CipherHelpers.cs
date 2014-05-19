using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBB_Sharp
{
    public static class CipherHelpers
    {
        public static UInt32 MASK0 = 0x55555555;
        public static UInt32 MASK1 = 0x33333333;

        // S-Box
        public static byte[] byteSub = new byte[256]
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

        public static void NLSub(List<NLC> allNLC)
        {
            allNLC[0].byte0 = byteSub[allNLC[0].byte0];
            allNLC[0].byte1 = byteSub[allNLC[0].byte1];
            allNLC[0].byte2 = byteSub[allNLC[0].byte2];
            allNLC[0].byte3 = byteSub[allNLC[0].byte3];
            allNLC[1].byte0 = byteSub[allNLC[1].byte0];
            allNLC[1].byte1 = byteSub[allNLC[1].byte1];
            allNLC[1].byte2 = byteSub[allNLC[1].byte2];
            allNLC[1].byte3 = byteSub[allNLC[1].byte3];
            allNLC[2].byte0 = byteSub[allNLC[2].byte0];
            allNLC[2].byte1 = byteSub[allNLC[2].byte1];
            allNLC[2].byte2 = byteSub[allNLC[2].byte2];
            allNLC[2].byte3 = byteSub[allNLC[2].byte3];
            allNLC[3].byte0 = byteSub[allNLC[3].byte0];
            allNLC[3].byte1 = byteSub[allNLC[3].byte1];
            allNLC[3].byte2 = byteSub[allNLC[3].byte2];
            allNLC[3].byte3 = byteSub[allNLC[3].byte3];
        }

        public static UInt32 CLShift(UInt32 A, int I)
        {
            UInt32 result = (((A) >> (32 - I)) ^ ((A) << (I)));
            return result;
        }

        public static void Fold(List<NLC> allNLC)
        {
            UInt32 tmp0 = Program.KEY[0] ^ Program.KEY[2];
            UInt32 tmp1 = Program.KEY[1] ^ Program.KEY[3];
            allNLC[0].word = tmp0;
            allNLC[1].word = tmp1;
            allNLC[2].word = ~tmp0;
            allNLC[3].word = ~tmp1;
        }

        public static void transpose32(ref UInt32 arr0, ref UInt32 arr1, ref UInt32 arr2, ref UInt32 arr3)
        {
            UInt32 tmp0 = arr0 & MASK0;
            UInt32 tmp1 = arr0 & ~MASK0;
            UInt32 tmp2 = arr1 & MASK0;
            UInt32 tmp3 = arr1 & ~MASK0;

            tmp0 = tmp0 << 1;
            tmp3 = tmp3 >> 1;
            arr0 = tmp1 ^ tmp3;
            arr1 = tmp0 ^ tmp2;

            tmp0 = arr2 & MASK0;
            tmp1 = arr2 & ~MASK0;
            tmp2 = arr3 & MASK0;
            tmp3 = arr3 & ~MASK0;

            tmp0 = tmp0 << 1;
            tmp3 = tmp3 >> 1;
            arr2 = tmp1 ^ tmp3;
            arr3 = tmp0 ^ tmp2;

            tmp0 = arr0 & MASK1;
            tmp1 = arr0 & ~MASK1;
            tmp2 = arr2 & MASK1;
            tmp3 = arr2 & ~MASK1;

            tmp0 = tmp0 << 2;
            tmp3 = tmp3 >> 2;
            arr0 = tmp1 ^ tmp3;
            arr2 = tmp0 ^ tmp2;

            tmp0 = arr1 & MASK1;
            tmp1 = arr1 & ~MASK1;
            tmp2 = arr3 & MASK1;
            tmp3 = arr3 & ~MASK1;

            tmp0 = tmp0 << 2;
            tmp3 = tmp3 >> 2;
            arr1 = tmp1 ^ tmp3;
            arr3 = tmp0 ^ tmp2;
        }

        public static void Round(CA FirstCA, CA SecondCA, List<NLC> allNLC)
	    {
            NLSub(allNLC);
            UInt32 tmp0 = allNLC[0].word ^ allNLC[1].word ^ allNLC[2].word ^ allNLC[3].word;
            allNLC[0].word = CLShift(tmp0 ^ allNLC[0].word, 4);
            allNLC[1].word = CLShift(tmp0 ^ allNLC[1].word, 12);
            allNLC[2].word = CLShift(tmp0 ^ allNLC[2].word, 20);
            allNLC[3].word = CLShift(tmp0 ^ allNLC[3].word, 28);
            transpose32(ref allNLC[0].word, ref allNLC[1].word, ref allNLC[2].word, ref allNLC[3].word);
            NLSub(allNLC);
            FirstCA.EvolveCA256();
            SecondCA.EvolveCA256();
            Program.KeyStream[0] = allNLC[0].word ^ FirstCA.state0;
            Program.KeyStream[1] = allNLC[1].word ^ FirstCA.state7;
            Program.KeyStream[2] = allNLC[2].word ^ SecondCA.state0;
            Program.KeyStream[3] = allNLC[3].word ^ SecondCA.state7;
            allNLC[0].word = allNLC[0].word ^ FirstCA.state3;
            allNLC[1].word = allNLC[1].word ^ FirstCA.state4;
            allNLC[2].word = allNLC[2].word ^ SecondCA.state3;
            allNLC[3].word = allNLC[3].word ^ SecondCA.state4;
	    }

        public static void KeySetup(CA FirstCA, CA SecondCA, List<NLC> allNLC)
        {
            UInt32[,] Temp = new UInt32[4, 4];
            Temp[0,0] = Program.KeyStream[0]; Temp[0,1] = Program.KeyStream[1]; Temp[0,2] = Program.KeyStream[2]; Temp[0,3] = Program.KeyStream[3];
            Round(FirstCA, SecondCA, allNLC);
            Temp[1, 0] = Program.KeyStream[0]; Temp[1, 1] = Program.KeyStream[1]; Temp[1, 2] = Program.KeyStream[2]; Temp[1, 3] = Program.KeyStream[3];
            Round(FirstCA, SecondCA, allNLC);
            Temp[2, 0] = Program.KeyStream[0]; Temp[2, 1] = Program.KeyStream[1]; Temp[2, 2] = Program.KeyStream[2]; Temp[2, 3] = Program.KeyStream[3];
            Round(FirstCA, SecondCA, allNLC);
            Temp[3, 0] = Program.KeyStream[0]; Temp[3, 1] = Program.KeyStream[1]; Temp[3, 2] = Program.KeyStream[2]; Temp[3, 3] = Program.KeyStream[3];
            Round(FirstCA, SecondCA, allNLC);

            FirstCA.state0 = FirstCA.state0 ^ Temp[3, 0]; 
            FirstCA.state1 = FirstCA.state1 ^ Temp[3, 1];
            FirstCA.state2 = FirstCA.state2 ^ Temp[3, 2]; 
            FirstCA.state3 = FirstCA.state3 ^ Temp[3, 3];
            FirstCA.state4 = FirstCA.state4 ^ Temp[2, 0]; 
            FirstCA.state5 = FirstCA.state5 ^ Temp[2, 1];
            FirstCA.state6 = FirstCA.state6 ^ Temp[2, 2]; 
            FirstCA.state7 = FirstCA.state7 ^ Temp[2, 3];
            SecondCA.state0 = SecondCA.state0 ^ Temp[1, 0]; 
            SecondCA.state1 = SecondCA.state1 ^ Temp[1, 1];
            SecondCA.state2 = SecondCA.state2 ^ Temp[1, 2]; 
            SecondCA.state3 = SecondCA.state3 ^ Temp[1, 3];
            SecondCA.state4 = SecondCA.state4 ^ Temp[0, 0]; 
            SecondCA.state5 = SecondCA.state5 ^ Temp[0, 1];
            SecondCA.state6 = SecondCA.state6 ^ Temp[0, 2]; 
            SecondCA.state7 = SecondCA.state7 ^ Temp[0, 3];
        }

        public static void InitBlockList(List<Block> BlockList, ref UInt32[] inArray) 
        {
            int NumberOfBlocks = inArray.Length / 4;

            for (int i = 0; i < inArray.Length; i = i + 4) 
            {
                Block currentBlock = new Block(ref inArray, i);
                BlockList.Add(currentBlock);
            }
        }

        public static void Encrypt(CA FirstCA, CA SecondCA, List<NLC> allNLC, List<Block> Messages, List<Block> Ciphers) 
        {
            int BlockNumber = Messages.Count;

            for (int i = 0; i < BlockNumber; i++)
	        {
                Round(FirstCA, SecondCA, allNLC);
		        // encryption
                Ciphers[i].first    = Messages[i].first  ^ Program.KeyStream[0];
                Ciphers[i].second   = Messages[i].second ^ Program.KeyStream[1];
                Ciphers[i].third    = Messages[i].third  ^ Program.KeyStream[2];
                Ciphers[i].fourth   = Messages[i].fourth ^ Program.KeyStream[3];
	        } // end of key generation
        }

        public static void Decrypt(CA FirstCA, CA SecondCA, List<NLC> allNLC, List<Block> Messages, List<Block> Ciphers) 
        {
            int BlockNumber = Messages.Count;

            for (int i = 0; i < BlockNumber; i++)
            {
                Round(FirstCA, SecondCA, allNLC);
                // decryption
                Messages[i].first   = Ciphers[i].first  ^ Program.KeyStream[0];
                Messages[i].second  = Ciphers[i].second ^ Program.KeyStream[1];
                Messages[i].third   = Ciphers[i].third  ^ Program.KeyStream[2];
                Messages[i].fourth  = Ciphers[i].fourth ^ Program.KeyStream[3];
            }
        }

        public enum Action 
        {
            Encrypt = 1,
            Decrypt = 2
        }

        public static List<NLC> NLC_generate(int count)
        {
            List<NLC> allNLC = new List<NLC>();
            for (int i = 0; i < count; i++) 
            {
                allNLC.Add(new NLC());
            }
            return allNLC;
        }

        public static void HBB(Action action, List<Block> Messages, List<Block> Ciphers) 
        {
            List<NLC> allNLC = NLC_generate(4);
            CA FirstCa = new CA(CAorder.first); CA SecondCa = new CA(CAorder.second);
            
            FirstCa.Exp(); SecondCa.Exp();

            CipherHelpers.Fold(allNLC);

            for (int i = 0; i <= 12; i++)
            {
                CipherHelpers.Round(FirstCa, SecondCa, allNLC);
            }

            CipherHelpers.KeySetup(FirstCa, SecondCa, allNLC);

            if (action == Action.Encrypt)
            {
                CipherHelpers.Encrypt(FirstCa, SecondCa, allNLC, Messages, Ciphers);
            }
            else if (action == Action.Decrypt)
            {
                CipherHelpers.Decrypt(FirstCa, SecondCa, allNLC, Messages, Ciphers);
            }
        }

        public static void PrintBlocks(List<Block> Blocks) 
        {
            string coma = ", ";
            foreach (Block block in Blocks)
                Console.Write(block.first + coma + block.second + coma + block.third + coma + block.fourth + coma);
            Console.WriteLine();
        }

        public static void SSmode(CA FirstCA, CA SecondCA, List<NLC> allNLC, List<Block> Messages, List<Block> Ciphers, int index)
        {
            // Place for SS mode
            // 1. Exp()
            // 2. LC[i] XOR C[i] || ... || C[i-3] => block
            // 3. Fold()
            // 4. NLC[i] XOR C[i] XOR ... XOR C[i-3]  => block
            FirstCA.Exp(); SecondCA.Exp(); // Exp(KEY)
        }
    }
}
