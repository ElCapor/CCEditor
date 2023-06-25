using CCEditor.Classes.UnityEngine;
using CCEditor.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.CC
{
    public class BinaryWriter2 : BinaryWriter1
    {
        public BinaryWriter2(Stream stream)
            : base(stream)
        {
        }

        public void Write(Color toWrite)
        {
            Write(toWrite.r);
            Write(toWrite.g);
            Write(toWrite.b);
            Write(toWrite.a);
        }

        public void Write(Color32 toWrite)
        {
            Write(toWrite.r);
            Write(toWrite.g);
            Write(toWrite.b);
            Write(toWrite.a);
        }

        public void Write(Matrix4x4 toWrite)
        {
            Write(toWrite.GetColumn(0));
            Write(toWrite.GetColumn(1));
            Write(toWrite.GetColumn(2));
            Write(toWrite.GetColumn(3));
        }

        public void Write(Quaternion toWrite)
        {
            Write(toWrite.x);
            Write(toWrite.y);
            Write(toWrite.z);
            Write(toWrite.w);
        }

        public void Write(Vector2 toWrite)
        {
            Write(toWrite.x);
            Write(toWrite.y);
        }

        public void Write(Vector2Int toWrite)
        {
            Write(toWrite.x);
            Write(toWrite.y);
        }

        public void Write(Vector3 toWrite)
        {
            Write(toWrite.x);
            Write(toWrite.y);
            Write(toWrite.z);
        }

        public void Write(Vector3Int toWrite)
        {
            Write(toWrite.x);
            Write(toWrite.y);
            Write(toWrite.z);
        }

        public void Write(Vector4 toWrite)
        {
            Write(toWrite.x);
            Write(toWrite.y);
            Write(toWrite.z);
            Write(toWrite.w);
        }

        public void Write(Ray toWrite)
        {
            Write(toWrite.origin);
            Write(toWrite.direction);
        }

        public void Write(ComputeTransform toWrite)
        {
            Write(toWrite.Position);
            Write(toWrite.EulerAngles);
            Write(toWrite.Scale);
        }

        public void Write(InspectorTRS toWrite)
        {
            Write(toWrite.name);
            Write(toWrite.position);
            Write(toWrite.eulerAngles);
            Write(toWrite.scale);
        }

        public void Write(DateTime toWrite)
        {
            Write(toWrite.Ticks);
        }

        public void Write(TimeSpan toWrite)
        {
            Write(toWrite.Ticks);
        }

        public override void Write(string toWrite)
        {
            if (string.IsNullOrEmpty(toWrite))
            {
                base.Write(string.Empty);
            }
            else
            {
                base.Write(toWrite);
            }
        }

        public override void Write(decimal d)
        {
            base.Write(d);
        }

        public void Write32BitEncodedInt(int i)
        {
            Write(i);
        }

        public void Write(Shape shp)
        {
            Write7BitEncodedInt(shp.Length);
            for (int i = 0; i < shp.Length; i++)
            {
                Write7BitEncodedInt(shp[i]);
            }
        }

        public void Write(BitArray ba)
        {
            Write7BitEncodedInt(ba.backBone.Length);
            Write(ba.backBone);
        }
    }
}
