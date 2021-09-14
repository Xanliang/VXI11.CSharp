using org.acplt.oncrpc.server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VXI11
{
    public abstract class vxi11_DEVICE_ASYNC_ServerStub : OncRpcServerStub, OncRpcDispatchable
    {

        public vxi11_DEVICE_ASYNC_ServerStub():this(0)
        {
        }

        public vxi11_DEVICE_ASYNC_ServerStub(int port):this(null, port)
        {
        }

        public vxi11_DEVICE_ASYNC_ServerStub(IPAddress bindAddr, int port)
        {
            info = new OncRpcServerTransportRegistrationInfo[] {
            new OncRpcServerTransportRegistrationInfo(vxi11.DEVICE_ASYNC, 1),
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
                    case 1:
                        {
                            Device_Link args = new Device_Link();
                            call.retrieveCall(args);
                            Device_Error result = device_abort_1(args);
                            call.reply(result);
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

        public abstract Device_Error device_abort_1(Device_Link arg1);

    }
}
