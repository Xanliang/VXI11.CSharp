using org.acplt.oncrpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI11
{
    public class Device_EnableSrqParms : XdrAble
    {
        public Device_Link lid;
        public bool enable;
        public byte[] handle;

        public Device_EnableSrqParms()
        {
        }

        public Device_EnableSrqParms(XdrDecodingStream xdr)
        {
            xdrDecode(xdr);
        }

        public void xdrEncode(XdrEncodingStream xdr)
        {
            lid.xdrEncode(xdr);
            xdr.xdrEncodeBoolean(enable);
            xdr.xdrEncodeDynamicOpaque(handle);
        }

        public void xdrDecode(XdrDecodingStream xdr)
        {
            lid = new Device_Link(xdr);
            enable = xdr.xdrDecodeBoolean();
            handle = xdr.xdrDecodeDynamicOpaque();
        }

    }
}
