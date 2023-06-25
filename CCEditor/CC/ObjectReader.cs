using CCEditor.Classes.WAI;
using CCEditor.Classes;
using CCEditor.ReflectionData;
using CCEditor.CC.Interfaces;
using CCEditor.CC.Attributes;
using CCEditor.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace CCEditor.CC
{
    public class ObjectReader : BinaryReader2
    {
        private readonly Dictionary<Type, Func<object>> readers;

        private readonly Dictionary<Type, Func<Type, object>> collectionReaders;

        private static readonly Dictionary<Type, MethodInfo> readersInfo;

        private static readonly object[] emptyObjectArray;

        private static readonly Type[] tuples;

        static ObjectReader()
        {
            emptyObjectArray = new object[0];
            tuples = new Type[9]
            {
                null,
                typeof(Tuple<>),
                typeof(Tuple<, >),
                typeof(Tuple<, , >),
                typeof(Tuple<, , , >),
                typeof(Tuple<, , , , >),
                typeof(Tuple<, , , , , >),
                typeof(Tuple<, , , , , , >),
                typeof(Tuple<, , , , , , , >)
            };
            IEnumerable<MethodInfo> source = from x in typeof(ObjectReader).BaseType.GetMethods()
                                             where x.Name.StartsWith("Read", StringComparison.CurrentCulture)
                                             select x;
            MethodInfo value = typeof(ObjectReader).GetMethods().First((MethodInfo x) => x.Name.Equals("ReadPackedArray"));
            source = source.Where((MethodInfo x) => x.ReturnType != typeof(int) || x.Name.Equals("ReadInt32"));
            Dictionary<Type, MethodInfo> readersByOutput = source.ToDictionary((MethodInfo x) => x.ReturnType, (MethodInfo x) => x);
            readersInfo = ReflectionHelpers.typesWithSpecificWriters.ToDictionary((Type x) => x, (Type x) => readersByOutput[x]);
            readersInfo.Add(typeof(PackedArray), value);
            ReflectionHelpers.StripingStitch();
        }

        public ObjectReader(Stream stream)
            : base(stream)
        {
            readers = readersInfo.ToDictionary((KeyValuePair<Type, MethodInfo> x) => x.Key, (KeyValuePair<Type, MethodInfo> x) => GetInstanceAction(x.Value));
            readers.Add(typeof(object), ReadIDAndObject);
            readers.Add(ReflectionHelpers.runtimeType, ReadNestedType);
            collectionReaders = new Dictionary<Type, Func<Type, object>>
            {
                {
                    typeof(Array),
                    ReadArrayOfType
                },
                {
                    typeof(List<>),
                    ReadList
                },
                {
                    typeof(Dictionary<, >),
                    ReadDictionary
                },
                {
                    typeof(Tuple),
                    ReadTuple
                },
                {
                    typeof(Enum),
                    ReadEnum
                }
            };
        }

        private Func<object> GetInstanceAction(MethodInfo mi)
        {
            return () => mi.Invoke(this, emptyObjectArray);
        }

        private Func<object, object> GetInstanceActionArray(MethodInfo mi)
        {
            return (object x) => mi.Invoke(this, new object[1] { x });
        }

        public object ReadIDAndObject()
        {
            Type type = ReadNestedType();
            Type type2 = ReflectionHelpers.NaturalizeType(type, naturalizeEnum: true);
            Func<object> value;
            if (type2 != type && collectionReaders.TryGetValue(type2, out var fu))
            {
                value = () => fu(type);
            }
            else if (!readers.TryGetValue(type2, out value))
            {
                value = () => ReadCompoundObject(type);
            }
            return value();
        }

        public object ReadTuple(Type type)
        {
            Type[] types = type.GetGenericArguments();
            object[] args = types.Select((Type x, int y) => GetTypeReader(types[y], isSubclassed: false)()).ToArray();
            return Activator.CreateInstance(type, args);
        }

        public object ReadEnum(Type tp)
        {
            Type underlyingType = Enum.GetUnderlyingType(tp);
            object value = readers[underlyingType]();
            return Enum.ToObject(tp, value);
        }

        public object ReadDictionary(Type type)
        {
            int num = Read7BitEncodedInt();
            IDictionary dictionary = (IDictionary)Activator.CreateInstance(type, num);
            Type[] genericArguments = type.GetGenericArguments();
            bool isSubclassed = !genericArguments[0].IsSealed && (genericArguments[0] == typeof(object) || ReadBoolean());
            bool isSubclassed2 = !genericArguments[1].IsSealed && (genericArguments[1] == typeof(object) || ReadBoolean());
            Func<object> typeReader = GetTypeReader(genericArguments[0], isSubclassed);
            Func<object> typeReader2 = GetTypeReader(genericArguments[1], isSubclassed2);
            for (int i = 0; i < num; i++)
            {
                dictionary.Add(typeReader(), typeReader2());
            }
            return dictionary;
        }

        public object ReadList(Type type)
        {
            object obj = ReadArray(type.GetGenericArguments()[0], 1);
            return Activator.CreateInstance(type, obj);
        }

        public object ReadArrayOfType(Type arrayType)
        {
            return ReadArray(arrayType.GetElementType(), arrayType.GetArrayRank());
        }

        public object ReadArray(Type elementType, int rank)
        {
            int[] array = new int[rank];
            for (int i = 0; i < rank; i++)
            {
                int num = Read7BitEncodedInt();
                array[i] = num;
            }
            Array array2 = Array.CreateInstance(elementType, array);
            if (elementType.IsPrimitive)
            {
                ReadIntoPrimitiveArray(array2);
                return array2;
            }
            Func<object> value = null;
            bool flag = !elementType.IsSealed && (elementType == typeof(object) || ReadBoolean());
            bool flag2 = ReflectionHelpers.IsArray(elementType);
            if (flag)
            {
                value = ReadIDAndObject;
            }
            else if (flag2)
            {
                Type grandChildType = elementType.GetElementType();
                byte childRank = (byte)elementType.GetArrayRank();
                value = () => ReadArray(grandChildType, childRank);
            }
            else if (elementType.IsGenericType)
            {
                value = (typeof(IList).IsAssignableFrom(elementType) ? ((Func<object>)(() => ReadList(elementType))) : ((!typeof(IDictionary).IsAssignableFrom(elementType)) ? ((Func<object>)(() => ReadCompoundObject(elementType))) : ((Func<object>)(() => ReadDictionary(elementType)))));
            }
            else
            {
                ReflectionHelpers.NaturalizeType(elementType, naturalizeEnum: true);
                if (!readers.TryGetValue(elementType, out value))
                {
                    value = () => ReadCompoundObject(elementType);
                }
            }
            if (rank == 1)
            {
                IList list = array2;
                for (int j = 0; j < array2.Length; j++)
                {
                    list[j] = value();
                }
            }
            else
            {
                Func<int, int[]> flattener = array2.GetFlattener();
                for (int k = 0; k < array2.Length; k++)
                {
                    array2.SetValue(value(), flattener(k));
                }
            }
            return array2;
        }

        private Func<object> GetTypeReader(Type type, bool isSubclassed)
        {
            Type key = ReflectionHelpers.NaturalizeType(type, naturalizeEnum: true);
            if (isSubclassed)
            {
                return ReadIDAndObject;
            }
            if (collectionReaders.TryGetValue(key, out var collReader))
            {
                return () => collReader(type);
            }
            if (readers.TryGetValue(key, out var value))
            {
                return value;
            }
            return () => ReadCompoundObject(type);
        }

        private object ReadCompoundObject(Type type)
        {
            object obj = Activator.CreateInstance(type);
            if (typeof(ISelfSerializable).IsAssignableFrom(type))
            {
                return ((ISelfSerializable)obj).DeserializeSelf(this);
            }
            if (typeof(IPreDeserializeByID).IsAssignableFrom(type))
            {
                ((IPreDeserializeByID)obj).OnPreDeserialize();
            }
            int num = Read7BitEncodedInt();
            long num2 = BaseStream.Position + num;
            Dictionary<int, SerializeByIDAttribute.SerializedObjectFields> objectIntDictionary = SerializeByIDAttribute.GetObjectIntDictionary(obj);
            while (BaseStream.Position < num2)
            {
                int key = Read7BitEncodedInt();
                object obj2 = ReadIDAndObject();
                if (objectIntDictionary.TryGetValue(key, out var value) && value.tp.IsAssignableFrom(obj2.GetType()))
                {
                    value.SetValue(obj2);
                }
            }
            if (typeof(IPostDeserializeByID).IsAssignableFrom(type))
            {
                ((IPostDeserializeByID)obj).OnDeserialize();
            }
            return obj;
        }

        public Array ReadPackedArray()
        {
            Type type = ReadNestedType();
            Shape shape = ReadShape();
            Array array = Array.CreateInstance(type, shape.Dimensions);
            int num = Read7BitEncodedInt();
            long num2 = BaseStream.Position + num;
            foreach (int[] item in array.GetNDIterator())
            {
                array.SetValue(Activator.CreateInstance(type), item);
            }
            Dictionary<int, SerializeByIDAttribute.SerializedClassInfo> classDictionary = SerializeByIDAttribute.GetClassDictionary(type);
            while (BaseStream.Position < num2)
            {
                int key = Read7BitEncodedInt();
                Type type2 = ReadType();
                Func<object> typeReader = GetTypeReader(type2, isSubclassed: false);
                if (classDictionary.TryGetValue(key, out var value))
                {
                    foreach (int[] item2 in array.GetNDIterator())
                    {
                        value.SetValue(array.GetValue(item2), typeReader());
                    }
                    continue;
                }
                foreach (int[] item3 in array.GetNDIterator())
                {
                    typeReader();
                }
            }
            if (typeof(IPostDeserializeByID).IsAssignableFrom(type))
            {
                foreach (int[] item4 in array.GetNDIterator())
                {
                    ((IPostDeserializeByID)array.GetValue(item4)).OnDeserialize();
                }
            }
            return array;
        }

        public Type ReadType()
        {
            int key = Read7BitEncodedInt();
            int key2 = Read7BitEncodedInt();
            return TypeIDs.groups[key].classes[key2];
        }

        public Type ReadNestedType()
        {
            Type type = ReadType();
            bool flag = type == typeof(Tuple);
            if (ReflectionHelpers.IsArray(type))
            {
                Type type2 = ReadNestedType();
                byte b = ReadByte();
                if (b != 1)
                {
                    return type2.MakeArrayType(b);
                }
                return type2.MakeArrayType();
            }
            if (type.IsGenericType || flag)
            {
                Type[] array = new Type[flag ? ReadByte() : type.GetGenericArguments().Length];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = ReadNestedType();
                }
                return (flag ? tuples[array.Length] : type).MakeGenericType(array);
            }
            return type;
        }
    }
}
