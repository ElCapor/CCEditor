using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.Classes
{
    public class Range : IEnumerable<int>, IEnumerable
    {
        private int min;

        private int max;

        private int currentValue;

        public Range(int max)
        {
            this.max = max;
        }

        public Range(int min, int max)
        {
            this.min = min;
            currentValue = min;
            this.max = max;
            if (min > max)
            {
                throw new Exception("max must be smaller ");
            }
        }

        public IEnumerator<int> GetEnumerator()
        {
            while (currentValue < max)
            {
                yield return currentValue;
                currentValue++;
            }
            currentValue = min;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
