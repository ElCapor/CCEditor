using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor
{
    public static class Mathf
    {
        public const float Epsilon = 1.401298E-45f;

        public static float Clamp01(float value)
        {
            return value < 0f ? 0f : (value > 1f ? 1f : value);
        }
        public static int RoundToInt(float value)
        {
            return (int)Mathf.Round(value);
        }

        public static float Round(float value)
        {
            return (float)Math.Round(value);
        }

        public static byte RoundToByte(float value)
        {
            return (byte)Mathf.RoundToInt(value);
        }
        public static float LinearToGammaSpace(float value)
        {
            return Mathf.Clamp01(Mathf.Pow(value, 1f / 2.2f));
        }
        public static float GammaToLinearSpace(float value)
        {
            return Mathf.Clamp01(Mathf.Pow(value, 2.2f));
        }
        public static float Pow(float x, float y)
        {
            return (float)Math.Pow(x, y);
        }

        public static float Floor(float value)
        {
            return (float)Math.Floor(value);
        }

        public static float Max(float a, float b)
        {
            return a > b ? a : b;
        }

        public static float Min(float a, float b)
        {
            return a < b ? a : b;
        }
        public static bool Approximately(float a, float b)
        {
            const float epsilon = 1e-6f; // Adjust this tolerance as per your requirements

            // Check if the absolute difference between a and b is less than the tolerance
            return Mathf.Abs(a - b) < epsilon;
        }
        public static float Clamp(float value, float min, float max)
        {
            return value < min ? min : (value > max ? max : value);
        }

        public static int Sign(float value)
        {
            return value < 0f ? -1 : (value > 0f ? 1 : 0);
        }

        public static int Min(int a, int b)
        {
            return a < b ? a : b;
        }

        public static int Max(int a, int b)
        {
            return a > b ? a : b;
        }

        public static float Sqrt(float value)
        {
            return (float)Math.Sqrt(value);
        }

        public static int Sqrt(int value)
        {
            return (int)Math.Sqrt(value);
        }

        public static int FloorToInt(float value)
        {
            return (int)Math.Floor(value);
        }

        public static int CeilToInt(float value)
        {
            return (int)Math.Ceiling(value);
        }

        public static float Acos(float value)
        {
            return (float)Math.Acos(value);
        }

        public static float Abs(float value)
        {
            return Math.Abs(value);
        }
        public static long RoundToLong(double d)
        {
            double value = Math.Round(d);
            try
            {
                return Convert.ToInt64(value);
            }
            catch
            {
                throw new Exception(d.ToString());
            }
        }
    }
}
