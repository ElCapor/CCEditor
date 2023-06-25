using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.Classes
{
    [Serializable]
    public struct InspectorTRS
    {
        public string name;

        public Vector3 position;

        public Vector3 eulerAngles;

        public Vector3 scale;
    }
}
