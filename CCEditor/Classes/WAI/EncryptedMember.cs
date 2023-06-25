using CCEditor.CC.Interfaces;
using CCEditor.CC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.Classes.WAI
{
    public class EncryptedMember : ISelfSerializable
    {
        private readonly object obj;

        private const string pword = "Anas&Fayez@WitchcraftAI!";

        public EncryptedMember()
        {
        }

        public EncryptedMember(object target)
        {
            obj = target;
        }

        public void SerializeSelf(ObjectWriter ow)
        {
            byte[] bytesToEncrypt = SerializeManager.SerializeObject(obj);
            EncryptToStream(ow, bytesToEncrypt, "Anas&Fayez@WitchcraftAI!");
        }

        public object DeserializeSelf(ObjectReader br)
        {
            return SerializeManager.DeserializeByteArray<object>(DecryptFromStream(br, "Anas&Fayez@WitchcraftAI!"));
        }

        public static void EncryptToStream(ObjectWriter ow, byte[] bytesToEncrypt, string password)
        {
            byte[] array = Guid.NewGuid().ToByteArray();
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, array);
            byte[] bytes = rfc2898DeriveBytes.GetBytes(16);
            byte[] bytes2 = rfc2898DeriveBytes.GetBytes(16);
            ow.Write(bytesToEncrypt.Length);
            ow.Write(array);
            _ = ow.BaseStream.Position;
            byte[] array2;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesCryptoServiceProvider.CreateEncryptor(bytes, bytes2), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytesToEncrypt, 0, bytesToEncrypt.Length);
                    }
                }
                array2 = memoryStream.ToArray();
            }
            ow.Write(array2.Length);
            ow.Write(array2);
        }

        public static byte[] DecryptFromStream(ObjectReader or, string password)
        {
            int num = or.ReadInt32();
            byte[] salt = or.ReadBytes(16);
            int num2 = or.ReadInt32();
            long position = or.BaseStream.Position + num2;
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt);
            byte[] bytes = rfc2898DeriveBytes.GetBytes(16);
            byte[] bytes2 = rfc2898DeriveBytes.GetBytes(16);
            byte[] array = new byte[num];
            AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider
            {
                Padding = PaddingMode.None
            };
            new CryptoStream(or.BaseStream, aesCryptoServiceProvider.CreateDecryptor(bytes, bytes2), CryptoStreamMode.Read).Read(array, 0, num);
            or.BaseStream.Position = position;
            return array;
        }

        public static byte[] Encrypt(byte[] bytesToEncrypt, string password)
        {
            byte[] array = Guid.NewGuid().ToByteArray();
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, array);
            byte[] bytes = rfc2898DeriveBytes.GetBytes(16);
            byte[] bytes2 = rfc2898DeriveBytes.GetBytes(16);
            byte[] array2;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesCryptoServiceProvider.CreateEncryptor(bytes, bytes2), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytesToEncrypt, 0, bytesToEncrypt.Length);
                    }
                }
                array2 = memoryStream.ToArray();
            }
            byte[] bytes3 = BitConverter.GetBytes(Convert.ToInt32(bytesToEncrypt.Length));
            byte[] array3 = new byte[bytes3.Length + array.Length + array2.Length];
            Buffer.BlockCopy(bytes3, 0, array3, 0, 4);
            Buffer.BlockCopy(array, 0, array3, 4, 16);
            Buffer.BlockCopy(array2, 0, array3, 20, array2.Length);
            return array3;
        }

        public static byte[] Decrypt(byte[] bytesToDecrypt, string password)
        {
            byte[] array = new byte[16];
            int count = BitConverter.ToInt32(bytesToDecrypt, 0);
            byte[] array2 = new byte[bytesToDecrypt.Length - 20];
            Buffer.BlockCopy(bytesToDecrypt, 4, array, 0, 16);
            Buffer.BlockCopy(bytesToDecrypt, 20, array2, 0, array2.Length);
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, array);
            byte[] bytes = rfc2898DeriveBytes.GetBytes(16);
            byte[] bytes2 = rfc2898DeriveBytes.GetBytes(16);
            using (MemoryStream memoryStream = new MemoryStream(array2))
            {
                using (AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider())
                {
                    aesCryptoServiceProvider.Padding = PaddingMode.None;
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesCryptoServiceProvider.CreateDecryptor(bytes, bytes2), CryptoStreamMode.Read))
                    {
                        cryptoStream.Read(array2, 0, count);
                    }
                }
                return memoryStream.ToArray().Take(count).ToArray();
            }
        }
    }
}
