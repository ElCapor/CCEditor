using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.CC
{
    public static class StringConverter
    {
        private static Dictionary<Type, Func<object, string>> toStringDict;

        private static Dictionary<Type, Func<string, object>> fromStringDict;

        static StringConverter()
        {
            fromStringDict = new Dictionary<Type, Func<string, object>>
            {
                {
                    typeof(bool),
                    ParseBool
                },
                {
                    typeof(byte),
                    ParseByte
                },
                {
                    typeof(char),
                    ParseChar
                },
                {
                    typeof(decimal),
                    ParseDecimal
                },
                {
                    typeof(double),
                    ParseDouble
                },
                {
                    typeof(float),
                    ParseFloat
                },
                {
                    typeof(int),
                    ParseInt
                },
                {
                    typeof(long),
                    ParseLong
                },
                {
                    typeof(sbyte),
                    ParseSByte
                },
                {
                    typeof(short),
                    ParseShort
                },
                {
                    typeof(string),
                    ParseString
                },
                {
                    typeof(uint),
                    ParseUInt
                },
                {
                    typeof(ulong),
                    ParseULong
                },
                {
                    typeof(ushort),
                    ParseUShort
                },
                {
                    typeof(DateTime),
                    ParseDateTime
                }
            };
            toStringDict = new Dictionary<Type, Func<object, string>>
            {
                {
                    typeof(bool),
                    WriteBool
                },
                {
                    typeof(byte),
                    WriteByte
                },
                {
                    typeof(char),
                    WriteChar
                },
                {
                    typeof(decimal),
                    WriteDecimal
                },
                {
                    typeof(double),
                    WriteDouble
                },
                {
                    typeof(float),
                    WriteFloat
                },
                {
                    typeof(int),
                    WriteInt
                },
                {
                    typeof(long),
                    WriteLong
                },
                {
                    typeof(sbyte),
                    WriteSByte
                },
                {
                    typeof(short),
                    WriteShort
                },
                {
                    typeof(string),
                    WriteString
                },
                {
                    typeof(uint),
                    WriteUInt
                },
                {
                    typeof(ulong),
                    WriteULong
                },
                {
                    typeof(ushort),
                    WriteUShort
                },
                {
                    typeof(DateTime),
                    WriteDateTime
                }
            };
        }

        public static object ParseObject(Type tp, string str)
        {
            return fromStringDict[tp](str);
        }

        public static string WriteObject(object o)
        {
            if (o == null)
            {
                return "NULL";
            }
            return toStringDict[o.GetType()](o);
        }

        public static object ParseBool(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return bool.Parse(str);
        }

        public static object ParseByte(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return (byte)0;
            }
            return byte.Parse(str);
        }

        public static object ParseChar(string str)
        {
            return char.Parse(str);
        }

        public static object ParseDecimal(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0m;
            }
            return decimal.Parse(str);
        }

        public static object ParseDouble(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0.0;
            }
            return double.Parse(str);
        }

        public static object ParseFloat(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0f;
            }
            return float.Parse(str);
        }

        public static object ParseInt(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            return int.Parse(str.Replace(",", ""));
        }

        public static object ParseLong(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0L;
            }
            return int.Parse(str.Replace(",", ""));
        }

        public static object ParseSByte(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return (sbyte)0;
            }
            return sbyte.Parse(str);
        }

        public static object ParseShort(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return (short)0;
            }
            return short.Parse(str.Replace(",", ""));
        }

        public static object ParseString(string str)
        {
            if (!str.ToLower().Equals("null"))
            {
                return str;
            }
            return null;
        }

        public static object ParseUInt(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0u;
            }
            return uint.Parse(str.Replace(",", ""));
        }

        public static object ParseULong(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0uL;
            }
            return ulong.Parse(str.Replace(",", ""));
        }

        public static object ParseUShort(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return (ushort)0;
            }
            return ushort.Parse(str.Replace(",", ""));
        }

        public static object ParseDateTime(string str)
        {
            int[] array = str.Split('-').Select(int.Parse).ToArray();
            return new DateTime(array[0], array[1], array[2]);
        }

        public static string WriteBool(object o)
        {
            return o.ToString();
        }

        public static string WriteByte(object o)
        {
            return o.ToString();
        }

        public static string WriteChar(object o)
        {
            return o.ToString();
        }

        public static string WriteDecimal(object o)
        {
            return o.ToString();
        }

        public static string WriteDouble(object o)
        {
            return o.ToString();
        }

        public static string WriteFloat(object o)
        {
            return o.ToString();
        }

        public static string WriteInt(object o)
        {
            return o.ToString();
        }

        public static string WriteLong(object o)
        {
            return o.ToString();
        }

        public static string WriteSByte(object o)
        {
            return o.ToString();
        }

        public static string WriteShort(object o)
        {
            return o.ToString();
        }

        public static string WriteString(object o)
        {
            return o.ToString();
        }

        public static string WriteUInt(object o)
        {
            return o.ToString();
        }

        public static string WriteULong(object o)
        {
            return o.ToString();
        }

        public static string WriteUShort(object o)
        {
            return o.ToString();
        }

        public static string WriteDateTime(object o)
        {
            return ((DateTime)o).ToString("yyyy-MM-dd");
        }
    }
}
