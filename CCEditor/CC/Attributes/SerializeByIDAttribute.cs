using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.CC.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SerializeByIDAttribute : Attribute
    {
        public class SerializedObjectFields
        {
            public MemberInfo memberInfo;

            public Type tp;

            public SerializeByIDAttribute serializer;

            public Func<object> GetValue;

            public Action<object> SetValue;
        }

        public class SerializedClassInfo
        {
            public MemberInfo memberInfo;

            public Type tp;

            public SerializeByIDAttribute serializer;

            public Func<object, object> GetValue;

            public Action<object, object> SetValue;
        }

        private static Dictionary<Type, Dictionary<int, SerializedClassInfo>> ClassLibrary = new Dictionary<Type, Dictionary<int, SerializedClassInfo>>();

        public readonly int id;

        public bool ignoreWrite;

        private const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        private static object threadingLock = new object();

        public SerializeByIDAttribute(int id, bool ignoreWrite = false)
        {
            this.id = id;
            this.ignoreWrite = ignoreWrite;
        }

        public static Dictionary<int, SerializedObjectFields> GetObjectIntDictionary(object obj)
        {
            return GetClassDictionary(obj.GetType()).ToDictionary((KeyValuePair<int, SerializedClassInfo> x) => x.Key, (KeyValuePair<int, SerializedClassInfo> x) => Class2ObjectInfo(x.Value, obj));
        }

        public static Dictionary<string, SerializedObjectFields> GetObjectStringDictionary(object obj)
        {
            return GetClassDictionary(obj.GetType()).ToDictionary((KeyValuePair<int, SerializedClassInfo> x) => x.Value.memberInfo.Name, (KeyValuePair<int, SerializedClassInfo> x) => Class2ObjectInfo(x.Value, obj));
        }

        public static void ApplyStringDictionary(object obj, Dictionary<string, string> values)
        {
            ApplyStringDictionary(obj, values, safely: false);
        }

        public static void TryApplyStringDictionary(object obj, Dictionary<string, string> values)
        {
            ApplyStringDictionary(obj, values, safely: true);
        }

        private static void ApplyStringDictionary(object obj, Dictionary<string, string> values, bool safely)
        {
            Dictionary<string, SerializedObjectFields> objectStringDictionary = GetObjectStringDictionary(obj);
            IEnumerable<string> enumerable;
            if (!safely)
            {
                IEnumerable<string> keys = values.Keys;
                enumerable = keys;
            }
            else
            {
                enumerable = values.Keys.Intersect(objectStringDictionary.Keys);
            }
            foreach (string item in enumerable)
            {
                SerializedObjectFields serializedObjectFields = objectStringDictionary[item];
                serializedObjectFields.SetValue(StringConverter.ParseObject(serializedObjectFields.tp, values[item]));
            }
        }

        public static Dictionary<int, SerializedClassInfo> GetClassDictionary(Type tp)
        {
            lock (threadingLock)
            {
                if (!ClassLibrary.ContainsKey(tp))
                {
                    ClassLibrary.Add(tp, GetClassAttributes(tp).ToDictionary((SerializedClassInfo x) => x.serializer.id, (SerializedClassInfo x) => x));
                }
            }
            return ClassLibrary[tp];
        }

        private static SerializedObjectFields Class2ObjectInfo(SerializedClassInfo scf, object obj)
        {
            return new SerializedObjectFields
            {
                memberInfo = scf.memberInfo,
                serializer = scf.serializer,
                tp = scf.tp,
                GetValue = () => scf.GetValue(obj),
                SetValue = delegate (object x)
                {
                    scf.SetValue(obj, x);
                }
            };
        }

        private static SerializedClassInfo[] GetClassAttributes(Type tp)
        {
            MemberInfo[] fields = tp.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            MemberInfo[] array = fields;
            MemberInfo[] first = array;
            fields = tp.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            array = fields;
            MemberInfo[] second = array;
            MemberInfo[] array2 = first.Concat(second).ToArray();
            SerializeByIDAttribute[] array3 = array2.Select(GetLinkedInfo).ToArray();
            List<SerializedClassInfo> list = new List<SerializedClassInfo>();
            for (int i = 0; i < array2.Length; i++)
            {
                if (array3[i] != null)
                {
                    list.Add(new SerializedClassInfo
                    {
                        memberInfo = array2[i],
                        serializer = array3[i],
                        tp = GetUnderlyingType(array2[i]),
                        GetValue = GetMemberGetter(array2[i]),
                        SetValue = GetMemberSetter(array2[i])
                    });
                }
            }
            return list.ToArray();
        }

        private static SerializeByIDAttribute GetLinkedInfo(MemberInfo fi)
        {
            return (SerializeByIDAttribute)Attribute.GetCustomAttribute(fi, typeof(SerializeByIDAttribute));
        }

        public static Action<object, object> GetMemberSetter(MemberInfo member)
        {
            if (member.MemberType == MemberTypes.Field)
            {
                return ((FieldInfo)member).SetValue;
            }
            if (member.MemberType == MemberTypes.Property)
            {
                return ((PropertyInfo)member).SetValue;
            }
            throw new Exception("Invalid Property");
        }

        public static Func<object, object> GetMemberGetter(MemberInfo member)
        {
            if (member.MemberType == MemberTypes.Field)
            {
                return ((FieldInfo)member).GetValue;
            }
            if (member.MemberType == MemberTypes.Property)
            {
                return ((PropertyInfo)member).GetValue;
            }
            throw new Exception("Invalid Property");
        }

        public static Type GetUnderlyingType(MemberInfo member)
        {
            MemberTypes memberType = member.MemberType;
            if (1 == 0)
            {
            }
            Type result;
            switch (memberType)
            {
                case MemberTypes.Field:
                    result = ((FieldInfo)member).FieldType;
                    break;
                case MemberTypes.Property:
                    result = ((PropertyInfo)member).PropertyType;
                    break;
                case MemberTypes.Method:
                    result = ((MethodInfo)member).ReturnType;
                    break;
                case MemberTypes.Event:
                    result = ((EventInfo)member).EventHandlerType;
                    break;
                default:
                    throw new ArgumentException();
            }
            if (1 == 0)
            {
            }
            return result;
        }
    }
}
