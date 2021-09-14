using org.acplt.oncrpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI11
{
    public class Device_DocmdParms : XdrAble
    {
        public Device_Link lid;
        public Device_Flags flags;
        public int io_timeout;
        public int lock_timeout;
        public int cmd;
        public bool network_order;
        public int datasize;
        public byte[] data_in;

        public Device_DocmdParms()
        {
        }

        public Device_DocmdParms(XdrDecodingStream xdr)
        {
            xdrDecode(xdr);
        }

        public void xdrEncode(XdrEncodingStream xdr)
        {
            lid.xdrEncode(xdr);
            flags.xdrEncode(xdr);
            xdr.xdrEncodeInt(io_timeout);
            xdr.xdrEncodeInt(lock_timeout);
            xdr.xdrEncodeInt(cmd);
            xdr.xdrEncodeBoolean(network_order);
            xdr.xdrEncodeInt(datasize);
            xdr.xdrEncodeDynamicOpaque(data_in);
        }

        public void xdrDecode(XdrDecodingStream xdr)
        {
            lid = new Device_Link(xdr);
            flags = new Device_Flags(xdr);
            io_timeout = xdr.xdrDecodeInt();
            lock_timeout = xdr.xdrDecodeInt();
            cmd = xdr.xdrDecodeInt();
            network_order = xdr.xdrDecodeBoolean();
            datasize = xdr.xdrDecodeInt();
            data_in = xdr.xdrDecodeDynamicOpaque();
        }

    }
}
