using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.Classes
{
    public class BitArray
    {
        public byte[] backBone { get; private set; } = new byte[0];


        public int Length => backBone.Length << 3;

        public bool this[int i]
        {
            get
            {
                return GetValue(i);
            }
            set
            {
                SetValue(i, value);
            }
        }

        public BitArray()
        {
        }

        public BitArray(byte[] backBone)
        {
            this.backBone = backBone;
        }

        private bool GetValue(int i)
        {
            int num = i / 8;
            byte b = (byte)(1 << i % 8);
            return (backBone[num] & b) != 0;
        }

        private void SetValue(int i, bool val)
        {
            int num = i / 8;
            byte b = (byte)(1 << i % 8);
            if (val)
            {
                backBone[num] |= b;
            }
            else
            {
                backBone[num] &= (byte)(~b & 0xFF);
            }
        }
    }

}
