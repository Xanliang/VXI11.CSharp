using org.acplt.oncrpc;
using org.acplt.oncrpc.server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VXI11
{
    public abstract class vxi11_DEVICE_INTR_ServerStub : OncRpcServerStub, OncRpcDispatchable
    {

        public vxi11_DEVICE_INTR_ServerStub():this(0)
        {
        }

        public vxi11_DEVICE_INTR_ServerStub(int port):this(null, port)
        {
        }

        public vxi11_DEVICE_INTR_ServerStub(IPAddress bindAddr, int port)
        {
            info = new OncRpcServerTransportRegistrationInfo[] {
            new OncRpcServerTransportRegistrationInfo(vxi11.DEVICE_INTR, 1),
        };
            transports = new OncRpcServerTransport[] {
            new OncRpcUdpServerTransport(this, bindAddr, port, info, 32768),
            new OncRpcTcpServerTransport(this, bindAddr, port, info, 32768)
        };
        }

        public void dispatchOncRpcCall(OncRpcCallInformation call, int program, int version, int procedure)
        {
            if (version == 1)
            {
                switch (procedure)
                {
                    case 30:
                        {
                            Device_SrqParms args = new Device_SrqParms();
                            call.retrieveCall(args);
                            device_intr_srq_1(args);
                            call.reply(XdrVoid.XDR_VOID);
                            break;
                        }
                    default:
                        call.failProcedureUnavailable();
                        break;
                }
            }
            else
            {
                call.failProgramUnavailable();
            }
        }

        public abstract void device_intr_srq_1(Device_SrqParms arg1);

    }
}
