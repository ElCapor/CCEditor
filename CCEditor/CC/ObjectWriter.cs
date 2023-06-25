using CCEditor.CC.Attributes;
using CCEditor.CC.Interfaces;
using CCEditor.Classes.WAI;
using CCEditor.Helpers;
using CCEditor.ReflectionData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.CC
{
    public class ObjectWriter : BinaryWriter2
    {
        private readonly Dictionary<Type, Action<object>> writers;

        private readonly Dictionary<Type, Action<object>> primitiveArrayWriters;

        private static readonly Dictionary<Type, MethodInfo> writersInfo;

        static ObjectWriter()
        {
            IEnumerable<MethodInfo> source = from x in typeof(ObjectWriter).GetMethods()
                                             where x.Name.Equals("Write") && x.GetParameters().Length == 1
                                             select x;
            Dictionary<Type, MethodInfo> writersByInput = source.ToDictionary((MethodInfo x) => x.GetParameters()[0].ParameterType, (MethodInfo x) => x);
            writersInfo = ReflectionHelpers.typesWithSpecificWriters.ToDictionary((Type x) => x, (Type x) => writersByInput[x]);
            writersInfo.Add(typeof(PackedArray), writersByInput[typeof(PackedArray)]);
        }

        public ObjectWriter(Stream stream)
            : base(stream)
        {
            writers = writersInfo.ToDictionary((KeyValuePair<Type, MethodInfo> x) => x.Key, (KeyValuePair<Type, MethodInfo> x) => GetInstanceAction(x.Value));
            writers.Add(typeof(object), WriteIDAndObj);
            writers.Add(ReflectionHelpers.runtimeType, WriteNestedType);
            writers.Add(typeof(Array), WriteArray);
            writers.Add(typeof(List<>), WriteList);
            writers.Add(typeof(Dictionary<,>), WriteDictionary);
            writers.Add(typeof(Tuple), WriteTuple);
            writers.Add(typeof(Enum), WriteEnum);
        }

        private Action<object> GetInstanceAction(MethodInfo mi)
        {
            return delegate (object x)
            {
                mi.Invoke(this, new object[1] { x });
            };
        }

        public void WriteIDAndObj(object obj)
        {
            Type type = obj.GetType();
            TypeIDs.GetTypeID(type);
            WriteNestedType(type);
            Type key = ReflectionHelpers.NaturalizeType(type, naturalizeEnum: true);
            if (!writers.TryGetValue(key, out var value))
            {
                value = WriteCompoundObject;
            }
            value(obj);
        }

        public void WriteTuple(object tpl)
        {
            Type[] genericArguments = tpl.GetType().GetGenericArguments();
            _ = new object[genericArguments.Length];
            object[] array = (from property in tpl.GetType().GetProperties()
                              select property.GetValue(tpl)).ToArray();
            for (int i = 0; i < genericArguments.Length; i++)
            {
                GetTypeWriter(genericArguments[i], isSubclassed: false)(array[i]);
            }
        }

        public void WriteEnum(object enm)
        {
            Type underlyingType = Enum.GetUnderlyingType(enm.GetType());
            writers[underlyingType](enm);
        }

        public void WriteDictionary(object dictionary)
        {
            IDictionary dictionary2 = (IDictionary)dictionary;
            Write7BitEncodedInt(dictionary2.Count);
            Type[] genericArguments = dictionary.GetType().GetGenericArguments();
            ReflectionHelpers.IsDictionarySubclassed(dictionary2, out var keysSubclassed, out var valuesSubclassed);
            if (!genericArguments[0].IsSealed && genericArguments[0] != typeof(object))
            {
                Write(keysSubclassed);
            }
            if (!genericArguments[1].IsSealed && genericArguments[1] != typeof(object))
            {
                Write(valuesSubclassed);
            }
            Action<object> typeWriter = GetTypeWriter(genericArguments[0], keysSubclassed);
            Action<object> typeWriter2 = GetTypeWriter(genericArguments[1], valuesSubclassed);
            foreach (DictionaryEntry item in dictionary2)
            {
                typeWriter(item.Key);
                typeWriter2(item.Value);
            }
        }

        public void WriteList(object list)
        {
            Array array = ReflectionHelpers.List2Array(list);
            WriteArray(array);
        }

        public void WriteArray(object array)
        {
            Type elementType = array.GetType().GetElementType();
            Array array2 = (Array)array;
            int[] dimensions = array2.GetShape().Dimensions;
            int[] array3 = dimensions;
            foreach (int i2 in array3)
            {
                Write7BitEncodedInt(i2);
            }
            if (elementType.IsPrimitive)
            {
                WriteFromPrimitiveArray(array2);
                return;
            }
            bool flag = false;
            if (!elementType.IsSealed && elementType != typeof(object))
            {
                flag = ReflectionHelpers.IsAnyArrayItemSubclassed(array2);
                Write(flag);
            }
            Action<object> typeWriter = GetTypeWriter(elementType, flag);
            foreach (object item in array2)
            {
                typeWriter(item);
            }
        }

        private Action<object> GetTypeWriter(Type type, bool isSubclassed)
        {
            Type key = ReflectionHelpers.NaturalizeType(type, naturalizeEnum: true);
            if (isSubclassed)
            {
                return WriteIDAndObj;
            }
            if (writers.TryGetValue(key, out var value))
            {
                return value;
            }
            return WriteCompoundObject;
        }

        private void WriteCompoundObject(object obj)
        {
            Type type = obj.GetType();
            if (typeof(IPreSerializeByID).IsAssignableFrom(type))
            {
                ((IPreSerializeByID)obj).OnPreSerialize();
            }
            if (typeof(ISelfSerializable).IsAssignableFrom(type))
            {
                ((ISelfSerializable)obj).SerializeSelf(this);
                return;
            }
            byte[] array;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ObjectWriter objectWriter = new ObjectWriter(memoryStream))
                {
                    foreach (KeyValuePair<int, SerializeByIDAttribute.SerializedObjectFields> item in SerializeByIDAttribute.GetObjectIntDictionary(obj).OrderBy(delegate (KeyValuePair<int, SerializeByIDAttribute.SerializedObjectFields> x)
                    {
                        KeyValuePair<int, SerializeByIDAttribute.SerializedObjectFields> keyValuePair = x;
                        return keyValuePair.Value.serializer.id;
                    }))
                    {
                        if (item.Value.serializer.ignoreWrite)
                        {
                            continue;
                        }
                        object obj2 = item.Value.GetValue();
                        if (obj2 != null)
                        {
                            objectWriter.Write7BitEncodedInt(item.Key);
                            if (CompressibleAttribute.IsMemberCompressible(item.Value.memberInfo))
                            {
                                obj2 = CompressedMember.IfEffective(obj2);
                            }
                            if (EncryptableAttribute.IsMemberEncryptable(item.Value.memberInfo))
                            {
                                obj2 = new EncryptedMember(obj2);
                            }
                            objectWriter.WriteIDAndObj(obj2);
                        }
                    }
                    array = memoryStream.ToArray();
                }
            }
            Write7BitEncodedInt(array.Length);
            Write(array);
        }

        public void Write(PackedArray packed)
        {
            Array array = packed.array;
            Type elementType = array.GetType().GetElementType();
            WriteNestedType(elementType);
            Write(array.GetShape());
            byte[] array2;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ObjectWriter objectWriter = new ObjectWriter(memoryStream))
                {
                    foreach (KeyValuePair<int, SerializeByIDAttribute.SerializedClassInfo> item in SerializeByIDAttribute.GetClassDictionary(elementType).OrderBy(delegate (KeyValuePair<int, SerializeByIDAttribute.SerializedClassInfo> x)
                    {
                        KeyValuePair<int, SerializeByIDAttribute.SerializedClassInfo> keyValuePair = x;
                        return keyValuePair.Value.serializer.id;
                    }))
                    {
                        if (item.Value.serializer.ignoreWrite)
                        {
                            continue;
                        }
                        objectWriter.Write7BitEncodedInt(item.Key);
                        objectWriter.WriteNestedType(item.Value.tp);
                        Action<object> typeWriter = objectWriter.GetTypeWriter(item.Value.tp, isSubclassed: false);
                        SerializeByIDAttribute.SerializedClassInfo value = item.Value;
                        foreach (object item2 in array)
                        {
                            typeWriter(value.GetValue(item2));
                        }
                    }
                    array2 = memoryStream.ToArray();
                }
            }
            Write7BitEncodedInt(array2.Length);
            Write(array2);
        }

        public void WriteType(Type tp)
        {
            TypeIDs.TypeID typeID = TypeIDs.GetTypeID(tp);
            Write7BitEncodedInt(typeID.groupID);
            Write7BitEncodedInt(typeID.subGroupID);
        }

        public void WriteNestedType(object obj)
        {
            Type type = (Type)obj;
            WriteType(type);
            if (ReflectionHelpers.IsArray(type))
            {
                WriteNestedType(type.GetElementType());
                Write((byte)type.GetArrayRank());
            }
            else if (type.IsGenericType)
            {
                Type[] genericArguments = type.GetGenericArguments();
                if (typeof(ITuple).IsAssignableFrom(type))
                {
                    Write((byte)genericArguments.Length);
                }
                Type[] array = genericArguments;
                Type[] array2 = array;
                foreach (Type obj2 in array2)
                {
                    WriteNestedType(obj2);
                }
            }
        }
    }
}
