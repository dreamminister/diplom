using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace HBB_Sharp
{
    [StructLayoutAttribute(LayoutKind.Explicit)]
    public class Block
    {
        [FieldOffsetAttribute(0)]
        public UInt32 first;
        [FieldOffsetAttribute(4)]
        public UInt32 second;
        [FieldOffsetAttribute(8)]
        public UInt32 third;
        [FieldOffsetAttribute(12)]
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
