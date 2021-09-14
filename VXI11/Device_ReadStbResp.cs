using org.acplt.oncrpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI11
{
    public class Device_ReadStbResp : XdrAble
    {
        public Device_ErrorCode error;
        public byte stb;

        public Device_ReadStbResp()
        {
        }

        public Device_ReadStbResp(XdrDecodingStream xdr)
        {
            xdrDecode(xdr);
        }

        public void xdrEncode(XdrEncodingStream xdr)
        {
            error.xdrEncode(xdr);
            xdr.xdrEncodeByte(stb);
        }

        public void xdrDecode(XdrDecodingStream xdr)
        {
            error = new Device_ErrorCode(xdr);
            stb = xdr.xdrDecodeByte();
        }

    }
}
