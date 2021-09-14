/*
 * Automatically generated by jrpcgen 1.0.7 on 2021/9/14
 * jrpcgen is part of the "Remote Tea.Net" ONC/RPC package for C#
 * See http://remotetea.sourceforge.net for details
 */
namespace tests.org.acplt.oncrpc.jrpcgen{
using global::org.acplt.oncrpc;

public class SOMERESULT : XdrAble {
    public int error;
    public string typedesc;
    public byte [] data;

    public SOMERESULT() {
    }

    public SOMERESULT(XdrDecodingStream xdr) {
        xdrDecode(xdr);
    }

    public void xdrEncode(XdrEncodingStream xdr) {
        xdr.xdrEncodeInt(error);
        xdr.xdrEncodeString(typedesc);
        xdr.xdrEncodeDynamicOpaque(data);
    }

    public void xdrDecode(XdrDecodingStream xdr) {
        error = xdr.xdrDecodeInt();
        typedesc = xdr.xdrDecodeString();
        data = xdr.xdrDecodeDynamicOpaque();
    }

}
} //End of Namespace tests.org.acplt.oncrpc.jrpcgen
// End of SOMERESULT.cs
