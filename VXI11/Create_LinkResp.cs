using org.acplt.oncrpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI11
{
    public class Create_LinkResp : XdrAble
    {
        public Device_ErrorCode error;
        public Device_Link lid;
        public short abortPort;
        public int maxRecvSize;

        public Create_LinkResp()
        {
        }

        public Create_LinkResp(XdrDecodingStream xdr)
        {
            xdrDecode(xdr);
        }

        public void xdrEncode(XdrEncodingStream xdr)
        {
            error.xdrEncode(xdr);
            lid.xdrEncode(xdr);
            xdr.xdrEncodeShort(abortPort);
            xdr.xdrEncodeInt(maxRecvSize);
        }

        public void xdrDecode(XdrDecodingStream xdr)
        {
            error = new Device_ErrorCode(xdr);
            lid = new Device_Link(xdr);
            abortPort = xdr.xdrDecodeShort();
            maxRecvSize = xdr.xdrDecodeInt();
        }
    }
}
