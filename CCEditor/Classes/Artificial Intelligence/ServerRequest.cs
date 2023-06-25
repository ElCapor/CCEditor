using CCEditor.CC.Attributes;
using CCEditor.Classes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.Classes.Artificial_Intelligence
{
    public class ServerRequest
    {
        public enum RequestContentType
        {
            liveMidiRecording,
            aiNotes,
            audioSamples,
            audioFile,
            webLink
        }
        [SerializeByID(0, false)]
        public object mainObj;

        [SerializeByID(1, false)]
        public sbyte transpose;

        [SerializeByID(2, false)]
        public RequestContentType contentType;

        [SerializeByID(3, false)]
        public string fileName;

        [SerializeByID(4, false)]
        public OffOnAuto handClassification = OffOnAuto.auto;

        [SerializeByID(5, false)]
        public OffOnAuto fingerClassification = OffOnAuto.on;

        [SerializeByID(6, false)]
        public OffOnAuto humanization = OffOnAuto.auto;

        [SerializeByID(7, false)]
        public bool trimSilence = true;

        [SerializeByID(8, false)]
        public bool unlimitedLength;
    }
}
