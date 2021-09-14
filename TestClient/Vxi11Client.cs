using org.acplt.oncrpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VXI11;

namespace TestClient
{
    public class Vxi11Client
    {
        private vxi11_DEVICE_CORE_Client client;
        private Device_Link link;
        private bool connected;
        //
        private int maxRecvSize;
        private bool eoi = true;
        private byte termChar = 0;

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="ip">设备IP地址</param>
        /// <param name="device">inst0</param>
        public void connect(String ip, String device)
        {
            client = new vxi11_DEVICE_CORE_Client(
                     IPAddress.Parse(ip), OncRpcProtocols.ONCRPC_TCP);
            Create_LinkParms createLinkParam = new Create_LinkParms();
            createLinkParam.device = device;
            Create_LinkResp linkResp = client.create_link_1(createLinkParam);
            link = linkResp.lid;
            maxRecvSize = linkResp.maxRecvSize;
            connected = true;
        }
        public bool isConnected()
        {
            return connected;
        }
        int indexOf(byte[] data, char c)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == c)
                {
                    return i;
                }
            }
            return -1;
        }
        static int getLength(long number)
        {
            int len = 0;
            while (number != 0)
            {
                number /= 10;
                len++;
            }
            return len;
        }

        //public int send(String command, File file, byte[] response)
        //{
        //    int Length = (int)file.Length();
        //    if (Length > 65000000)
        //    {
        //       Console.WriteLine(file + " too long");
        //        return -1;
        //    }
        //    else
        //    {
        //        StringBuilder sb = new StringBuilder(command);
        //        sb.Append(",#");
        //        sb.Append(getLength(Length)).Append(Length);

        //        byte[] cmd = sb.ToString().getBytes();
        //        FileInputStream fis = null;


        //        try
        //        {
        //            fis = new FileInputStream(file);

        //            Device_WriteParms dwp = new Device_WriteParms();
        //            int len = cmd.Length + Length;
        //            int sent = 0;
        //            int count;

        //            dwp.lid = link;
        //            dwp.io_timeout = 10000; // in ms
        //            dwp.lock_timeout = 10000; // in ms

        //            while (len > 0)
        //            {
        //                if ((sent == 0) && (len <= maxRecvSize))
        //                {
        //                    byte[] data = new byte[len];
        //                    Array.Copy(cmd, 0, data, 0, cmd.Length);
        //                    fis.read(data, cmd.Length, Length);

        //                    dwp.data = data;
        //                    len = 0;
        //                }
        //                else
        //                {
        //                    if (len > maxRecvSize)
        //                        count = maxRecvSize;
        //                    else
        //                        count = len;
        //                    byte[] btmp = new byte[count];
        //                    if (sent > 0)
        //                    {
        //                        fis.read(btmp, 0, count);
        //                    }
        //                    else
        //                    {
        //                        Array.Copy(cmd, 0, btmp, 0, cmd.Length);
        //                        fis.read(btmp, cmd.Length, count - cmd.Length);
        //                    }
        //                    sent += count;
        //                    len -= count;
        //                    dwp.data = btmp;
        //                }

        //                if ((len == 0) && eoi)
        //                    dwp.flags = new Device_Flags(0x8);
        //                else
        //                    dwp.flags = new Device_Flags(0);

        //                Device_WriteResp writeResp = client.device_write_1(dwp);
        //                if (writeResp == null || writeResp.error.value != 0)
        //                {
        //                    Console.WriteLine("Write Error Code " + (writeResp == null ? "null" : writeResp.error.value.ToString()));
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.StackTrace);
        //        }
        //        finally
        //        {
        //            if (fis != null)
        //            {
        //                fis.close();
        //            }
        //        }
        //        if (response != null)
        //        {
        //        Array.Copy(cmd, 0, response, 0, cmd.Length);
        //            return cmd.Length;
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public int send(byte[] data, byte[] response)
        {
            int rv = 0;
            if (link != null)
            {
                Device_WriteParms writeParam = new Device_WriteParms();
                writeParam.lid = link;
                writeParam.io_timeout = 10000; // in ms
                writeParam.lock_timeout = 10000; // in ms
                writeParam.flags = new Device_Flags();

                writeParam.data = data;
                Device_WriteResp writeResp = client.device_write_1(writeParam);
                if (writeResp == null || writeResp.error.value != 0)
                {
                    Console.WriteLine("Write Error Code " + (writeResp == null ? "null" : writeResp.error.value.ToString()));
                }
                if (data[data.Length - 1] == '\n' && indexOf(data, '?') > -1)
                {
                    String command = Encoding.Default.GetString(data);
                    Console.WriteLine(command);
                    Device_ReadParms readParam = new Device_ReadParms();
                    readParam.lid = link;
                    readParam.requestSize = response.Length;
                    readParam.io_timeout = 10000;
                    readParam.lock_timeout = 10000;
                    readParam.flags = new Device_Flags();
                    readParam.termChar = termChar;



                    Device_ReadResp readResp = client.device_read_1(readParam);
                    if (readResp == null || readResp.error.value != 0)
                    {
                        Console.WriteLine("Read Error Code " + (readResp == null ? "null" : readResp.error.value.ToString()));


                    }
                    else
                    {
                        if (readResp.data != null)
                        {
                            Array.Copy(readResp.data, 0, response, 0, readResp.data.Length);
                            rv = readResp.data.Length;
                            Console.WriteLine("*IDN?->" + Encoding.Default.GetString(readResp.data));
                        }
                    }

                }
            }
            return rv;
        }
        public int send(String command, byte[] response)
        {
            return send(Encoding.Default.GetBytes(command), response);
        }
        public void close()
        {
            connected = false;
            if (link != null && client != null)
            {
                try
                {
                    client.destroy_link_1(link);
                }
                catch (OncRpcException e)
                {
                    // TODO Auto-generated catch block
                    Console.WriteLine(e.StackTrace);
                }
                catch (IOException e)
                {
                    // TODO Auto-generated catch block
                    Console.WriteLine(e.StackTrace);
                }
                link = null;
            }
            if (client != null)
            {
                try
                {
                    client.close();
                }
                catch (OncRpcException e)
                {
                    Console.WriteLine(e.StackTrace);
                }
                client = null;
            }
        }

    }
}
