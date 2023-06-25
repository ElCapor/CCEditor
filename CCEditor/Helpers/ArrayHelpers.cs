using CCEditor.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.Helpers
{
    public static class ArrayHelpers
    {
        [Serializable]
        [CompilerGenerated]
        private sealed class _003C_003Ec
        {
            public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

            public static Func<int, int[]> _003C_003E9__5_0;

            public static Func<Array, Shape> _003C_003E9__9_0;

            public static Func<Shape, int> _003C_003E9__9_1;

            public static Func<Array, int> _003C_003E9__13_0;

            internal int[] _003CReshapeArray_003Eb__5_0(int x)
            {
                return new int[1] { x };
            }

            internal Shape _003CConcatenate_003Eb__9_0(Array x)
            {
                return x.GetShape();
            }

            internal int _003CConcatenate_003Eb__9_1(Shape x)
            {
                return x[0];
            }

            internal int _003CFlattenNestedArrayNonGeneric_003Eb__13_0(Array x)
            {
                return x.Length;
            }
        }

        public static Array Reshape(this Array source, params int[] destShape)
        {
            return ReshapeArray(source, new Shape(destShape));
        }

        public static Array Reshape(this Array source, Shape destShape)
        {
            return ReshapeArray(source, destShape);
        }

        public static IEnumerable<int[]> GetNDIterator(this Array arr)
        {
            Shape shape = arr.GetShape();
            int[] dims = shape.Dimensions;
            int total = shape.GetTotalCount() - 1;
            int[] current = new int[dims.Length];
            int lastDim = dims.Length - 1;
            for (int i = 0; i < total; i++)
            {
                yield return current;
                IncrementIndeces(current, dims, lastDim);
            }
            yield return current;
        }

        private static void IncrementIndeces(int[] current, int[] maxes, int currentDim)
        {
            while (true)
            {
                int num = ++current[currentDim];
                int num2 = maxes[currentDim];
                if (num == num2)
                {
                    current[currentDim] = 0;
                    currentDim--;
                    continue;
                }
                break;
            }
        }

        public static Array Transpose(this Array source)
        {
            int[] lengths = source.GetShape().Dimensions.Reverse().ToArray();
            Array array = Array.CreateInstance(source.GetType().GetElementType(), lengths);
            foreach (int[] item in source.GetNDIterator())
            {
                object value = source.GetValue(item);
                array.SetValue(value, item.Reverse().ToArray());
            }
            return array;
        }

        public static Array ReshapeArray(Array source, Shape destShape)
        {
            Type elementType = source.GetType().GetElementType();
            Shape shape = source.GetShape();
            int totalCount = shape.GetTotalCount();
            destShape.RemoveAmbiguity(totalCount);
            Array array = Array.CreateInstance(elementType, destShape.Dimensions);
            if (destShape.GetTotalCount() != totalCount)
            {
                throw new Exception("Cannot Reshape (" + string.Join(", ", shape.Dimensions) + ") into (" + string.Join(", ", destShape.Dimensions) + ").");
            }
            if (elementType.IsPrimitive)
            {
                int num = Marshal.SizeOf(elementType);
                Buffer.BlockCopy(source, 0, array, 0, source.Length * num);
            }
            else
            {
                if (_003C_003Ec._003C_003E9__5_0 == null)
                {
                    _003C_003Ec._003C_003E9__5_0 = (int x) => new int[1] { x };
                }
                Func<int, int[]> func = ((source.Rank == 1) ? null : source.GetFlattener());
                Func<int, int[]> func2 = ((array.Rank == 1) ? null : array.GetFlattener());
                source.GetShape().GetTotalCount();
                for (int i = 0; i < source.Length; i++)
                {
                    array.SetValue(source.GetValue(func(i)), func2(i));
                }
            }
            return array;
        }

        public static Array Flat(this Array source)
        {
            return ReshapeArray(source, new Shape(-1));
        }

        public static T[] FlattenMultiDimensional<T>(Array source)
        {
            Type elementType = source.GetType().GetElementType();
            int num = Marshal.SizeOf(elementType);
            int totalCount = source.GetShape().GetTotalCount();
            Array array = Array.CreateInstance(elementType, totalCount);
            Buffer.BlockCopy(source, 0, array, 0, totalCount * num);
            return (T[])array;
        }

        public static T[,] Transpose<T>(T[,] source)
        {
            Shape shape = source.GetShape();
            T[,] array = new T[shape[1], shape[0]];
            for (int i = 0; i < shape[0]; i++)
            {
                for (int j = 0; j < shape[1]; j++)
                {
                    array[j, i] = source[i, j];
                }
            }
            return array;
        }

        public static Array Concatenate(params Array[] arrs)
        {
            Shape[] array = arrs.Select((Array x) => x.GetShape()).ToArray();
            Shape shape = array[0];
            for (int i = 1; i < arrs.Length; i++)
            {
                bool flag = false;
                Shape shape2 = array[i];
                if (shape2.Length != shape.Length)
                {
                    flag = true;
                }
                for (int j = 1; j < shape.Length; j++)
                {
                    if (shape2[j] != shape[j])
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    throw new Exception(string.Concat("Unmatching shapes ", shape, " and ", shape2));
                }
            }
            Shape copy = shape.GetCopy();
            copy.Dimensions[0] = array.Sum((Shape x) => x[0]);
            Type elementType = arrs[0].GetType().GetElementType();
            int num = Marshal.SizeOf(elementType);
            Array array2 = Array.CreateInstance(elementType, copy.Dimensions);
            _ = array2.Length;
            int num2 = 0;
            foreach (Array array3 in arrs)
            {
                int num3 = array3.Length * num;
                Buffer.BlockCopy(array3, 0, array2, num2, num3);
                num2 += num3;
            }
            return array2;
        }

        public static T[,] NestedTo2DArray<T>(T[][] nested)
        {
            return (T[,])NestedToNDArray(nested);
        }

        public static Array NestedToNDArray(Array[] source)
        {
            int[] dimensions = source[0].GetShape().Dimensions.Prepend(source.Length).ToArray();
            return ReshapeArray(FlattenNestedArrayNonGeneric(source), new Shape(dimensions));
        }

        public static T[] FlattenNestedArray<T>(T[][] nested)
        {
            return (T[])FlattenNestedArrayNonGeneric(nested);
        }

        public static Array FlattenNestedArrayNonGeneric(Array[] nested)
        {
            Type elementType = nested.GetType().GetElementType().GetElementType();
            int length = nested.Sum((Array x) => x.Length);
            Array array = Array.CreateInstance(elementType, length);
            if (elementType.IsPrimitive)
            {
                int num = Marshal.SizeOf(elementType);
                int num2 = 0;
                foreach (Array array2 in nested)
                {
                    int num3 = array2.Length * num;
                    Buffer.BlockCopy(array2, 0, array, num2, num3);
                    num2 += num3;
                }
            }
            else
            {
                int num4 = 0;
                for (int j = 0; j < nested.Length; j++)
                {
                    foreach (object item in nested[j])
                    {
                        array.SetValue(item, num4++);
                    }
                }
            }
            return array;
        }

        public static void SetRow<T>(this T[,] table, int rowIndex, T[] row)
        {
            for (int i = 0; i < row.Length; i++)
            {
                table[rowIndex, i] = row[i];
            }
        }

        public static T[][] NestedFrom2DArray<T>(T[,] source)
        {
            int length = source.GetLength(0);
            int length2 = source.GetLength(1);
            T[][] array = new T[length][];
            for (int i = 0; i < length; i++)
            {
                T[] array2 = new T[length2];
                for (int j = 0; j < length2; j++)
                {
                    array2[j] = source[i, j];
                }
                array[i] = array2;
            }
            return array;
        }

        public static Array MultiDimensional2Flat(Array source)
        {
            Array array = Array.CreateInstance(source.GetType().GetElementType(), source.Length);
            int num = 0;
            foreach (object item in source)
            {
                array.SetValue(item, num);
                num++;
            }
            return array;
        }

        public static Func<int, int[]> GetFlattener(this Array array)
        {
            int[] significance = GetFlatStepsPerDimension(array);
            return (int x) => FlatToMultiDimensionalCoordinate(x, significance);
        }

        public static Shape GetShape(this Array array)
        {
            return Shape.OfArray(array);
        }

        private static int[] GetFlatStepsPerDimension(Array array)
        {
            Shape shape = array.GetShape();
            int[] array2 = new int[shape.Length];
            int num = 1;
            for (int num2 = shape.Length - 1; num2 >= 0; num2--)
            {
                array2[num2] = num;
                num *= shape[num2];
            }
            return array2;
        }

        private static int[] FlatToMultiDimensionalCoordinate(int ind, int[] dimensionSteps)
        {
            int[] array = new int[dimensionSteps.Length];
            for (int i = 0; i < array.Length; i++)
            {
                int num = ind / dimensionSteps[i];
                array[i] = num;
                ind %= dimensionSteps[i];
            }
            return array;
        }

        public static void BufferCopyTo<T>(this T[] arr, Array target) where T : struct
        {
            int num = Marshal.SizeOf(arr.GetType().GetElementType());
            int count = arr.Length * num;
            Buffer.BlockCopy(arr, 0, target, 0, count);
        }

        public static T[] GetCopy<T>(this T[] arr) where T : struct
        {
            T[] array = new T[arr.Length];
            arr.BufferCopyTo(array);
            return array;
        }

        public static Array GetCopy(this Array arr)
        {
            Shape shape = arr.GetShape();
            Type elementType = arr.GetType().GetElementType();
            int num = Marshal.SizeOf(elementType);
            Array array = Array.CreateInstance(elementType, shape.Dimensions);
            Buffer.BlockCopy(arr, 0, array, 0, shape.GetTotalCount() * num);
            return array;
        }

        public static Array GetNDCopy(this Array source)
        {
            Shape shape = source.GetShape();
            Type elementType = source.GetType().GetElementType();
            Array array = Array.CreateInstance(elementType, shape.Dimensions);
            int num = Marshal.SizeOf(elementType);
            int totalCount = shape.GetTotalCount();
            Buffer.BlockCopy(source, 0, array, 0, num * totalCount);
            return array;
        }

        public static T[][] SplitArray<T>(T[] source, int pieceCount)
        {
            Type elementType = source.GetType().GetElementType();
            int num = Marshal.SizeOf(elementType);
            int num2 = source.Length % pieceCount;
            int num3 = source.Length / pieceCount;
            if (num2 != 0)
            {
                num3++;
            }
            int num4 = source.Length - num3 * (pieceCount - 1);
            T[][] array = new T[pieceCount][];
            int num5 = 0;
            int num6 = 0;
            if (elementType.IsPrimitive)
            {
                for (int i = 0; i < pieceCount; i++)
                {
                    int num7 = ((i == pieceCount - 1) ? num4 : num3);
                    int num8 = num7 * num;
                    T[] array2 = new T[num7];
                    Buffer.BlockCopy(source, num5, array2, 0, num8);
                    num5 += num8;
                    array[i] = array2;
                }
            }
            else
            {
                for (int j = 0; j < pieceCount; j++)
                {
                    T[] array3 = new T[(j == pieceCount - 1) ? num4 : num3];
                    for (int k = 0; k < array3.Length; k++)
                    {
                        array3[k] = source[num6++];
                    }
                    array[j] = array3;
                }
            }
            return array;
        }
    }
}
