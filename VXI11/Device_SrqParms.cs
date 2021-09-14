using org.acplt.oncrpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI11
{
    public class Device_SrqParms : XdrAble
    {
        public byte[] handle;

        public Device_SrqParms()
        {
        }

        public Device_SrqParms(XdrDecodingStream xdr)
        {
            xdrDecode(xdr);
        }

        public void xdrEncode(XdrEncodingStream xdr)
        {
            xdr.xdrEncodeDynamicOpaque(handle);
        }

        public void xdrDecode(XdrDecodingStream xdr)
        {
            handle = xdr.xdrDecodeDynamicOpaque();
        }

    }
}
