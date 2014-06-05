using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace HBB_Sharp
{
    public class Block
    {
        public UInt32 first;
        public UInt32 second;
        public UInt32 third;
        public UInt32 fourth;

        public Block(ref UInt32[] initBlock, int index) 
        {
            first   = initBlock[index + 0];
            second  = initBlock[index + 1];
            third   = initBlock[index + 2];
            fourth  = initBlock[index + 3];
        }

        public Block() 
        {
            first   = 0;
            second  = 0;
            third   = 0;
            fourth  = 0;
        }
    }
}
