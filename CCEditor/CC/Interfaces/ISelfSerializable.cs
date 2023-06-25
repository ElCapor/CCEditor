using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.CC.Interfaces
{
    public interface ISelfSerializable
    {
        void SerializeSelf(ObjectWriter s);

        object DeserializeSelf(ObjectReader s);
    }
}
