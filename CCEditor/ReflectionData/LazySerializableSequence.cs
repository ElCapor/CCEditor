using CCEditor.CC.Attributes;
using CCEditor.CC.Interfaces;
using CCEditor.CC;
using CCEditor.Classes.WAI;
using CCEditor.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace CCEditor.ReflectionData
{
    public class LazySerializableSequence : ISelfSerializable
    {
        [SerializeByID(0, false)]
        public LazySerializableElement[] contentElements;

        public byte[] flattnenedElements;

        private int startingPos;

        private Stream s;

        public IEnumerable<object> Values => from x in contentElements
                                             select x.lazyValue.Value into x
                                             where x != null
                                             select x;

        public static LazySerializableSequence FromElements(IEnumerable<object> pieces)
        {
            byte[][] serializedPieces = pieces.Select(SerializeManager.SerializeObject).ToArray();
            Type[] types = pieces.Select((object x) => x.GetType()).ToArray();
            return FromSerializedPieces(serializedPieces, types);
        }

        public static LazySerializableSequence FromSerializedPieces(byte[][] serializedPieces, Type[] types)
        {
            LazySerializableSequence lazySerializableSequence = new LazySerializableSequence();
            lazySerializableSequence.contentElements = new LazySerializableElement[serializedPieces.Length];
            LazySerializableSequence lazySerializableSequence2 = lazySerializableSequence;
            for (int i = 0; i < serializedPieces.Length; i++)
            {
                lazySerializableSequence2.contentElements[i] = new LazySerializableElement
                {
                    length = serializedPieces[i].Length,
                    type = types[i]
                };
            }
            lazySerializableSequence2.flattnenedElements = ArrayHelpers.FlattenNestedArray(serializedPieces);
            return lazySerializableSequence2;
        }

        public T ReadPiece<T>()
        {
            LazySerializableElement[] array = contentElements;
            LazySerializableElement[] array2 = array;
            foreach (LazySerializableElement lazySerializableElement in array2)
            {
                if (lazySerializableElement.type == typeof(T))
                {
                    return (T)lazySerializableElement.lazyValue.Value;
                }
            }
            return default(T);
        }

        public void ReplacePiece<T>(T replacement)
        {
            int num = 0;
            for (int i = 0; i < contentElements.Length; i++)
            {
                LazySerializableElement lazySerializableElement = contentElements[i];
                if (lazySerializableElement.type == typeof(T))
                {
                    byte[] array = SerializeManager.SerializeObject(replacement);
                    if (array.Length == lazySerializableElement.length)
                    {
                        Buffer.BlockCopy(array, 0, flattnenedElements, num, array.Length);
                        break;
                    }
                    int num2 = flattnenedElements.Length - num - lazySerializableElement.length;
                    byte[] src = flattnenedElements;
                    flattnenedElements = new byte[num + array.Length + num2];
                    Buffer.BlockCopy(src, 0, flattnenedElements, 0, num);
                    Buffer.BlockCopy(array, 0, flattnenedElements, num, array.Length);
                    Buffer.BlockCopy(src, num + lazySerializableElement.length, flattnenedElements, num + array.Length, num2);
                    break;
                }
                num += lazySerializableElement.length;
            }
        }

        public void SerializeSelf(ObjectWriter ow)
        {
            Stream baseStream = ow.BaseStream;
            SerializeManager.SerializeToStream(baseStream, contentElements);
            baseStream.Write(flattnenedElements, 0, flattnenedElements.Length);
        }

        public object DeserializeSelf(ObjectReader or)
        {
            s = or.BaseStream;
            contentElements = SerializeManager.DeserializeStream<LazySerializableElement[]>(s);
            startingPos = (int)s.Position;
            long position = FillStartingIndices(s);
            s.Position = position;
            return this;
        }

        private long FillStartingIndices(Stream s)
        {
            long num = s.Position;
            for (int i = 0; i < contentElements.Length; i++)
            {
                contentElements[i].PrepareLazy(s, num);
                num += contentElements[i].length;
            }
            return num;
        }

        public void FillFlattened()
        {
            s.Position = startingPos;
            flattnenedElements = new byte[contentElements.Sum((LazySerializableElement x) => x.length)];
            s.Read(flattnenedElements, 0, flattnenedElements.Length);
        }

        public override string ToString()
        {
            return Helpers.Helpers.NewLineJoin(contentElements.OrderByDescending((LazySerializableElement x) => x.length));
        }
    }
    public class LazySerializableSequence<T> : ISelfSerializable
    {
        [SerializeByID(0, false)]
        public LazySerializableElement<T>[] contentElements;

        public byte[] flattnenedElements;

        private int startingPos;

        private Stream s;

        public IEnumerable<T> Values => from x in contentElements
                                        select x.lazyValue.Value into x
                                        where x != null
                                        select x;

        public static LazySerializableSequence<T> FromElements(IEnumerable<T> pieces)
        {
            byte[][] serializedPieces = pieces.OfType<object>().Select(SerializeManager.SerializeObject).ToArray();
            Type[] types = pieces.Select((T x) => x.GetType()).ToArray();
            return FromSerializedPieces(serializedPieces, types);
        }

        public static LazySerializableSequence<T> FromSerializedPieces(byte[][] serializedPieces, Type[] types)
        {
            LazySerializableSequence<T> lazySerializableSequence = new LazySerializableSequence<T>();
            lazySerializableSequence.contentElements = new LazySerializableElement<T>[serializedPieces.Length];
            LazySerializableSequence<T> lazySerializableSequence2 = lazySerializableSequence;
            for (int i = 0; i < serializedPieces.Length; i++)
            {
                lazySerializableSequence2.contentElements[i] = new LazySerializableElement<T>
                {
                    length = serializedPieces[i].Length,
                    type = types[i]
                };
            }
            lazySerializableSequence2.flattnenedElements = ArrayHelpers.FlattenNestedArray(serializedPieces);
            return lazySerializableSequence2;
        }

        public TComponent ReadPiece<TComponent>() where TComponent : T
        {
            LazySerializableElement<T>[] array = contentElements;
            LazySerializableElement<T>[] array2 = array;
            foreach (LazySerializableElement<T> lazySerializableElement in array2)
            {
                if (lazySerializableElement.type == typeof(TComponent))
                {
                    return (TComponent)(object)lazySerializableElement.lazyValue.Value;
                }
            }
            return default(TComponent);
        }

        public void ReplacePiece<TComponent>(TComponent replacement) where TComponent : T
        {
            int num = 0;
            for (int i = 0; i < contentElements.Length; i++)
            {
                LazySerializableElement<T> lazySerializableElement = contentElements[i];
                if (lazySerializableElement.type == typeof(TComponent))
                {
                    byte[] array = SerializeManager.SerializeObject(replacement);
                    if (array.Length == lazySerializableElement.length)
                    {
                        Buffer.BlockCopy(array, 0, flattnenedElements, num, array.Length);
                        return;
                    }
                    int num2 = flattnenedElements.Length - num - lazySerializableElement.length;
                    byte[] src = flattnenedElements;
                    flattnenedElements = new byte[num + array.Length + num2];
                    Buffer.BlockCopy(src, 0, flattnenedElements, 0, num);
                    Buffer.BlockCopy(array, 0, flattnenedElements, num, array.Length);
                    Buffer.BlockCopy(src, num + lazySerializableElement.length, flattnenedElements, num + array.Length, num2);
                    contentElements[i].length = array.Length;
                    return;
                }
                num += lazySerializableElement.length;
            }
            InsertPiece(contentElements.Length, replacement);
        }

        public void InsertPiece<TComponent>(int index, TComponent toInsert) where TComponent : T
        {
            if (index > contentElements.Length)
            {
                index = contentElements.Length;
            }
            byte[] array = SerializeManager.SerializeObject(toInsert);
            byte[] array2 = flattnenedElements;
            flattnenedElements = new byte[array2.Length + array.Length];
            int num = 0;
            for (int i = 0; i < index; i++)
            {
                num += contentElements[i].length;
            }
            Buffer.BlockCopy(array2, 0, flattnenedElements, 0, num);
            Buffer.BlockCopy(array, 0, flattnenedElements, num, array.Length);
            Buffer.BlockCopy(array2, num, flattnenedElements, num + array.Length, array2.Length - num);
            List<LazySerializableElement<T>> list = contentElements.ToList();
            list.Insert(index, new LazySerializableElement<T>
            {
                length = array.Length,
                type = typeof(TComponent)
            });
            contentElements = list.ToArray();
        }

        public void DeletePiece<TComponent>()
        {
            int num = 0;
            for (int i = 0; i < contentElements.Length; i++)
            {
                LazySerializableElement<T> lazySerializableElement = contentElements[i];
                if (lazySerializableElement.type == typeof(TComponent))
                {
                    int num2 = flattnenedElements.Length - num - lazySerializableElement.length;
                    byte[] src = flattnenedElements;
                    flattnenedElements = new byte[num + num2];
                    Buffer.BlockCopy(src, 0, flattnenedElements, 0, num);
                    Buffer.BlockCopy(src, num + lazySerializableElement.length, flattnenedElements, flattnenedElements.Length - num, num2);
                    contentElements = contentElements.Where(e => e != lazySerializableElement).ToArray();
                    break;
                }
                num += lazySerializableElement.length;
            }
        }

        public object DeserializeSelf(ObjectReader or)
        {
            s = or.BaseStream;
            contentElements = SerializeManager.DeserializeStream<LazySerializableElement<T>[]>(s);
            startingPos = (int)s.Position;
            long position = FillStartingIndices(s);
            s.Position = position;
            return this;
        }

        public void SerializeSelf(ObjectWriter ow)
        {
            Stream baseStream = ow.BaseStream;
            SerializeManager.SerializeToStream(baseStream, contentElements);
            baseStream.Write(flattnenedElements, 0, flattnenedElements.Length);
        }

        private long FillStartingIndices(Stream s)
        {
            long num = s.Position;
            for (int i = 0; i < contentElements.Length; i++)
            {
                contentElements[i].PrepareLazy(s, num);
                num += contentElements[i].length;
            }
            return num;
        }

        public void FillFlattened()
        {
            s.Position = startingPos;
            flattnenedElements = new byte[contentElements.Sum((LazySerializableElement<T> x) => x.length)];
            s.Read(flattnenedElements, 0, flattnenedElements.Length);
        }

        public override string ToString()
        {
            return Helpers.Helpers.NewLineJoin(contentElements.OrderByDescending((LazySerializableElement<T> x) => x.length));
        }
    }
}