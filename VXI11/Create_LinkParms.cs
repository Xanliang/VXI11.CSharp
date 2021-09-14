using org.acplt.oncrpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI11
{
    public class Create_LinkParms : XdrAble
    {
        public int clientId;
        public bool lockDevice;
        public int lock_timeout;
        public String device;

        public Create_LinkParms()
        {
        }

        public Create_LinkParms(XdrDecodingStream xdr)
        {
            xdrDecode(xdr);
        }

        public void xdrEncode(XdrEncodingStream xdr)
        {
            xdr.xdrEncodeInt(clientId);
            xdr.xdrEncodeBoolean(lockDevice);
            xdr.xdrEncodeInt(lock_timeout);
            xdr.xdrEncodeString(device);
        }

        public void xdrDecode(XdrDecodingStream xdr)
        {
            clientId = xdr.xdrDecodeInt();
            lockDevice = xdr.xdrDecodeBoolean();
            lock_timeout = xdr.xdrDecodeInt();
            device = xdr.xdrDecodeString();
        }

    }
}
