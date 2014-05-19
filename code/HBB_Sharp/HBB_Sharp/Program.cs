using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBB_Sharp
{
    public class Program
    {
        public static UInt32[] KEY = new UInt32[4] { 1, 5, 34, 23 }; // 4 * 4 = 16 bytes = 16 * 8 = 128 bits
        public static UInt32[] KeyStream = new UInt32[4] { 0, 0, 0, 0 }; // refreshes on each round function call
        public static UInt32[] M = new UInt32[24] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        public static UInt32[] C = new UInt32[24] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<Block> Messages = new List<Block>();
        public static List<Block> CipherBlocks = new List<Block>();

        static void Main(string[] args)
        {
            CipherHelpers.InitBlockList(Messages, ref M);
            CipherHelpers.InitBlockList(CipherBlocks, ref C);

            CipherHelpers.HBB(CipherHelpers.Action.Encrypt, Messages, CipherBlocks);
            CipherHelpers.PrintBlocks(Messages);
            CipherHelpers.HBB(CipherHelpers.Action.Decrypt, Messages, CipherBlocks);
            CipherHelpers.PrintBlocks(Messages);
        }
    }
}
