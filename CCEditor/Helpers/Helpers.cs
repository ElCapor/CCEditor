using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.Helpers
{
    public class Helpers
    {
        public static int BinarySearch<T>(T[] sortedContainer, T valueToFind) where T : IComparable
        {
            int num = Array.BinarySearch(sortedContainer, valueToFind);
            if (num >= 0)
            {
                return num;
            }
            return ~num;
        }
        public static string NewLineJoin<T>(IEnumerable<T> ie)
        {
            return string.Join("\n", ie);
        }
    }
}
