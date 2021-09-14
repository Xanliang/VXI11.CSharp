using org.acplt.oncrpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI11
{
    public class Device_Link : XdrAble
    {
        public int value;

        public Device_Link()
        {
        }

        public Device_Link(int value)
        {
            this.value = value;
        }

        public Device_Link(XdrDecodingStream xdr)
        {
            xdrDecode(xdr);
        }

        public void xdrEncode(XdrEncodingStream xdr)
        {
            xdr.xdrEncodeInt(value);
        }

        public void xdrDecode(XdrDecodingStream xdr)
        {
            value = xdr.xdrDecodeInt();
        }
    }
}
