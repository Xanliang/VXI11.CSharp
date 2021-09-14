using org.acplt.oncrpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI11
{
    public class Device_ReadResp : XdrAble
    {
        public Device_ErrorCode error;
        public int reason;
        public byte[] data;

        public Device_ReadResp()
        {
        }

        public Device_ReadResp(XdrDecodingStream xdr)
        {
            xdrDecode(xdr);
        }

        public void xdrEncode(XdrEncodingStream xdr)
        {
            error.xdrEncode(xdr);
            xdr.xdrEncodeInt(reason);
            xdr.xdrEncodeDynamicOpaque(data);
        }

        public void xdrDecode(XdrDecodingStream xdr)
        {
            error = new Device_ErrorCode(xdr);
            reason = xdr.xdrDecodeInt();
            data = xdr.xdrDecodeDynamicOpaque();
        }

    }
}
