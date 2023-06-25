using System;
using System.IO;
using System.Runtime.InteropServices;
using CCEditor.Helpers;
public class BinaryReader1 : BinaryReader
{
    private byte[] primitiveBuffer = new byte[0];

    public BinaryReader1(Stream stream)
        : base(stream)
    {
    }

    public new int Read7BitEncodedInt()
    {
        return base.Read7BitEncodedInt();
    }

    private void VerifyBufferLength(int byteCount)
    {
        if (byteCount > primitiveBuffer.Length)
        {
            int num = Helpers.BinarySearch(StaticVariables.powersOf2, byteCount);
            primitiveBuffer = new byte[StaticVariables.powersOf2[num]];
        }
    }

    public void ReadIntoPrimitiveArray(Array arr)
    {
        Type type = arr.GetType();
        Type elementType = type.GetElementType();
        if (type == typeof(byte[]))
        {
            base.Read((byte[])arr, 0, arr.Length);
            return;
        }
        int num = ((elementType == typeof(bool)) ? 1 : Marshal.SizeOf(elementType));
        int num2 = arr.GetShape().GetTotalCount() * num;
        VerifyBufferLength(num2);
        base.Read(primitiveBuffer, 0, num2);
        Buffer.BlockCopy(primitiveBuffer, 0, arr, 0, num2);
    }
}
