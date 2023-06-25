using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.CC
{
    public static class DataEncoder
    {
        private const int x0 = 0;

        private const int x1 = 1;

        private const int x4 = 4;

        private const float f8912 = 8192f;

        private const float f8912i = 0.00012207031f;

        private static readonly object lockObj = new object();

        public static byte[] FullyEncodeFloats(float[] whole)
        {
            return CompressToGZip(DeltaEncodeFloats(whole));
        }

        public static byte[] CompressToGZip(byte[] input)
        {
            using (MemoryStream memoryStream2 = new MemoryStream(input))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (GZipStream gZipStream = new GZipStream(memoryStream, System.IO.Compression.CompressionLevel.Optimal))
                    {
                        memoryStream2.CopyTo(gZipStream);
                        gZipStream.Close();
                        return memoryStream.ToArray();
                    }
                }
            }
        }

        public static byte[] DecompressGZip(byte[] input)
        {
            using (MemoryStream stream = new MemoryStream(input))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (GZipStream gZipStream = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        gZipStream.CopyTo(memoryStream);
                        gZipStream.Close();
                        return memoryStream.ToArray();
                    }
                }
            }
        }

        public static byte[] ReadCompressedBytes(Stream s, int count)
        {
            byte[] array = new byte[count];
            new GZipStream(s, CompressionMode.Decompress).Read(array, 0, count);
            return array;
        }

        public static byte[] DeltaEncodeFloats(float[] floats)
        {
            byte[] array = new byte[floats.Length * 4];
            Buffer.BlockCopy(floats, 0, array, 0, array.Length);
            using (MemoryStream input = new MemoryStream(array, writable: false))
            {
                using (BinaryReader binaryReader = new BinaryReader(input))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (BinaryWriter1 binaryWriter = new BinaryWriter1(memoryStream))
                        {
                            int num = array.Length;
                            int num2 = FloatStreamToScaledInt(binaryReader);
                            binaryWriter.Write7BitEncodedInt(ZigZagEncode(num2));
                            int num3 = FloatStreamToScaledInt(binaryReader);
                            binaryWriter.Write7BitEncodedInt(ZigZagEncode(num3 - num2));
                            long position = binaryReader.BaseStream.Position;
                            while (position != num)
                            {
                                int num4 = FloatStreamToScaledInt(binaryReader);
                                position = binaryReader.BaseStream.Position;
                                int i = ZigZagEncode(num3 + num3 - num2 - num4);
                                binaryWriter.Write7BitEncodedInt(i);
                                num2 = num3;
                                num3 = num4;
                            }
                            return memoryStream.ToArray();
                        }
                    }
                }
            }
        }

        private static int FloatStreamToScaledInt(BinaryReader reader)
        {
            return Mathf.RoundToInt(reader.ReadSingle() * 8192f);
        }

        private static void ScaledIntToFloatStream(BinaryWriter writer, int scaled)
        {
            writer.Write((float)scaled * 0.00012207031f);
        }

        public static int ZigZagEncode(int i)
        {
            return (i << 1) ^ (i >> 31);
        }

        public static int ZigZagDecode(int i)
        {
            return (i >> 1) ^ -(i & 1);
        }

        public static void DeltaDecodeToFloats(byte[] source, Stream target, int byteCount)
        {
            using (MemoryStream stream = new MemoryStream(source, writable: false))
            {
                using (BinaryReader1 binaryReader = new BinaryReader1(stream))
                {
                    BinaryWriter writer = new BinaryWriter(target);
                    int num = ZigZagDecode(binaryReader.Read7BitEncodedInt());
                    ScaledIntToFloatStream(writer, num);
                    int num2 = ZigZagDecode(binaryReader.Read7BitEncodedInt()) + num;
                    ScaledIntToFloatStream(writer, num2);
                    long position = binaryReader.BaseStream.Position;
                    while (position < byteCount)
                    {
                        int i = binaryReader.Read7BitEncodedInt();
                        position = binaryReader.BaseStream.Position;
                        int num3 = ZigZagDecode(i);
                        int num4 = num2 + num2 - num - num3;
                        ScaledIntToFloatStream(writer, num4);
                        num = num2;
                        num2 = num4;
                    }
                }
            }
        }

        public static T[][] SplitArray<T>(T[] flattened, int pieceCount)
        {
            int num = Mathf.Min(flattened.Length, pieceCount);
            T[][] array = new T[num][];
            int element = flattened.Length / num;
            int num2 = flattened.Length % num;
            int[] array2 = Enumerable.Repeat(element, num).ToArray();
            array2[num - 1] += num2;
            int num3 = Marshal.SizeOf<T>();
            int num4 = 0;
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new T[array2[i]];
                int num5 = array2[i] * num3;
                Buffer.BlockCopy(flattened, num4, array[i], 0, num5);
                num4 += num5;
            }
            return array;
        }
    }
}
