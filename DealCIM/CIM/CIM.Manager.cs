using BasicClass;
using DealFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DealCIM
{
    public partial class CIM
    {
        #region config
        //配置文件section
        public const string section = "CONFIG";
        //配置文件路径
        public static string Path_Config = ParPathRoot.PathRoot + "Store\\Custom\\" + "Cim.ini";
        #endregion

        #region log path
        /// <summary>
        /// 日志根目录
        /// </summary>
        public const string logFolder = "软件运行记录\\Custom\\CIM\\";
        /// <summary>
        /// trackout日志目录
        /// </summary>
        public static string Path_Log_TrackOut { get => ParPathRoot.PathRoot + logFolder + "TrackOut\\"; }
        /// <summary>
        /// chipid日志目录
        /// </summary>
        public static string Path_Log_ChipID { get => ParPathRoot.PathRoot + logFolder + "ChipID\\"; }
        /// <summary>
        /// lotno日志目录
        /// </summary>
        public static string Path_Log_Lot { get => ParPathRoot.PathRoot + logFolder + "Lot\\"; }

        public static string Path_Log_Response { get => ParPathRoot.PathRoot + logFolder + "Response\\"; }
        #endregion

        #region path xml
        /// <summary>
        /// xml文档根目录，保存的是标准xml文件
        /// </summary>
        private const string xmlFolder = "Store\\Custom\\XML\\";
        /// <summary>
        /// trackout.xml目录
        /// </summary>
        public static string Path_XML_TrackOut { get => ParPathRoot.PathRoot + xmlFolder + "track_out.xml"; }
        /// <summary>
        /// chipid.xml目录
        /// </summary>
        public static string Path_XML_ChipID { get => ParPathRoot.PathRoot + xmlFolder + "chipid.xml"; }
        /// <summary>
        /// lot.xml目录
        /// </summary>
        public static string Path_XML_Lot { get => ParPathRoot.PathRoot + xmlFolder + "lot.xml"; }
        #endregion

        #region params
        /// <summary>
        /// 根节点内容
        /// </summary>
        public const string strRoot = @"si300_interface";
        /// <summary>
        /// 用户id
        /// </summary>
        public static string StrUserID { get; set; }
        /// <summary>
        /// fab号
        /// </summary>
        public static string StrFab { get; set; }
        /// <summary>
        /// 工作区域
        /// </summary>
        public static string StrArea { get; set; }
        /// <summary>
        /// 线体
        /// </summary>
        public static string StrLine { get; set; }
        /// <summary>
        /// 工作站
        /// </summary>
        public static string StrOperation { get; set; }
        /// <summary>
        /// lot号
        /// </summary>
        public static string StrLot { get; set; }
        /// <summary>
        /// 发送数据队列号
        /// </summary>
        public static string StrSendQueue { get; set; } = string.Empty;
        /// <summary>
        /// 接收数据队列号
        /// </summary>
        public static string StrReadQueue { get; set; } = string.Empty;
        /// <summary>
        /// cim连接的ip地址
        /// </summary>
        public static string StrIP { get; set; } = string.Empty;
        /// <summary>
        /// cim连接的端口号
        /// </summary>
        public static string StrPort { get; set; }

        /// <summary>
        /// 从配置文件读取保存在本地的CIM连接配置
        /// </summary>
        public static void InitParams()
        {
            try
            {
                StrSendQueue = GetCimConfig(CIM_PARAMS.SendQueue.ToString());
                StrReadQueue = GetCimConfig(CIM_PARAMS.ReadQueue.ToString());
                StrIP = GetCimConfig(CIM_PARAMS.IP.ToString());
                StrPort = GetCimConfig(CIM_PARAMS.Port.ToString());
                StrUserID = GetCimConfig(CIM_PARAMS.UserID.ToString());
                StrFab = GetCimConfig(CIM_PARAMS.Fab.ToString());
                StrArea = GetCimConfig(CIM_PARAMS.Area.ToString());
                StrLine = GetCimConfig(CIM_PARAMS.Line.ToString());
                StrOperation = GetCimConfig(CIM_PARAMS.Operation.ToString());
                StrLot = GetCimConfig(CIM_PARAMS.RunCard.ToString());
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
        }

        /// <summary>
        /// 封装个接口，少写几个变量，方便统一改动
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetCimConfig(string key)
        {
            try
            {
                return IniFile.I_I.ReadIniStr(section, key, Path_Config);
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                return string.Empty;
            }
        }

        public static void WriteCimConfig()
        {
            WriteCimConfig(CIM_PARAMS.SendQueue.ToString(), StrSendQueue);
            WriteCimConfig(CIM_PARAMS.ReadQueue.ToString(), StrReadQueue);
            WriteCimConfig(CIM_PARAMS.IP.ToString(), StrIP);
            WriteCimConfig(CIM_PARAMS.Port.ToString(), StrPort);
            WriteCimConfig(CIM_PARAMS.UserID.ToString(), StrUserID);
            WriteCimConfig(CIM_PARAMS.Fab.ToString(), StrFab);
            WriteCimConfig(CIM_PARAMS.Area.ToString(), StrArea);
            WriteCimConfig(CIM_PARAMS.Line.ToString(), StrLine);
            WriteCimConfig(CIM_PARAMS.Operation.ToString(), StrOperation);
            WriteCimConfig(CIM_PARAMS.RunCard.ToString(), StrLot);
        }

        /// <summary>
        /// 将CIM参数记录到本地
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private static void WriteCimConfig(string key, string value)
        {
            try
            {
                IniFile.I_I.WriteIni(section, key, value, Path_Config);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
        }

        /// <summary>
        /// 配置xml中的数据，为了普适，其实准备好对应的xml，只要写一个modelno即可，其余对于一台机而言可以写死，但耗费较少，不做特殊处理，并且写法冗长，有志之士可以用dic<节点名,数据>来进行管理，一个foreach+dic.trygetvalue就可以少几十行代码
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="modelno"></param>
        public static void AssignXML(ref XmlDocument xmlDoc, string modelno)
        {
            try
            {
                XmlNodeList nodeList = xmlDoc.SelectSingleNode(strRoot).ChildNodes;
                foreach (XmlNode xn in nodeList)
                {
                    XmlElement xe = (XmlElement)xn; //将子节点类型转换为XmlElement类型 

                    if (xe.Name == "user_id")
                    {
                        xe.InnerText = StrUserID;
                    }
                    if (xe.Name == "p_area")
                    {
                        xe.InnerText = StrFab;
                    }
                    if (xe.Name == "area")
                    {
                        xe.InnerText = StrArea;
                    }
                    if (xe.Name == "line")
                    {
                        xe.InnerText = StrLine;
                    }
                    if (xe.Name == "operation")
                    {
                        xe.InnerText = StrOperation;
                    }
                    if (xe.Name == "lot_no")
                    {
                        xe.InnerText = StrLot;
                    }
                    if (xe.Name == "model_no")
                    {
                        xe.InnerText = modelno;
                    }
                }
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
        }
        #endregion

        #region returnValue
        //保护dic的读写
        static object dic_lock = new object();
        /// <summary>
        /// 保存cim反馈结果的dic，<correlationid,result>
        /// </summary>
        static Dictionary<string, bool> dic_returnValue = new Dictionary<string, bool>();

        /// <summary>
        /// 查询Dic中是否有对应的结果，如果有则返回结果并清除记录
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>查询结果-是否存在所查询的内容</returns>
        public static bool CheckDic(string key, out bool value)
        {
            value = false;
            lock (dic_lock)
            {
                if (dic_returnValue.ContainsKey(key))
                {
                    value = dic_returnValue[key];
                    dic_returnValue.Remove(key);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 在dic中添加新获取的cim反馈结果
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool AddDic(string key, bool value)
        {
            lock (dic_lock)
            {
                try
                {
                    dic_returnValue.Add(key, value);
                    return true;
                }
                catch (Exception ex)
                {
                    Log.L_I.WriteError(ClassName, ex);
                    return false;
                }
            }
        }
        #endregion

        #region chipid list
        /// <summary>
        /// chipidlist的保存路径
        /// </summary>
        public static string Path_ChipIDList { get => ParPathRoot.PathRoot + logFolder + " ChipID_List.txt\\"; }
        /// <summary>
        /// 用于保存插栏成功的chipid
        /// </summary>
        public static List<string> chipid_list = new List<string>();

        /// <summary>
        /// 软件开启时，从本地读取之前的记录
        /// </summary>
        /// <param name="cnt">可以设定读取前多少个，感觉没什么用，反正加了以防万一，不加个参数感觉太死板</param>
        public static void LoadList(int cnt)
        {
            try
            {
                FileStream fileStream = new FileStream(Path_ChipIDList, FileMode.Open, FileAccess.Read);
                TextReader textReader = new StreamReader(fileStream, Encoding.Default);
                for (int i = 0; i < cnt; ++i)
                {
                    //一次一行，一行一个chipid
                    chipid_list.Add(textReader.ReadLine());
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
        }

        /// <summary>
        /// 添加新数据，并记录到本地，按理说要trycatch，但没必要
        /// </summary>
        /// <param name="value"></param>
        public static bool AppendChipIDList(string value)
        {
            try
            {
                if (chipid_list.Contains(value))
                    return false;
                chipid_list.Add(value);
                TxtFile.DealTxt_Inst.WriteText(Path_ChipIDList, value);
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }

            return true;
        }
        #endregion

        #region 接口
        /// <summary>
        /// CIM的初始化，连接队列并打开monitor
        /// </summary>
        public bool Init()
        {
            return Connect() == "0000";
        }
        #endregion
    }

    /// <summary>
    /// 过账类型，分chipid/lot/trackout
    /// </summary>
    public enum PostType
    {
        ChipID,
        Lot,
        TrackOut
    }

    /// <summary>
    /// CIM的参数
    /// </summary>
    public enum CIM_PARAMS
    {
        SendQueue,
        ReadQueue,
        IP,
        Port,
        UserID,
        Fab,
        Area,
        Line,
        Operation,
        RunCard
    }
}
