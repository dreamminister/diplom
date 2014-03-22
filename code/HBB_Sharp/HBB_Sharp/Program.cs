using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBB_Sharp
{
    public class Program
    {
        public static UInt32[] KEY = new UInt32[4] { 0, 0, 0, 0 };
        public static UInt32[] KeyStream = new UInt32[4] { 0, 0, 0, 0 };

        static void Main(string[] args)
        {
            NLC NLC0 = new NLC(); NLC NLC1 = new NLC(); NLC NLC2 = new NLC(); NLC NLC3 = new NLC();
            CA FirstCa = new CA(CAorder.first);
            CA SecondCa = new CA(CAorder.second);
            //UInt32[,] T = new UInt32[4, 4];
            UInt32[] M = new UInt32[4] { 1, 2, 3, 4 };
            UInt32[] C = new UInt32[4];
            //int NumberOfKeyBlocks = 1;
            FirstCa.Exp();
            SecondCa.Exp();
            CipherHelpers.Fold(ref NLC0, ref NLC1, ref NLC2, ref NLC3);

            for (int i = 0; i <= 12; i++)
            {
                CipherHelpers.Round(FirstCa, SecondCa, ref NLC0, ref NLC1, ref NLC2, ref NLC3);
            }

            CipherHelpers.KeySetup(FirstCa, SecondCa, ref NLC0, ref NLC1, ref NLC2, ref NLC3);
            CipherHelpers.Encrypt(FirstCa, SecondCa, ref NLC0, ref NLC1, ref NLC2, ref NLC3, ref M, ref C);
        }
    }
}
