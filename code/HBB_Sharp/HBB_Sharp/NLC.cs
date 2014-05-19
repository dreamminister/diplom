using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace HBB_Sharp
{
    [StructLayoutAttribute(LayoutKind.Explicit)]
    public class NLC
    {
        [FieldOffsetAttribute(0)]
        public UInt32 word;
        
        [FieldOffsetAttribute(0)]
        public byte byte0;

        [FieldOffsetAttribute(1)]
        public byte byte1;

        [FieldOffsetAttribute(2)]
        public byte byte2;

        [FieldOffsetAttribute(3)]
        public byte byte3;
    }
}
