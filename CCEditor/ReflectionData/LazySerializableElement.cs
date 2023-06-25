using CCEditor.CC.Attributes;
using CCEditor.CC.Extensions;
using CCEditor.CC;
using System.IO;
using System;

namespace CCEditor.ReflectionData
{
    public class LazySerializableElement
    {
        [SerializeByID(1, false)]
        public Type type;

        [SerializeByID(2, false)]
        public int length;

        private long startingIndex;

        public Lazy<object> lazyValue;

        private Stream s;

        [SerializeByID(0, true)]
        private long LegacyLength
        {
            set
            {
                length = (int)value;
            }
        }

        private object GetValue()
        {
            s.Position = startingIndex;
            try
            {
                return SerializeManager.DeserializeStream<object>(s);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Concat("Couldn't deserialize piece ", type, "\n", ex));
                return null;
            }
        }

        public void PrepareLazy(Stream s, long startingIndex)
        {
            this.s = s;
            this.startingIndex = startingIndex;
            lazyValue = new Lazy<object>(GetValue);
        }

        public override string ToString()
        {
            return string.Concat(type, ": ", length.WithCommas(), " B (", ((double)length * 1E-06).ToString("N2"), " MB)");
        }
    }
    public class LazySerializableElement<T>
    {
        [SerializeByID(1, false)]
        public Type type;

        [SerializeByID(2, false)]
        public int length;

        private long startingIndex;

        public Lazy<T> lazyValue;

        private Stream s;

        [SerializeByID(0, true)]
        private long LegacyLength
        {
            set
            {
                length = (int)value;
            }
        }

        private T GetValue()
        {
            s.Position = startingIndex;
            try
            {
                return SerializeManager.DeserializeStream<T>(s);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Concat("Couldn't deserialize piece ", type, "\n", ex));
                return default(T);
            }
        }

        public void PrepareLazy(Stream s, long startingIndex)
        {
            this.s = s;
            this.startingIndex = startingIndex;
            lazyValue = new Lazy<T>(GetValue);
        }

        public override string ToString()
        {
            return string.Concat(type, ": ", length.WithCommas(), " B (", ((double)length * 1E-06).ToString("N2"), " MB)");
        }
    }
}