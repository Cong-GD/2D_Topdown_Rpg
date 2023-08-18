using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CongTDev.IOSystem
{
    public interface ISerializable
    {
        SerializedObject Serialize();
    }
}
