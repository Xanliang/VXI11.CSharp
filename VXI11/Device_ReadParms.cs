using org.acplt.oncrpc;

namespace VXI11
{
    public class Device_ReadParms : XdrAble
    {
        public Device_Link lid;
        public int requestSize;
        public int io_timeout;
        public int lock_timeout;
        public Device_Flags flags;
        public byte termChar;

        public Device_ReadParms()
        {
        }

        public Device_ReadParms(XdrDecodingStream xdr)
        {
            xdrDecode(xdr);
        }

        public void xdrEncode(XdrEncodingStream xdr)
        {
            lid.xdrEncode(xdr);
            xdr.xdrEncodeInt(requestSize);
            xdr.xdrEncodeInt(io_timeout);
            xdr.xdrEncodeInt(lock_timeout);
            flags.xdrEncode(xdr);
            xdr.xdrEncodeByte(termChar);
        }

        public void xdrDecode(XdrDecodingStream xdr)
        {
            lid = new Device_Link(xdr);
            requestSize = xdr.xdrDecodeInt();
            io_timeout = xdr.xdrDecodeInt();
            lock_timeout = xdr.xdrDecodeInt();
            flags = new Device_Flags(xdr);
            termChar = xdr.xdrDecodeByte();
        }

    }
}