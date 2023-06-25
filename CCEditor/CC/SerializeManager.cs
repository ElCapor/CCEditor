using CCEditor.Classes.WAI;
using CCEditor.ReflectionData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.CC
{
    public static class SerializeManager
    {
        public static string SerializeObjectToString(object obj)
        {
            return Convert.ToBase64String(SerializeObject(obj));
        }

        public static string CompressToString(object obj)
        {
            return Convert.ToBase64String(SerializeToCompressedObject(obj));
        }

        public static byte[] SerializeObject(object obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ObjectWriter objectWriter = new ObjectWriter(memoryStream))
                {
                    objectWriter.WriteIDAndObj(obj);
                    return memoryStream.ToArray();
                }
            }
        }

        public static byte[] SerializeToCompressedObject(object obj)
        {
            return SerializeObject(CompressedMember.IfEffective(obj));
        }

        public static object DeserializeByteArray(byte[] byteArray)
        {
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                using (ObjectReader objectReader = new ObjectReader(stream))
                {
                    return objectReader.ReadIDAndObject();
                }
            }
        }

        public static T DeserializeByteArray<T>(byte[] byteArray)
        {
            return (T)DeserializeByteArray(byteArray);
        }

        public static object LoadFile(string path)
        {
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (ObjectReader objectReader = new ObjectReader(stream))
                {
                    return objectReader.ReadIDAndObject();
                }
            }
        }

        public static T Copy<T>(T obj)
        {
            return DeserializeByteArray<T>(SerializeObject(obj));
        }

        public static T LoadFile<T>(string path)
        {
            return (T)LoadFile(path);
        }

        public static Type GetFullFileType(string path)
        {
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (ObjectReader objectReader = new ObjectReader(stream))
                {
                    return objectReader.ReadIDAndObject().GetType();
                }
            }
        }

        public static Type GetFileType(string path)
        {
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (ObjectReader objectReader = new ObjectReader(stream))
                {
                    return objectReader.ReadNestedType();
                }
            }
        }

        public static object DeserializeString(string base64)
        {
            return DeserializeByteArray(Convert.FromBase64String(base64));
        }

        public static T DeserializeString<T>(string base64)
        {
            return (T)DeserializeString(base64);
        }

        public static T DeserializeStream<T>(Stream s)
        {
            return (T)DeserializeStream(s);
        }

        public static object DeserializeStream(Stream s)
        {
            return new ObjectReader(s).ReadIDAndObject();
        }

        public static void SerializeToStream(Stream s, object obj)
        {
            ObjectWriter objectWriter = new ObjectWriter(s);
            objectWriter.WriteIDAndObj(obj);
            objectWriter.Flush();
        }

        public static T LoadFromFile<T>()
        {
            return (T)Load("");
        }

        public static T LoadFromFile<T>(string path)
        {
            object obj = Load(path);
            try
            {
                return (T)obj;
            }
            catch
            {
                throw new Exception(string.Concat("Couldn't cast ", obj.GetType(), " to ", typeof(T)));
            }
        }

        public static T LoadFromLazyFile<T>(string path)
        {
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            T result = DeserializeStream<LazySerializableSequence>(stream).ReadPiece<T>();
            stream.Close();
            return result;
        }

        public static T[] LoadLazyFile<T>(string path)
        {
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            object obj = DeserializeStream<object>(stream);
            T[] result = ((!(obj is LazySerializableSequence<T> lazySerializableSequence)) ? ((LazySerializableSequence)obj).Values.OfType<T>().ToArray() : lazySerializableSequence.Values.ToArray());
            stream.Close();
            return result;
        }

        public static string LoadLazyFileSummary(string path)
        {
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            string result = DeserializeStream<object>(stream).ToString();
            stream.Close();
            return result;
        }

        public static LazySerializableSequence<T> LoadRawLazyFile<T>(byte[] b)
        {
            LazySerializableSequence<T> lazySerializableSequence = DeserializeStream<LazySerializableSequence<T>>(new MemoryStream(b, writable: false));
            lazySerializableSequence.FillFlattened();
            return lazySerializableSequence;
        }

        public static LazySerializableSequence<T> LoadRawLazyFile<T>(string path)
        {
            LazySerializableSequence<T> lazySerializableSequence = DeserializeStream<LazySerializableSequence<T>>(new FileStream(path, FileMode.Open, FileAccess.Read));
            lazySerializableSequence.FillFlattened();
            return lazySerializableSequence;
        }

        public static TElement LoadFromLazyFile<TContainer, TElement>(string path) where TElement : TContainer
        {
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            object obj = DeserializeStream<object>(stream);
            TElement result = ((!(obj is LazySerializableSequence<TContainer> lazySerializableSequence)) ? ((LazySerializableSequence)obj).ReadPiece<TElement>() : lazySerializableSequence.ReadPiece<TElement>());
            stream.Close();
            return result;
        }

        

        public static object Load(string path)
        {
            using (Stream s = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return DeserializeStream(s);
            }
        }

        public static string GetSavingPath(string defaultPath = null)
        {
            
            return "C:\\AIServer\\test.mtb";
        }

        public static void CompressToDirectory(object obj, string defaultPath = null)
        {
            string savingPath = GetSavingPath(defaultPath);
            if (string.IsNullOrEmpty(savingPath))
            {
                Console.WriteLine("Cancelled");
            }
            else
            {
                SaveC(savingPath, obj);
            }
        }

        public static void SaveC(string path, object obj)
        {
            path = AddMTBIfNeeded(path);
            byte[] bytes = SerializeToCompressedObject(obj);
            File.WriteAllBytes(path, bytes);
        }

        public static void SerializeToDirectory(object obj, string defaultPath = null)
        {
            string savingPath = GetSavingPath(defaultPath);
            if (string.IsNullOrEmpty(savingPath))
            {
                Console.WriteLine("Cancelled");
            }
            else
            {
                SaveC(savingPath, obj);
            }
        }

        public static void Save(string path, object obj)
        {
            path = AddMTBIfNeeded(path);
            byte[] bytes = SerializeObject(obj);
            File.WriteAllBytes(path, bytes);
        }

        private static string AddMTBIfNeeded(string s)
        {
            if (!s.EndsWith(".mtb"))
            {
                return s + ".mtb";
            }
            return s;
        }
    }
}
