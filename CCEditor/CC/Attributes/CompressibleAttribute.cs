using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CCEditor.CC.Extensions;
namespace CCEditor.CC.Attributes
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CompressibleAttribute : Attribute
    {
        private const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        private static Dictionary<Type, string[]> dict = new Dictionary<Type, string[]>();

        private static object threadingLock = new object();

        public static bool IsMemberCompressible(MemberInfo mi)
        {
            return GetCompressedMembers(mi.DeclaringType).Contains(mi.Name);
        }

        private static string[] GetCompressedMembers(Type tp)
        {
            string[] value;
            lock (threadingLock)
            {
                dict.TryGetValue(tp, out value);
                if (value == null)
                {
                    MemberInfo[] fields = tp.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    MemberInfo[] array = fields;
                    MemberInfo[] first = array;
                    fields = tp.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    array = fields;
                    MemberInfo[] second = array;
                    MemberInfo[] array2 = first.Concat(second).ToArray();
                    CompressibleAttribute[] atttributes = array2.Select(GetLinkedInfo).ToArray();
                    value = array2.ToIndexedDictionary().Where(delegate (KeyValuePair<int, MemberInfo> x)
                    {
                        CompressibleAttribute[] array3 = atttributes;
                        KeyValuePair<int, MemberInfo> keyValuePair2 = x;
                        return array3[keyValuePair2.Key] != null;
                    }).Select(delegate (KeyValuePair<int, MemberInfo> x)
                    {
                        KeyValuePair<int, MemberInfo> keyValuePair = x;
                        return keyValuePair.Value.Name;
                    })
                        .ToArray();
                    dict.Add(tp, value);
                }
            }
            return value;
        }

        private static CompressibleAttribute GetLinkedInfo(MemberInfo fi)
        {
            return (CompressibleAttribute)Attribute.GetCustomAttribute(fi, typeof(CompressibleAttribute));
        }
    }

}
