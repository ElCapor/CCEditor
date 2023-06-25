using CCEditor.Classes.Master_File_Pieces;
using CCEditor.Classes.Mocap_Helpers;
using CCEditor.Classes.UnityEngine;
using CCEditor.Classes.WAI;
using CCEditor.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BitArray = CCEditor.Classes.BitArray;

namespace CCEditor.ReflectionData
{
    public static class ReflectionHelpers
    {
        public static readonly Type[] typesWithSpecificWriters = new Type[30]
        {
            typeof(bool),
            typeof(byte),
            typeof(char),
            typeof(decimal),
            typeof(double),
            typeof(float),
            typeof(int),
            typeof(long),
            typeof(sbyte),
            typeof(short),
            typeof(string),
            typeof(uint),
            typeof(ulong),
            typeof(ushort),
            typeof(Color),
            typeof(Color32),
            typeof(Quaternion),
            typeof(Matrix4x4),
            typeof(Vector2),
            typeof(Vector2Int),
            typeof(Vector3),
            typeof(Vector3Int),
            typeof(Vector4),
            typeof(Ray),
            typeof(ComputeTransform),
            typeof(InspectorTRS),
            typeof(DateTime),
            typeof(TimeSpan),
            typeof(Shape),
            typeof(BitArray)
        };

        public static readonly Type runtimeType = Type.GetType("System.RuntimeType");

        public static Array List2Array(object list)
        {
            IList list2 = (IList)list;
            Array array = Array.CreateInstance(list.GetType().GetGenericArguments()[0], list2.Count);
            list2.CopyTo(array, 0);
            return array;
        }

        public static void StripingStitch()
        {
            ObjectReader objectReader = new ObjectReader(new MemoryStream(new byte[512]));
            objectReader.ReadColor();
            objectReader.ReadColor32();
            objectReader.ReadQuaternion();
            objectReader.ReadMatrix4x4();
            objectReader.ReadVector2();
            objectReader.ReadVector2Int();
            objectReader.ReadVector3();
            objectReader.ReadVector3Int();
            objectReader.ReadVector4();
            objectReader.ReadRay();
            objectReader.ReadComputeTransform();
            objectReader.ReadInspectorTRS();
            objectReader.ReadDateTime();
            objectReader.ReadTimeSpan();
            objectReader.ReadShape();
            objectReader.ReadBitArray();
            objectReader.Close();
            ObjectWriter objectWriter = new ObjectWriter(new MemoryStream());
            objectWriter.Write(0m);
            objectWriter.Write((sbyte)0);
            objectWriter.Write(default(Color));
            objectWriter.Write(default(Color32));
            objectWriter.Write(default(Quaternion));
            objectWriter.Write(default(Matrix4x4));
            objectWriter.Write(default(Vector2));
            objectWriter.Write(default(Vector2Int));
            objectWriter.Write(default(Vector3));
            objectWriter.Write(default(Vector3Int));
            objectWriter.Write(default(Vector4));
            objectWriter.Write(default(Ray));
            objectWriter.Write(new ComputeTransform());
            objectWriter.Write(default(InspectorTRS));
            objectWriter.Write(default(DateTime));
            objectWriter.Write(default(TimeSpan));
            objectWriter.Write(new Shape());
            objectWriter.Write(new BitArray());
            objectWriter.Write(new PackedArray(new byte[0]));
            object[] array = new object[30]
            {
                new CompressedMember(),
                new LazySerializableElement(),
                new LazySerializableElement<FileComponent>(),
                new EncryptedMember(),
                new RemoteConfig(),
                new NDCompressedLegacy(),
                new SongsTable(),
                new LanguageTable(),
                new MasterFile(),
                new NDAnimationLegacy(),
                new PlayableNotes(),
                new MecanimFile(),
                new SoundTrack(),
                new MXLScoreFile(),
                default(PerSongAdjustments.MarkerAdjustment),
                new MocapGlobalShifts(),
                new MasterFileMetaData(),
                new FaceAnimation(),
                new OtherSongData(),
                new InitialPose(),
                new IKPassComponent(),
                new SongLyrics(),
                new MIDIScoreSync(),
                new FileComponent(),
                new SVGScoreFile(),
                new MeshScore(),
                new LazySerializableSequence(),
                new LazySerializableSequence<FileComponent>(),
                new Shape(),
                new BitArray()
            };
            objectWriter.Write(array.Length);
            objectWriter.Close();
        }

        public static bool IsTypeArrayOfBuiltIn(Type arrayType)
        {
            if (IsArray(arrayType))
            {
                return arrayType.GetElementType().IsPrimitive;
            }
            return false;
        }

        public static bool IsAnyArrayItemSubclassed(Array array)
        {
            Type elementType = array.GetType().GetElementType();
            foreach (object item in array)
            {
                if (item.GetType() != elementType)
                {
                    return true;
                }
            }
            return false;
        }

        public static void IsDictionarySubclassed(IDictionary IDictionary, out bool keysSubclassed, out bool valuesSubclassed)
        {
            Type[] genericArguments = IDictionary.GetType().GetGenericArguments();
            Type type = genericArguments[0];
            Type type2 = genericArguments[1];
            keysSubclassed = false;
            valuesSubclassed = false;
            foreach (DictionaryEntry item in IDictionary)
            {
                if (item.Key.GetType() != type)
                {
                    keysSubclassed = true;
                }
                if (item.Value.GetType() != type2)
                {
                    valuesSubclassed = true;
                }
            }
        }

        public static Type NaturalizeType(Type tp, bool naturalizeEnum)
        {
            if (IsArray(tp))
            {
                return typeof(Array);
            }
            if (typeof(ITuple).IsAssignableFrom(tp))
            {
                return typeof(Tuple);
            }
            if (naturalizeEnum && tp.IsSubclassOf(typeof(Enum)))
            {
                return typeof(Enum);
            }
            if (tp.IsGenericType)
            {
                return tp.GetGenericTypeDefinition();
            }
            return tp;
        }

        public static bool IsArray(Type tp)
        {
            if (!(tp == typeof(Array)))
            {
                return tp.IsArray;
            }
            return true;
        }

        public static T[] ReinterpretBytes<T>(this Array arr) where T : struct
        {
            Type elementType = arr.GetType().GetElementType();
            Type typeFromHandle = typeof(T);
            int num = Marshal.SizeOf(elementType);
            int num2 = Marshal.SizeOf(typeFromHandle);
            int num3 = arr.Length * num;
            T[] array = new T[num3 / num2];
            Buffer.BlockCopy(arr, 0, array, 0, num3);
            return array;
        }

        public static Array ReinterpretBytesAndShape<T>(this Array arr, Shape shp)
        {
            int num = Marshal.SizeOf(arr.GetType().GetElementType());
            int count = arr.Length * num;
            Array array = Array.CreateInstance(typeof(T), shp.Dimensions);
            Buffer.BlockCopy(arr, 0, array, 0, count);
            return array;
        }
    }
}
