using CCEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.Classes
{

    public class Shape
    {
        public int[] Dimensions { get; private set; } = new int[0];


        public int Length => Dimensions.Length;

        public int this[int i]
        {
            get
            {
                return Dimensions[ConvertIndex(i)];
            }
            set
            {
                Dimensions[ConvertIndex(i)] = value;
            }
        }

        public Shape()
        {
        }

        public Shape(params int[] dimensions)
        {
            Dimensions = dimensions;
        }

        public Shape GetCopy()
        {
            return new Shape(Dimensions.GetCopy());
        }

        public static Shape OfArray(Array array)
        {
            return new Shape(new Range(array.Rank).Select((int i) => array.GetLength(i)).ToArray());
        }

        private int ConvertIndex(int i)
        {
            if (i < 0)
            {
                return Length + i;
            }
            return i;
        }

        public int GetTotalCount()
        {
            int num = 1;
            int[] dimensions = Dimensions;
            int[] array = dimensions;
            foreach (int num2 in array)
            {
                num *= num2;
            }
            return num;
        }

        public void RemoveAmbiguity(int totalCount)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < Dimensions.Length; i++)
            {
                if (Dimensions[i] == -1)
                {
                    list.Add(i);
                }
            }
            if (list.Count == 1)
            {
                Dimensions[list[0]] = totalCount / -GetTotalCount();
            }
            else if (list.Count != 0)
            {
                throw new Exception("Only one dimension can be -1");
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Shape shape))
            {
                return false;
            }
            return shape.Dimensions.SequenceEqual(Dimensions);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "Shape (" + string.Join(", ", Dimensions) + ")";
        }
    }

}
