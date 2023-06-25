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
    public class BinaryReader2 : BinaryReader1
    {
        public BinaryReader2(Stream stream)
            : base(stream)
        {
        }

        public Color ReadColor()
        {
            return new Color(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        public Color32 ReadColor32()
        {
            return new Color32(ReadByte(), ReadByte(), ReadByte(), ReadByte());
        }

        public Matrix4x4 ReadMatrix4x4()
        {
            return new Matrix4x4(ReadVector4(), ReadVector4(), ReadVector4(), ReadVector4());
        }

        public Quaternion ReadQuaternion()
        {
            return new Quaternion(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        public Vector2 ReadVector2()
        {
            return new Vector2(ReadSingle(), ReadSingle());
        }

        public Vector2Int ReadVector2Int()
        {
            return new Vector2Int(ReadInt32(), ReadInt32());
        }

        public Vector3 ReadVector3()
        {
            return new Vector3(ReadSingle(), ReadSingle(), ReadSingle());
        }

        public Vector3Int ReadVector3Int()
        {
            return new Vector3Int(ReadInt32(), ReadInt32(), ReadInt32());
        }

        public Vector4 ReadVector4()
        {
            return new Vector4(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        public Ray ReadRay()
        {
            return new Ray(ReadVector3(), ReadVector3());
        }

        public ComputeTransform ReadComputeTransform()
        {
            return ComputeTransform.TRS(ReadVector3(), ReadVector3(), ReadVector3());
        }

        public DateTime ReadDateTime()
        {
            return new DateTime(ReadInt64());
        }

        public TimeSpan ReadTimeSpan()
        {
            return new TimeSpan(ReadInt64());
        }

        public InspectorTRS ReadInspectorTRS()
        {
            InspectorTRS result = default(InspectorTRS);
            result.name = ReadString();
            result.position = ReadVector3();
            result.eulerAngles = ReadVector3();
            result.scale = ReadVector3();
            return result;
        }

        public Shape ReadShape()
        {
            int num = Read7BitEncodedInt();
            int[] array = new int[num];
            for (int i = 0; i < num; i++)
            {
                array[i] = Read7BitEncodedInt();
            }
            return new Shape(array);
        }

        public BitArray ReadBitArray()
        {
            int count = Read7BitEncodedInt();
            return new BitArray(ReadBytes(count));
        }
    }
}

