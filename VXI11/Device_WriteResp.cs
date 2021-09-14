using org.acplt.oncrpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI11
{
    public class Device_WriteResp : XdrAble
    {
        public Device_ErrorCode error;
        public int size;

        public Device_WriteResp()
        {
        }

        public Device_WriteResp(XdrDecodingStream xdr)
        {
            xdrDecode(xdr);
        }

        public void xdrEncode(XdrEncodingStream xdr)
        {
            error.xdrEncode(xdr);
            xdr.xdrEncodeInt(size);
        }

        public void xdrDecode(XdrDecodingStream xdr)
        {
            error = new Device_ErrorCode(xdr);
            size = xdr.xdrDecodeInt();
        }

    }
}
