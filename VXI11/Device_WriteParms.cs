using org.acplt.oncrpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI11
{
    public class Device_WriteParms : XdrAble
    {
        public Device_Link lid;
        public int io_timeout;
        public int lock_timeout;
        public Device_Flags flags;
        public byte[] data;

        public Device_WriteParms()
        {
        }

        public Device_WriteParms(XdrDecodingStream xdr)
        {
            xdrDecode(xdr);
        }

        public void xdrEncode(XdrEncodingStream xdr)
        {
            lid.xdrEncode(xdr);
            xdr.xdrEncodeInt(io_timeout);
            xdr.xdrEncodeInt(lock_timeout);
            flags.xdrEncode(xdr);
            xdr.xdrEncodeDynamicOpaque(data);
        }

        public void xdrDecode(XdrDecodingStream xdr)
        {
            lid = new Device_Link(xdr);
            io_timeout = xdr.xdrDecodeInt();
            lock_timeout = xdr.xdrDecodeInt();
            flags = new Device_Flags(xdr);
            data = xdr.xdrDecodeDynamicOpaque();
        }

    }
}
