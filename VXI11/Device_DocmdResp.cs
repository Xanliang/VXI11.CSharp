using org.acplt.oncrpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI11
{
    public class Device_DocmdResp : XdrAble
    {
        public Device_ErrorCode error;
        public byte[] data_out;

        public Device_DocmdResp()
        {
        }

        public Device_DocmdResp(XdrDecodingStream xdr)
        {
            xdrDecode(xdr);
        }

        public void xdrEncode(XdrEncodingStream xdr)
        {
            error.xdrEncode(xdr);
            xdr.xdrEncodeDynamicOpaque(data_out);
        }

        public void xdrDecode(XdrDecodingStream xdr)
        {
            error = new Device_ErrorCode(xdr);
            data_out = xdr.xdrDecodeDynamicOpaque();
        }

    }
}
