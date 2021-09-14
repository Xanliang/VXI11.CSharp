/*
 * Automatically generated by jrpcgen 1.0.7 on 2021/9/14
 * jrpcgen is part of the "Remote Tea.Net" ONC/RPC package for C#
 * See http://remotetea.sourceforge.net for details
 */
namespace tests.org.acplt.oncrpc.jrpcgen{
using global::org.acplt.oncrpc;

public class ANSWER : XdrAble {
    public int value;
    public int wrong;
    public int the_answer;
    public int check_hash;

    public ANSWER() {
    }

    public ANSWER(XdrDecodingStream xdr) {
        xdrDecode(xdr);
    }

    public void xdrEncode(XdrEncodingStream xdr) {
        xdr.xdrEncodeInt(value);
        switch ( value ) {
        case 40:
        case 41:
            xdr.xdrEncodeInt(wrong);
            break;
        case 42:
            xdr.xdrEncodeInt(the_answer);
            break;
        default:
            xdr.xdrEncodeInt(check_hash);
            break;
        }
    }

    public void xdrDecode(XdrDecodingStream xdr) {
        value = xdr.xdrDecodeInt();
        switch ( value ) {
        case 40:
        case 41:
            wrong = xdr.xdrDecodeInt();
            break;
        case 42:
            the_answer = xdr.xdrDecodeInt();
            break;
        default:
            check_hash = xdr.xdrDecodeInt();
            break;
        }
    }

}
} //End of Namespace tests.org.acplt.oncrpc.jrpcgen
// End of ANSWER.cs
