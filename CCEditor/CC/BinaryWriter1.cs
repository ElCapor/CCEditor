using CCEditor.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.CC
{
    public class BinaryWriter1 : BinaryWriter
    {
        private byte[] primitiveBuffer = new byte[0];

        public BinaryWriter1(Stream stream)
            : base(stream)
        {
        }

        public new void Write7BitEncodedInt(int i)
        {
            base.Write7BitEncodedInt(i);
        }

        private void VerifyBufferLength(int byteCount)
        {
            if (byteCount > primitiveBuffer.Length)
            {
                int num = Helpers.Helpers.BinarySearch(StaticVariables.powersOf2, byteCount);
                primitiveBuffer = new byte[StaticVariables.powersOf2[num]];
            }
        }

        public void WriteFromPrimitiveArray(Array arr)
        {
            Type elementType = arr.GetType().GetElementType();
            int num = ((elementType == typeof(bool)) ? 1 : Marshal.SizeOf(elementType)) * arr.GetShape().GetTotalCount();
            VerifyBufferLength(num);
            Buffer.BlockCopy(arr, 0, primitiveBuffer, 0, num);
            base.Write(primitiveBuffer, 0, num);
        }
    }

}
