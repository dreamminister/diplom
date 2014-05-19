using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace HBB_Sharp
{
    public enum CAorder
    {
        first = 1,
        second = 2
    }

    
    public class CA
    {
        public UInt32 state0;
        public UInt32 state1;
        public UInt32 state2;
        public UInt32 state3;
        public UInt32 state4;
        public UInt32 state5;
        public UInt32 state6;
        public UInt32 state7;

        CAorder order;

        UInt32 RULE00 = 0x80ffaf46;
        UInt32 RULE01 = 0x977969e9;
        UInt32 RULE02 = 0x71553bb5;
        UInt32 RULE03 = 0x99be6b2b;
        UInt32 RULE04 = 0x4b337295;
        UInt32 RULE05 = 0x2308c787;
        UInt32 RULE06 = 0xb84c7cce;
        UInt32 RULE07 = 0x36d501e6;

        UInt32 RULE10 = 0xdd18c62b;
        UInt32 RULE11 = 0x153df31a;
        UInt32 RULE12 = 0xc98e86c1;
        UInt32 RULE13 = 0x910fee24;
        UInt32 RULE14 = 0x2942d51b;
        UInt32 RULE15 = 0x4201eb3d;
        UInt32 RULE16 = 0xc1d1a85f;
        UInt32 RULE17 = 0x57b8919b;

        public CA(CAorder InOrder)
        {
            state0 = 0;
            state1 = 0;
            state2 = 0;
            state3 = 0;
            state4 = 0;
            state5 = 0;
            state6 = 0;
            state7 = 0;
            order = InOrder;
        }

        public void Exp()
        {
            if (order == CAorder.first)
            {
                state0 = Program.KEY[0];
                state1 = Program.KEY[1];
                state2 = Program.KEY[2];
                state3 = Program.KEY[3];
                state4 = ~Program.KEY[0];
                state5 = ~Program.KEY[1];
                state6 = ~Program.KEY[2];
                state7 = ~Program.KEY[3];
            }
            else
            {
                state0 = ~Program.KEY[0];
                state1 = ~Program.KEY[1];
                state2 = ~Program.KEY[2];
                state3 = ~Program.KEY[3];
                state4 = Program.KEY[0];
                state5 = Program.KEY[1];
                state6 = Program.KEY[2];
                state7 = Program.KEY[3];
            }


        }

        public void MergeWithCipher(UInt32 C0, UInt32 C1, UInt32 C2, UInt32 C3) 
        {
            state7 = state7 ^ C3;
            state5 = state5 ^ C2;
            state3 = state3 ^ C1;
            state1 = state1 ^ C0;
        }

        public void EvolveCA256()
        {
            if (order == CAorder.first)
            {
                UInt32 tmp0 = ((state0 << 1) ^ (state1 >> 31)) ^ (RULE00 & state0) ^ (state0 >> 1);
                UInt32 tmp1 = ((state1 << 1) ^ (state2 >> 31)) ^ (RULE01 & state1) ^ ((state1 >> 1) ^ (state0 << 31));
                UInt32 tmp2 = ((state2 << 1) ^ (state3 >> 31)) ^ (RULE02 & state2) ^ ((state2 >> 1) ^ (state1 << 31));
                UInt32 tmp3 = ((state3 << 1) ^ (state4 >> 31)) ^ (RULE03 & state3) ^ ((state3 >> 1) ^ (state2 << 31));
                UInt32 tmp4 = ((state4 << 1) ^ (state5 >> 31)) ^ (RULE04 & state4) ^ ((state4 >> 1) ^ (state3 << 31));
                UInt32 tmp5 = ((state5 << 1) ^ (state6 >> 31)) ^ (RULE05 & state5) ^ ((state5 >> 1) ^ (state4 << 31));
                UInt32 tmp6 = ((state6 << 1) ^ (state7 >> 31)) ^ (RULE06 & state6) ^ ((state6 >> 1) ^ (state5 << 31));
                UInt32 tmp7 = (state7 << 1) ^ (RULE07 & state7) ^ ((state7 >> 1) ^ (state6 << 31));
                state0 = tmp0; state1 = tmp1; state2 = tmp2; state3 = tmp3;
                state4 = tmp4; state5 = tmp5; state6 = tmp6; state7 = tmp7;
            }
            else
            {
                UInt32 tmp0 = ((state0 << 1) ^ (state1 >> 31)) ^ (RULE10 & state0) ^ (state0 >> 1);
                UInt32 tmp1 = ((state1 << 1) ^ (state2 >> 31)) ^ (RULE11 & state1) ^ ((state1 >> 1) ^ (state0 << 31));
                UInt32 tmp2 = ((state2 << 1) ^ (state3 >> 31)) ^ (RULE12 & state2) ^ ((state2 >> 1) ^ (state1 << 31));
                UInt32 tmp3 = ((state3 << 1) ^ (state4 >> 31)) ^ (RULE13 & state3) ^ ((state3 >> 1) ^ (state2 << 31));
                UInt32 tmp4 = ((state4 << 1) ^ (state5 >> 31)) ^ (RULE14 & state4) ^ ((state4 >> 1) ^ (state3 << 31));
                UInt32 tmp5 = ((state5 << 1) ^ (state6 >> 31)) ^ (RULE15 & state5) ^ ((state5 >> 1) ^ (state4 << 31));
                UInt32 tmp6 = ((state6 << 1) ^ (state7 >> 31)) ^ (RULE16 & state6) ^ ((state6 >> 1) ^ (state5 << 31));
                UInt32 tmp7 = (state7 << 1) ^ (RULE17 & state7) ^ ((state7 >> 1) ^ (state6 << 31));
                state0 = tmp0; state1 = tmp1; state2 = tmp2; state3 = tmp3;
                state4 = tmp4; state5 = tmp5; state6 = tmp6; state7 = tmp7;
            }
        }
    }
}
