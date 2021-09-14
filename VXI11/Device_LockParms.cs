using org.acplt.oncrpc;

namespace VXI11
{
    public class Device_LockParms : XdrAble
    {
        public Device_Link lid;
        public Device_Flags flags;
        public int lock_timeout;

        public Device_LockParms()
        {
        }

        public Device_LockParms(XdrDecodingStream xdr)
        {
            xdrDecode(xdr);
        }

        public void xdrEncode(XdrEncodingStream xdr)
        {
            lid.xdrEncode(xdr);
            flags.xdrEncode(xdr);
            xdr.xdrEncodeInt(lock_timeout);
        }

        public void xdrDecode(XdrDecodingStream xdr)
        {
            lid = new Device_Link(xdr);
            flags = new Device_Flags(xdr);
            lock_timeout = xdr.xdrDecodeInt();
        }

    }
}