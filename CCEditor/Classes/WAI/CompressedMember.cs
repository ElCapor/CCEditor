using CCEditor.CC.Interfaces;
using CCEditor.CC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.Classes.WAI
{
    public class CompressedMember : ISelfSerializable
    {
        private readonly byte[] comp;

        public CompressedMember()
        {
        }

        private CompressedMember(byte[] compressed)
        {
            comp = compressed;
        }

        public static object IfEffective(object target)
        {
            TryIfEffective(target, out var potentiallcompressed);
            return potentiallcompressed;
        }

        public static CompressedMember ForceCompress(object target)
        {
            byte[] array = SerializeManager.SerializeObject(target);
            byte[] array2 = DataEncoder.CompressToGZip(array);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ObjectWriter objectWriter = new ObjectWriter(memoryStream))
                {
                    objectWriter.Write7BitEncodedInt(array.Length);
                    objectWriter.Write7BitEncodedInt(array2.Length);
                    objectWriter.Write(array2);
                    return new CompressedMember(memoryStream.ToArray());
                }
            }
        }

        public static bool TryIfEffective(object target, out object potentiallcompressed)
        {
            byte[] array = SerializeManager.SerializeObject(target);
            byte[] array2 = DataEncoder.CompressToGZip(array);
            byte[] array3;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ObjectWriter objectWriter = new ObjectWriter(memoryStream))
                {
                    objectWriter.Write7BitEncodedInt(array.Length);
                    objectWriter.Write7BitEncodedInt(array2.Length);
                    objectWriter.Write(array2);
                    array3 = memoryStream.ToArray();
                }
            }
            if (array3.Length + 3 < array.Length)
            {
                potentiallcompressed = new CompressedMember(array3);
                return true;
            }
            potentiallcompressed = target;
            return false;
        }

        public void SerializeSelf(ObjectWriter ow)
        {
            ow.Write(comp);
        }

        public object DeserializeSelf(ObjectReader br)
        {
            int count = br.Read7BitEncodedInt();
            int num = br.Read7BitEncodedInt();
            long position = br.BaseStream.Position + num;
            byte[] byteArray = DataEncoder.ReadCompressedBytes(br.BaseStream, count);
            br.BaseStream.Position = position;
            return SerializeManager.DeserializeByteArray<object>(byteArray);
        }
    }
}
