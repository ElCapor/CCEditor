using CCEditor.CC.Attributes;
using CCEditor.CC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.Classes.Master_File_Pieces
{
    public class IKPassComponent : FileComponent, IPreSerializeByID
    {
        [SerializeByID(0, false)]
        private Vector3 leftWristShift;

        [SerializeByID(1, false)]
        private Vector3 rightWristShift;

        [SerializeByID(2, false)]
        private float ikWeight;

        [SerializeByID(3, false)]
        private float ikRotationWeight;

        public void OnPreSerialize()
        {
        }
    }
}
