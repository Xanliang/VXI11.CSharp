using org.acplt.oncrpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VXI11;

namespace TestServer
{
    public class Vxi11Server : vxi11_DEVICE_CORE_ServerStub
    {
        /// <summary>
        /// 当前设备
        /// </summary>
        IBaseDevice Device =null;

        /// <summary>
        /// 编码规则 默认ASCLL
        /// </summary>
        public Encoding CV_Encoding = Encoding.ASCII;


        /// <summary>
        /// 线程同步锁
        /// </summary>
        public ManualResetEvent AsyncLocker = new ManualResetEvent(false);

        /// <summary>
        /// 读取缓存buff
        /// </summary>
        public byte[] ReadBuff = new byte[0];

        /// <summary>
        /// 超时等待时间ms
        /// </summary>
        public int WaitOnOutTime = 1000;

        /// <summary>
        /// 当前操作指令类型
        /// </summary>
        public ESCPIOPerType CurentOperType = ESCPIOPerType.None;


        public Vxi11Server() : base(0)
        {
            InitialDevice();
        }

        public Vxi11Server(int port) : base(null, port)
        {
            InitialDevice();
        }

        public Vxi11Server(IPAddress bindAddr, int port) : base(bindAddr, port)
        {
            InitialDevice();
        }

        /// <summary>
        /// 设备初始化 
        /// </summary>
        public void InitialDevice()
        {
            Device = new demoDevice(AsyncLocker);
        }

        int lidID = 0;

        /// <summary>
        /// 创建一个设备连接
        /// </summary>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public override Create_LinkResp create_link_1(Create_LinkParms arg1)
        {
            Create_LinkResp result = new Create_LinkResp();
            lidID++;
            result.lid = new Device_Link()
            { value = lidID };

            if (arg1.device == "inst0")
            {
                result.error = new Device_ErrorCode() { value = Visa32.VISA.VI_SUCCESS };
            }
            else
            {
                result.error = new Device_ErrorCode() { value = Visa32.VISA.VI_ERROR_SYSTEM_ERROR };
            }
            return result;
        }

        /// <summary>
        /// 销毁一个连接 
        /// </summary>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public override Device_Error destroy_link_1(Device_Link arg1)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 创建一个 中断通道 
        /// </summary>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public override Device_Error create_intr_chan_1(Device_RemoteFunc arg1)
        {
            Device_Error result = new Device_Error();
            result.error = new Device_ErrorCode(0);
            return result;
        }

        /// <summary>
        /// 销毁 一个中断 通道
        /// </summary>
        /// <returns></returns>
        public override Device_Error destroy_intr_chan_1()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设备清除
        /// </summary>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public override Device_Error device_clear_1(Device_GenericParms arg1)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设备执行一个命令
        /// </summary>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public override Device_DocmdResp device_docmd_1(Device_DocmdParms arg1)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设备使能或不使能发送请求服务 
        /// </summary>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public override Device_Error device_enable_srq_1(Device_EnableSrqParms arg1)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设备本地控制使能
        /// </summary>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public override Device_Error device_local_1(Device_GenericParms arg1)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设备远程控制使能 
        /// </summary>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public override Device_Error device_remote_1(Device_GenericParms arg1)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回设备状态byte
        /// </summary>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public override Device_ReadStbResp device_readstb_1(Device_GenericParms arg1)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// 设备执行一个触发
        /// </summary>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public override Device_Error device_trigger_1(Device_GenericParms arg1)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设备锁定
        /// </summary>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public override Device_Error device_lock_1(Device_LockParms arg1)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设备解锁
        /// </summary>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public override Device_Error device_unlock_1(Device_Link arg1)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 读取指令
        /// </summary>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public override Device_ReadResp device_read_1(Device_ReadParms arg1)
        {
            Device_ReadResp readRes = new Device_ReadResp();
            if (CurentOperType == ESCPIOPerType.None || CurentOperType == ESCPIOPerType.Write)
            {
                ReadBuff = new byte[0];
                AsyncLocker.Reset();
            }
            if (!AsyncLocker.WaitOne(WaitOnOutTime))
            {
                readRes.data = ReadBuff;
                readRes.error = new Device_ErrorCode() { value = Visa32.VISA.VI_ERROR_INV_EXPR };//超时
                readRes.reason = 3;
                return readRes;
            }

            if (CurentOperType == ESCPIOPerType.Read)
            {
                readRes.data = ReadBuff;
                readRes.error = new Device_ErrorCode() { value = Visa32.VISA.VI_SUCCESS };
                readRes.reason = 3;
            }
            CurentOperType = ESCPIOPerType.None;//重置操作类型
            return readRes;
        }

        /// <summary>
        /// 写入指令
        /// </summary>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public override Device_WriteResp device_write_1(Device_WriteParms arg1)
        {
            string cmd = CV_Encoding.GetString(arg1.data);
            WriteLog("lid:" + arg1.lid.value + "->" + "接收指令：" + cmd.Replace("\n", "\\n"));
            Device_WriteResp result = new Device_WriteResp();
            result.error = new Device_ErrorCode(Visa32.VISA.VI_SUCCESS);
            string[] scpiArray = cmd.Split(new char[] { '\n', '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (scpiArray.Length == 0)
            {
                //指令错误或未定义
                result.error = new Device_ErrorCode(Visa32.VISA.VI_ERROR_INV_EXPR);
                return result;
            }

            for (int n = 0; n < scpiArray.Length; n++)
            {
                string spciStr = scpiArray[n];//指令
                WriteLog("执行指令 ：" + spciStr);
                string[] args = new string[0];//参数集合
                string[] tempArray = scpiArray[n].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                spciStr = tempArray[0].Trim();

                this.AsyncLocker.Reset();//阻止线程
                this.ReadBuff = new byte[0];

                if (spciStr[spciStr.Length - 1] == '?')//查询指令
                {
                    CurentOperType = ESCPIOPerType.Read;
                }
                else
                {
                    CurentOperType = ESCPIOPerType.Write;
                }

                if (tempArray.Length >= 2)
                {
                    args = tempArray[1].Split(new char[] { '，' }, StringSplitOptions.RemoveEmptyEntries);
                }

                MethodInfo method = this.Device.GetType().GetMethods().ToList().Find(p =>
               {
                   var att = p.GetCustomAttribute(typeof(SCPIAttrubite));
                   if (att == null) return false;
                   SCPIAttrubite scpiAtt = ((SCPIAttrubite)att);

                   //比较指令 
                   if (scpiAtt.SCPIContent.ToUpper() != spciStr.ToUpper())//指令相同不区分大小写
                   {
                       return false;
                   }
                   else//比较字符串 缩写:POWER?->:Pow? 可省略项？？？？？？
                   { 

                   }
                   return true;
               });

                if (method != null)
                {
                    SCPIAttrubite scpiAtt = (SCPIAttrubite)method.GetCustomAttribute(typeof(SCPIAttrubite));
                    try
                    {
                        object res = null;
                        switch (scpiAtt.SCPIOperType)
                        {
                            case ESCPIOPerType.None:
                                WriteLog("方法特性标记错误：" + method.ToString() + " " + scpiAtt.SCPIOperType + "。", ConsoleColor.Red);
                                break;
                            case ESCPIOPerType.Write://写指令 //解析 参数
                                res = method.Invoke(Device, args);
                                result.error = new Device_ErrorCode(Visa32.VISA.VI_SUCCESS);
                                break;
                            case ESCPIOPerType.Read://查询指令 
                                res = method.Invoke(Device, args);
                                if (res != null)
                                {
                                    ReadBuff = CV_Encoding.GetBytes(res.ToString());
                                    WriteLog("查询结果：" + res.ToString() + "。");
                                }
                                else
                                {
                                    WriteLog("查询结果：" + "NULL" + "。");
                                }
                                result.error = new Device_ErrorCode(Visa32.VISA.VI_SUCCESS);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog("调用方法出错：" + method.ToString() + "\r\n" + ex.Message, ConsoleColor.Red);
                        //参数错误
                        result.error = new Device_ErrorCode(Visa32.VISA.VI_ERROR_INV_EXPR);
                    }
                }
                else
                {
                    WriteLog("未找到方法：" + spciStr, ConsoleColor.Red);
                    result.error = new Device_ErrorCode(Visa32.VISA.VI_ERROR_INV_EXPR);  //指令错误或未定义
                    CurentOperType = ESCPIOPerType.None;
                }
                this.AsyncLocker.Set();//复位阻止
            }


            return result;
        }


        /// <summary>
        /// 显示日志
        /// </summary>
        /// <param name="log"></param>
        public void WriteLog(string log, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(GetNowTimeString() + "##" + log);
            Console.ResetColor();
        }

        /// <summary>
        /// 获取当前时间字符串 yyyy-MM-dd HH:mm:ss.fff
        /// </summary>
        /// <returns>yyyy-MM-dd HH:mm:ss.fff</returns>
        public string GetNowTimeString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
    }
}
