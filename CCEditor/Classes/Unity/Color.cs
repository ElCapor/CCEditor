using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.Classes
{
    public class Color
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public float this[int index]
        {
            get
            {
                return index switch
                {
                    0 => r,
                    1 => g,
                    2 => b,
                    3 => a,
                    _ => throw new IndexOutOfRangeException("Invalid Color index(" + index + ")!"),
                };
            }
            set
            {
                switch (index)
                {
                    case 0:
                        r = value; break;
                    case 1:
                        g = value; break;
                    case 2:
                        b = value; break;
                    case 3:
                        a = value; break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Color index(" + index + ")!");
                }
            }
        }
        public Color(float r, float g, float b, float a) 
        { 
            this.a = a;
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public Color()
        {
            this.r = 0.0f;
            this.g = 0.0f;
            this.b = 0.0f;
            this.a = 1.0f;
        }
    }
}
