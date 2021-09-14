using org.acplt.oncrpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI11
{
    public class Device_RemoteFunc : XdrAble
    {
        public int hostAddr;
        public int hostPort;
        public int progNum;
        public int progVers;
        public int progFamily;

        public Device_RemoteFunc()
        {
        }

        public Device_RemoteFunc(XdrDecodingStream xdr)
        {
            xdrDecode(xdr);
        }

        public void xdrEncode(XdrEncodingStream xdr)
        {
            xdr.xdrEncodeInt(hostAddr);
            xdr.xdrEncodeInt(hostPort);
            xdr.xdrEncodeInt(progNum);
            xdr.xdrEncodeInt(progVers);
            xdr.xdrEncodeInt(progFamily);
        }

        public void xdrDecode(XdrDecodingStream xdr)
        {
            hostAddr = xdr.xdrDecodeInt();
            hostPort = xdr.xdrDecodeInt();
            progNum = xdr.xdrDecodeInt();
            progVers = xdr.xdrDecodeInt();
            progFamily = xdr.xdrDecodeInt();
        }

    }
}
