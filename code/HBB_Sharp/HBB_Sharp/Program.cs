using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBB_Sharp
{
    public class Program
    {
        public static UInt32[] KEY = new UInt32[4] { 1, 0, 0, 0 };
        public static UInt32[] KeyStream = new UInt32[4] { 0, 0, 0, 0 };
        public static UInt32[] M = new UInt32[12] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        public static UInt32[] C = new UInt32[16];

        static void Main(string[] args)
        {
            CipherHelpers.HBB(CipherHelpers.Action.Encrypt);
            Console.WriteLine(M[0] + " " + M[1] + " " + M[2] + " " + M[3] + M[4] + " " + M[5] + " " + M[6] + " " + M[7] + M[8] + " " + M[9] + " " + M[10] + " " + M[11]);
            CipherHelpers.HBB(CipherHelpers.Action.Decrypt);
            Console.WriteLine(M[0] + " " + M[1] + " " + M[2] + " " + M[3] + M[4] + " " + M[5] + " " + M[6] + " " + M[7] + M[8] + " " + M[9] + " " + M[10] + " " + M[11]);
        }
    }
}
