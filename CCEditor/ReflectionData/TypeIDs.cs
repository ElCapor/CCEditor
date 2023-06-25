using CCEditor.Classes.Artificial_Intelligence;
using CCEditor.Classes.Enums;
using CCEditor.Classes.Master_File_Pieces;
using CCEditor.Classes.Mocap_Helpers;
using CCEditor.Classes.UnityEngine;
using CCEditor.Classes.WAI;
using CCEditor.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.ReflectionData
{
    public static class TypeIDs
    {
        public class TypeID
        {
            public int groupID;

            public int subGroupID;
        }

        public class TypeGroup
        {
            public string name;

            public int groupID;

            public Dictionary<int, Type> classes;
        }

        public static Dictionary<int, TypeGroup> groups;

        public static Dictionary<Type, TypeID> type2ID;

        public static TypeID GetTypeID(Type tp)
        {
            Type key = ReflectionHelpers.NaturalizeType(tp, naturalizeEnum: false);
            if (type2ID.TryGetValue(key, out var value))
            {
                return value;
            }
            throw new Exception($"Type ---- {tp} ---- hasn't been added to the serialization dictionary.");
        }

        static TypeIDs()
        {
            List<TypeGroup> list = new List<TypeGroup>();
            type2ID = new Dictionary<Type, TypeID>();
            list.Add(new TypeGroup
            {
                groupID = 0,
                name = ".NET Primitives",
                classes = new Dictionary<int, Type>
                {
                    {
                        0,
                        typeof(bool)
                    },
                    {
                        1,
                        typeof(byte)
                    },
                    {
                        2,
                        typeof(char)
                    },
                    {
                        3,
                        typeof(decimal)
                    },
                    {
                        4,
                        typeof(double)
                    },
                    {
                        5,
                        typeof(float)
                    },
                    {
                        6,
                        typeof(int)
                    },
                    {
                        7,
                        typeof(long)
                    },
                    {
                        8,
                        typeof(object)
                    },
                    {
                        9,
                        typeof(sbyte)
                    },
                    {
                        10,
                        typeof(short)
                    },
                    {
                        11,
                        typeof(string)
                    },
                    {
                        12,
                        typeof(uint)
                    },
                    {
                        13,
                        typeof(ulong)
                    },
                    {
                        14,
                        typeof(ushort)
                    },
                    {
                        15,
                        typeof(DateTime)
                    },
                    {
                        17,
                        typeof(Tuple)
                    },
                    {
                        18,
                        typeof(TimeSpan)
                    }
                }
            });
            list.Add(new TypeGroup
            {
                groupID = 1,
                name = "Collections",
                classes = new Dictionary<int, Type>
                {
                    {
                        0,
                        typeof(Array)
                    },
                    {
                        1,
                        typeof(List<>)
                    },
                    {
                        2,
                        typeof(Dictionary<, >)
                    }
                }
            });
            list.Add(new TypeGroup
            {
                groupID = 2,
                name = ".Net Included",
                classes = new Dictionary<int, Type>
                {
                    {
                        0,
                        typeof(Type)
                    },
                    {
                        1,
                        ReflectionHelpers.runtimeType
                    }
                }
            });
            list.Add(new TypeGroup
            {
                groupID = 256,
                name = "Unity Objects",
                classes = new Dictionary<int, Type>
                {
                    {
                        0,
                        typeof(Color)
                    },
                    {
                        1,
                        typeof(Color32)
                    },
                    {
                        2,
                        typeof(Matrix4x4)
                    },
                    {
                        3,
                        typeof(Quaternion)
                    },
                    {
                        4,
                        typeof(Vector2)
                    },
                    {
                        5,
                        typeof(Vector2Int)
                    },
                    {
                        6,
                        typeof(Vector3)
                    },
                    {
                        7,
                        typeof(Vector3Int)
                    },
                    {
                        8,
                        typeof(Vector4)
                    },
                    {
                        9,
                        typeof(Ray)
                    }
                }
            });
            list.Add(new TypeGroup
            {
                groupID = 512,
                name = "WAI Objects",
                classes = new Dictionary<int, Type>
                {
                    {
                        0,
                        typeof(ComputeTransform)
                    },
                    {
                        1,
                        typeof(InspectorTRS)
                    },
                    {
                        3,
                        typeof(CompressedMember)
                    },
                    {
                        4,
                        typeof(LazySerializableSequence)
                    },
                    {
                        5,
                        typeof(LazySerializableElement)
                    },
                    {
                        6,
                        typeof(LazySerializableSequence<>)
                    },
                    {
                        7,
                        typeof(LazySerializableElement<>)
                    },
                    {
                        9,
                        typeof(SongsTable)
                    },
                    {
                        10,
                        typeof(EncryptedMember)
                    },
                    {
                        11,
                        typeof(RemoteConfig)
                    },
                    {
                        12,
                        typeof(LanguageTable)
                    },
                    {
                        13,
                        typeof(Shape)
                    },
                    {
                        14,
                        typeof(BitArray)
                    },
                    {
                        15,
                        typeof(Array3DUncompressed)
                    },
                    {
                        16,
                        typeof(Array3DCompressed)
                    },
                    {
                        17,
                        typeof(PackedArray)
                    },
                    {
                        18,
                        typeof(OurException)
                    },
                    {
                        19,
                        typeof(CompressedAudio)
                    }
                }
            });
            list.Add(new TypeGroup
            {
                groupID = 513,
                name = "Master File Pieces",
                classes = new Dictionary<int, Type>
                {
                    {
                        0,
                        typeof(MasterFile)
                    },
                    {
                        1,
                        typeof(NDAnimationLegacy)
                    },
                    {
                        2,
                        typeof(PlayableNotes)
                    },
                    {
                        3,
                        typeof(MecanimFile)
                    },
                    {
                        4,
                        typeof(MXLScoreFile)
                    },
                    {
                        6,
                        typeof(PerSongAdjustments.MarkerAdjustment)
                    },
                    {
                        7,
                        typeof(PerSongAdjustments)
                    },
                    {
                        8,
                        typeof(MocapGlobalShifts)
                    },
                    {
                        9,
                        typeof(MasterFileMetaData)
                    },
                    {
                        10,
                        typeof(FaceAnimation)
                    },
                    {
                        11,
                        typeof(OtherSongData)
                    },
                    {
                        12,
                        typeof(InitialPose)
                    },
                    {
                        13,
                        typeof(IKPassComponent)
                    },
                    {
                        14,
                        typeof(SongLyrics)
                    },
                    {
                        15,
                        typeof(MIDIScoreSync)
                    },
                    {
                        16,
                        typeof(FileComponent)
                    },
                    {
                        17,
                        typeof(SVGScoreFile)
                    },
                    {
                        18,
                        typeof(MeshScore)
                    },
                    {
                        19,
                        typeof(SoundTrack)
                    },
                    {
                        20,
                        typeof(BodyGuide)
                    },
                    {
                        21,
                        typeof(HeadAnimation)
                    },
                    {
                        22,
                        typeof(SyntheticComponents)
                    },
                    {
                        23,
                        typeof(NDV3Animation)
                    },
                    {
                        24,
                        typeof(NDQTAnimation)
                    },
                    {
                        25,
                        typeof(AISongInfo)
                    }
                }
            });
            list.Add(new TypeGroup
            {
                groupID = 514,
                name = "Mocap Helpers",
                classes = new Dictionary<int, Type> {
                {
                    0,
                    typeof(NDCompressedLegacy)
                } }
            });
            list.Add(new TypeGroup
            {
                groupID = 515,
                name = "Enums",
                classes = new Dictionary<int, Type>
                {
                    {
                        0,
                        typeof(AnimationTarget)
                    },
                    {
                        1,
                        typeof(ServerRequest.RequestContentType)
                    },
                    {
                        2,
                        typeof(OffOnAuto)
                    }
                }
            });
            list.Add(new TypeGroup
            {
                groupID = 1026,
                name = "Artificial Intelligence",
                classes = new Dictionary<int, Type>
                {
                    {
                        1,
                        typeof(AIBridgeNote)
                    },
                    {
                        8,
                        typeof(ServerRequest)
                    }
                }
            });
            groups = list.ToDictionary((TypeGroup x) => x.groupID, (TypeGroup x) => x);
            foreach (int key in groups.Keys)
            {
                foreach (KeyValuePair<int, Type> @class in groups[key].classes)
                {
                    type2ID.Add(@class.Value, new TypeID
                    {
                        groupID = key,
                        subGroupID = @class.Key
                    });
                }
            }
        }
    }
}
