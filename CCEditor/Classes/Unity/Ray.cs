using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.Classes
{
    public struct Ray : IFormattable
    {
        private Vector3 m_Origin;

        private Vector3 m_Direction;

        public Vector3 origin
        {
            get
            {
                return m_Origin;
            }
            set
            {
                m_Origin = value;
            }
        }

        public Vector3 direction
        {
            get
            {
                return m_Direction;
            }
            set
            {
                m_Direction = value.normalized;
            }
        }

        public Ray(Vector3 origin, Vector3 direction)
        {
            m_Origin = origin;
            m_Direction = direction.normalized;
        }

        public Vector3 GetPoint(float distance)
        {
            return m_Origin + m_Direction * distance;
        }

        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture.NumberFormat);
        }

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture.NumberFormat);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "F1";
            }

            return String.Format("Origin: {0}, Dir: {1}", new object[2]
            {
                m_Origin.ToString(format, formatProvider),
                m_Direction.ToString(format, formatProvider)
            });
        }
    }
}
