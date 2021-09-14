using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestServer
{

    public enum ESCPIOPerType
    {
        None = 0,
        Write,
        Read
    }

    /// <summary>
    /// SCPI指令标记属性
    /// </summary>
    public class SCPIAttrubite : Attribute
    {
        private string m_SCPIContent = "";

        /// <summary>
        /// SCPI命令内容 可标记全称
        /// </summary>
        public string SCPIContent
        {
            get { return this.m_SCPIContent; }
        }

        private ESCPIOPerType m_SCPIOperType = ESCPIOPerType.None;

        /// <summary>
        /// 指令读写类型
        /// </summary>
        public ESCPIOPerType SCPIOperType
        {
            get { return this.m_SCPIOperType; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sCPIContent">scpi指令内容</param>
        /// <param name="sCPIOperType">scpi 读写操作</param>
        public SCPIAttrubite(string sCPIContent, ESCPIOPerType sCPIOperType)
        {
            this.m_SCPIContent = sCPIContent;
            this.m_SCPIOperType = sCPIOperType;
        }
    }

    public interface IBaseDevice
    {

        /// <summary>
        /// 清除状态 *CLS
        /// </summary>
        /// <returns></returns>
        bool CLS();



        /// <summary>
        /// 复位设备 *RST
        /// </summary>
        bool RST();


        /// <summary>
        /// 标准事件状态 使能 *ESE
        /// </summary>
        bool ESE();


        /// <summary>
        /// 标准事件状态 使能查询  *ESE?
        /// </summary>
        string ESERead();


        /// <summary>
        /// 操作完成指令 *OPC
        /// </summary>
        bool OPC();

        /// <summary>
        /// 操作完成查询 *OPC?
        /// </summary>
        string OPCRead();

        /// <summary>
        /// 识别查询 *IDN?
        /// </summary>
        string IDNRead();


        /// <summary>
        /// 服务请示使能指令 *SER
        /// </summary>
        bool SRE();

        /// <summary>
        /// 服务请示使能查询 *SER?
        /// </summary>
        string SRERead();




        /// <summary>
        /// 读取状态 字节查询 *STB?
        /// </summary>
        string STBRead();


        /// <summary>
        /// 自测试查询 *TST?
        /// </summary>
        string TSTRead();

    }
}
