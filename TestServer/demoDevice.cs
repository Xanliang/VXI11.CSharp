using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestServer
{
    class demoDevice : IBaseDevice
    {

        /// <summary>
        /// 线程同步锁
        /// </summary>
        public ManualResetEvent AsyncLocker = null;

        /// <summary>
        /// 设备ID标识字符串
        /// </summary>
        public const string CV_IDN = "PhaseLock Technology,EXXX0A";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="locker"></param>
        public demoDevice(ManualResetEvent locker)
        {
            this.AsyncLocker = locker;
        }


        #region IBaseDevice 接口

        /// <summary>
        /// 清除状态 *CLS
        /// </summary>
        /// <returns></returns>
        [SCPIAttrubite("*CLS", ESCPIOPerType.Write)]
        public bool CLS()
        {
            this.AsyncLocker.Set();//信号量设置
            return true;
        }

        /// <summary>
        /// 复位设备 *RST
        /// </summary>
        [SCPIAttrubite("*RST", ESCPIOPerType.Write)]
        public bool RST()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 标准事件状态 使能 *ESE
        /// </summary>
        [SCPIAttrubite("*ESE", ESCPIOPerType.Write)]
        public bool ESE()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 标准事件状态 使能查询  *ESE?
        /// </summary>
        [SCPIAttrubite("*ESE?", ESCPIOPerType.Read)]
        public string ESERead()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 操作完成指令 *OPC
        /// </summary>
        [SCPIAttrubite("*OPC", ESCPIOPerType.Write)]
        public bool OPC()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 操作完成查询 *OPC?
        /// </summary>
        [SCPIAttrubite("*OPC?", ESCPIOPerType.Read)]
        public string OPCRead()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 识别查询 *IDN?
        /// </summary>
        [SCPIAttrubite("*IDN?", ESCPIOPerType.Read)]
        public string IDNRead()
        {
            return CV_IDN;
        }

        /// <summary>
        /// 服务请示使能指令 *SRE
        /// </summary>
        [SCPIAttrubite("*SRE", ESCPIOPerType.Write)]
        public bool SRE()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 服务请示使能查询 *SRE?
        /// </summary>
        [SCPIAttrubite("*SRE?", ESCPIOPerType.Read)]
        public string SRERead()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 读取状态 字节查询 *STB?
        /// </summary>
        [SCPIAttrubite("*STB?", ESCPIOPerType.Read)]
        public string STBRead()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 自测试查询 *TST?
        /// </summary>
        [SCPIAttrubite("*TST?", ESCPIOPerType.Read)]
        public string TSTRead()
        {
            throw new NotImplementedException();
        }

        #endregion


        #region 自定义指令集合

        [SCPIAttrubite(":FREQ?", ESCPIOPerType.Read)]
        public double ReadFreq()
        {
            return 1000.000;
        }

        #endregion
    }
}
