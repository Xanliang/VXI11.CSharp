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
    public abstract class vxi11_DEVICE_CORE_ServerStub : OncRpcServerStub, OncRpcDispatchable
    {

        public vxi11_DEVICE_CORE_ServerStub() : this(0)
        {
        }

        public vxi11_DEVICE_CORE_ServerStub(int port) : this(null, port)
        {
        }

        public vxi11_DEVICE_CORE_ServerStub(IPAddress bindAddr, int port)
        {
            info = new OncRpcServerTransportRegistrationInfo[] {
            new OncRpcServerTransportRegistrationInfo(vxi11.DEVICE_CORE, 1),
        };
            transports = new OncRpcServerTransport[] {
            //new OncRpcUdpServerTransport(this, bindAddr, port+2, info, 32768),
            new OncRpcTcpServerTransport(this, bindAddr, port, info, 32768)
        };
        }

        public void dispatchOncRpcCall(OncRpcCallInformation call, int program, int version, int procedure)
        {
            if (version == 1)
            {
                switch (procedure)
                {
                    case 10:
                        {
                            Create_LinkParms args = new Create_LinkParms();
                            call.retrieveCall(args);
                            Create_LinkResp result = create_link_1(args);
                            call.reply(result);
                            break;
                        }
                    case 11:
                        {
                            Device_WriteParms args = new Device_WriteParms();
                            call.retrieveCall(args);
                            Device_WriteResp result = device_write_1(args);
                            call.reply(result);
                            break;
                        }
                    case 12:
                        {
                            Device_ReadParms args = new Device_ReadParms();
                            call.retrieveCall(args);
                            Device_ReadResp result = device_read_1(args);
                            call.reply(result);
                            break;
                        }
                    case 13:
                        {
                            Device_GenericParms args = new Device_GenericParms();
                            call.retrieveCall(args);
                            Device_ReadStbResp result = device_readstb_1(args);
                            call.reply(result);
                            break;
                        }
                    case 14:
                        {
                            Device_GenericParms args = new Device_GenericParms();
                            call.retrieveCall(args);
                            Device_Error result = device_trigger_1(args);
                            call.reply(result);
                            break;
                        }
                    case 15:
                        {
                            Device_GenericParms args = new Device_GenericParms();
                            call.retrieveCall(args);
                            Device_Error result = device_clear_1(args);
                            call.reply(result);
                            break;
                        }
                    case 16:
                        {
                            Device_GenericParms args = new Device_GenericParms();
                            call.retrieveCall(args);
                            Device_Error result = device_remote_1(args);
                            call.reply(result);
                            break;
                        }
                    case 17:
                        {
                            Device_GenericParms args = new Device_GenericParms();
                            call.retrieveCall(args);
                            Device_Error result = device_local_1(args);
                            call.reply(result);
                            break;
                        }
                    case 18:
                        {
                            Device_LockParms args = new Device_LockParms();
                            call.retrieveCall(args);
                            Device_Error result = device_lock_1(args);
                            call.reply(result);
                            break;
                        }
                    case 19:
                        {
                            Device_Link args = new Device_Link();
                            call.retrieveCall(args);
                            Device_Error result = device_unlock_1(args);
                            call.reply(result);
                            break;
                        }
                    case 20:
                        {
                            Device_EnableSrqParms args = new Device_EnableSrqParms();
                            call.retrieveCall(args);
                            Device_Error result = device_enable_srq_1(args);
                            call.reply(result);
                            break;
                        }
                    case 22:
                        {
                            Device_DocmdParms args = new Device_DocmdParms();
                            call.retrieveCall(args);
                            Device_DocmdResp result = device_docmd_1(args);
                            call.reply(result);
                            break;
                        }
                    case 23:
                        {
                            Device_Link args = new Device_Link();
                            call.retrieveCall(args);
                            Device_Error result = destroy_link_1(args);
                            call.reply(result);
                            break;
                        }
                    case 25:
                        {
                            Device_RemoteFunc args = new Device_RemoteFunc();
                            call.retrieveCall(args);
                            Device_Error result = create_intr_chan_1(args);
                            call.reply(result);
                            break;
                        }
                    case 26:
                        {
                            call.retrieveCall(XdrVoid.XDR_VOID);
                            Device_Error result = destroy_intr_chan_1();
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

        public abstract Create_LinkResp create_link_1(Create_LinkParms arg1);

        public abstract Device_WriteResp device_write_1(Device_WriteParms arg1);

        public abstract Device_ReadResp device_read_1(Device_ReadParms arg1);

        public abstract Device_ReadStbResp device_readstb_1(Device_GenericParms arg1);

        public abstract Device_Error device_trigger_1(Device_GenericParms arg1);

        public abstract Device_Error device_clear_1(Device_GenericParms arg1);

        public abstract Device_Error device_remote_1(Device_GenericParms arg1);

        public abstract Device_Error device_local_1(Device_GenericParms arg1);

        public abstract Device_Error device_lock_1(Device_LockParms arg1);

        public abstract Device_Error device_unlock_1(Device_Link arg1);

        public abstract Device_Error device_enable_srq_1(Device_EnableSrqParms arg1);

        public abstract Device_DocmdResp device_docmd_1(Device_DocmdParms arg1);

        public abstract Device_Error destroy_link_1(Device_Link arg1);

        public abstract Device_Error create_intr_chan_1(Device_RemoteFunc arg1);

        public abstract Device_Error destroy_intr_chan_1();

    }
}
