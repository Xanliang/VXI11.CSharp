using org.acplt.oncrpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI11
{
    public class Device_GenericParms : XdrAble
    {
        public Device_Link lid;
        public Device_Flags flags;
        public int lock_timeout;
        public int io_timeout;

        public Device_GenericParms()
        {
        }

        public Device_GenericParms(XdrDecodingStream xdr)
        {
            xdrDecode(xdr);
        }

        public void xdrEncode(XdrEncodingStream xdr)
        {
            lid.xdrEncode(xdr);
            flags.xdrEncode(xdr);
            xdr.xdrEncodeInt(lock_timeout);
            xdr.xdrEncodeInt(io_timeout);
        }

        public void xdrDecode(XdrDecodingStream xdr)
        {
            lid = new Device_Link(xdr);
            flags = new Device_Flags(xdr);
            lock_timeout = xdr.xdrDecodeInt();
            io_timeout = xdr.xdrDecodeInt();
        }

    }
}
