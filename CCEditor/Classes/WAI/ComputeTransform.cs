using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.Classes
{
    public class ComputeTransform
    {
        protected Vector3 _position;

        protected Vector3 _eulerAngles;

        protected Vector3 _scale = Vector3.one;
        public ComputeTransform() { }
        public ComputeTransform(Vector3 position, Vector3 eulerAngles, Vector3 scale)
        {
            _position = position;
            _eulerAngles = eulerAngles;
            _scale = scale;
        }

        public static ComputeTransform TRS(Vector3 position, Vector3 eulerAngles, Vector3 scale)
        {
            return new ComputeTransform(position, eulerAngles, scale);
        }

    }
}
