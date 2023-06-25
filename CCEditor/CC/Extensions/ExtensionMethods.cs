using CCEditor.Classes.UnityEngine;
using CCEditor.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static MaterialSkin.Controls.MaterialForm;
using static System.Net.Mime.MediaTypeNames;

namespace CCEditor.CC.Extensions
{
    public static class ExtensionMethods
    {
        public static int[] Search(this string s, string substring)
        {
            List<int> list = new List<int>();
            string text = s;
            while (true)
            {
                int num = text.IndexOf(substring);
                if (num == -1)
                {
                    break;
                }
                list.Add(num);
                text = text.Substring(num + 1);
            }
            return list.ToArray();
        }

        public static string[] SplitByString(this string s, string separator)
        {
            return s.Split(new string[1] { separator }, StringSplitOptions.None);
        }

        public static Quaternion ReflectYZ(this Quaternion f)
        {
            return new Quaternion(f.x, 0f - f.y, 0f - f.z, f.w);
        }

        

       

        
        public static Dictionary<int, T> ToIndexedDictionary<T>(this List<T> list)
        {
            return new Range(list.Count).ToDictionary((int x) => x, (int x) => list[x]);
        }

        public static Dictionary<T, int> ValueToIndexDitionary<T>(this IEnumerable<T> enumberable)
        {
            return enumberable.ToArray().ValueToIndexDictionary();
        }

        public static Dictionary<T, int> ValueToIndexDictionary<T>(this T[] originalArray)
        {
            return new Range(originalArray.Length).ToDictionary((int x) => originalArray[x], (int x) => x);
        }

        public static Dictionary<T, int> ValueToIndexDictionary<T>(this List<T> originalArray)
        {
            return new Range(originalArray.Count).ToDictionary((int x) => originalArray[x], (int x) => x);
        }

        public static Dictionary<int, T> ToIndexedDictionary<T>(this T[] originalArray)
        {
            return new Range(originalArray.Length).ToDictionary((int x) => x, (int x) => originalArray[x]);
        }

        public static Dictionary<int, T> ToIndexedDictionary<T>(this IEnumerable<T> originalArray)
        {
            return originalArray.ToArray().ToIndexedDictionary();
        }

        public static IEnumerable<TResult> SelectNonNull<T, TResult>(this IEnumerable<T> sequence, Func<T, TResult> projection)
        {
            return from x in sequence
                   select projection(x) into e
                   where e != null
                   select e;
        }

        public static IEnumerable<T> WhereNonNull<T>(this IEnumerable<T> sequence)
        {
            return sequence.Where((T e) => e != null);
        }

        public static Vector3 Sum(this IEnumerable<Vector3> lst)
        {
            Vector3 zero = Vector3.zero;
            foreach (Vector3 item in lst)
            {
                zero += item;
            }
            return zero;
        }

        public static Vector3 Average(this IEnumerable<Vector3> lst)
        {
            return lst.Sum() / lst.Count();
        }

        public static Vector3 WeightedAverage(this IEnumerable<Vector3> lst, float[] weights)
        {
            Vector3 zero = Vector3.zero;
            Vector3[] array = lst.ToArray();
            for (int i = 0; i < weights.Length; i++)
            {
                zero += array[i] * weights[i];
            }
            return zero;
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }
        }

        public static string Formatted(this DateTime dt)
        {
            return dt.ToString("yy-MM-dd HH:mm:ss");
        }

        public static string AsString(this Matrix4x4 m)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    stringBuilder.Append(m[i, j].ToString().PadRight(15));
                }
                if (i != 3)
                {
                    stringBuilder.Append("\n");
                }
            }
            return stringBuilder.ToString();
        }

        public static string AsString(this Vector3 v)
        {
            return "(" + v.x + " , " + v.y + " , " + v.z + ")";
        }

        public static string AsString(this Vector2 v)
        {
            return "(" + v.x + " , " + v.y + ")";
        }

        

        public static string AsString(this Ray r)
        {
            return r.origin.AsString() + "\t->\t" + r.direction.AsString();
        }

        public static string AsString(this Quaternion q)
        {
            return "(" + q.x + " , " + q.y + " , " + q.z + " , " + q.w + ")";
        }

        

        public static bool GetB0(this byte b)
        {
            return (b & 1) == 1;
        }

        public static bool GetB1(this byte b)
        {
            return (b & 2) == 2;
        }

        public static bool GetB2(this byte b)
        {
            return (b & 4) == 4;
        }

        public static bool GetB3(this byte b)
        {
            return (b & 8) == 8;
        }

        public static bool GetB4(this byte b)
        {
            return (b & 0x10) == 16;
        }

        public static bool GetB5(this byte b)
        {
            return (b & 0x20) == 32;
        }

        public static bool GetB6(this byte b)
        {
            return (b & 0x40) == 64;
        }

        public static bool GetB7(this byte b)
        {
            return (b & 0x80) == 128;
        }

        
        public static bool IsOrDerivesFrom(this Type t0, Type type)
        {
            if (!t0.IsSubclassOf(type))
            {
                return t0 == type;
            }
            return true;
        }

        public static int Pow(this int x, uint pow)
        {
            int num = 1;
            while (pow != 0)
            {
                if ((pow & 1) == 1)
                {
                    num *= x;
                }
                x *= x;
                pow >>= 1;
            }
            return num;
        }

        public static bool Approximately(this float x, float y)
        {
            return Mathf.Approximately(x, y);
        }

        

        public static string WithCommas(this int n)
        {
            return n.ToString("#,##0");
        }

        public static string WithCommas(this long n)
        {
            return n.ToString("#,##0");
        }

        public static string Fraction(this decimal n, uint digits)
        {
            return ((double)n).Fraction(digits);
        }

        public static string Fraction(this float n, uint digits)
        {
            return ((double)n).Fraction(digits);
        }

        public static string Fraction(this double n, uint digits)
        {
            int num = 10.Pow(digits);
            return Mathf.RoundToLong(n % 1.0 * (double)num).WithCommas().PadLeft((int)digits, '0');
        }

        public static string WithCommas(this float n)
        {
            return $"{n:n2}";
        }

        public static string WithCommas(this double n)
        {
            return $"{n:n2}";
        }

        public static string GetSearchable(this string text)
        {
            string input = text.ToLower().RemoveDiacritics().Replace("'", " ")
                .Replace(",", " ");
            RegexOptions options = RegexOptions.None;
            return new Regex("[ ]{2,}", options).Replace(input, " ");
        }

        public static string RemoveDiacritics(this string text)
        {
            string text2 = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();
            string text3 = text2;
            string text4 = text3;
            foreach (char c in text4)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }

}
