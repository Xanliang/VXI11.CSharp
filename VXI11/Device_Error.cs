using org.acplt.oncrpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI11
{
    public class Device_Error : XdrAble
    {
        public Device_ErrorCode error;

        public Device_Error()
        {
        }

        public Device_Error(XdrDecodingStream xdr)
        {
            xdrDecode(xdr);
        }

        public void xdrEncode(XdrEncodingStream xdr)
        {
            error.xdrEncode(xdr);
        }

        public void xdrDecode(XdrDecodingStream xdr)
        {
            error = new Device_ErrorCode(xdr);
        }
    }
}
