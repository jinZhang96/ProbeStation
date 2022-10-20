using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using BasicClass;

namespace DealCIM
{
    public class XMLHelpler
    {
        #region 定义

        const string ClassName = "XMLHelper";
        public string PathCreate = string.Empty;
        public string PathParse = string.Empty;

        public static IntAction UpdataLotNum_event;
        #endregion

        #region getstring 接口
        /// <summary>
        /// 根据指定的chipid和modelno创建对应的string格式xml
        /// </summary>
        /// <param name="chipid"></param>
        /// <param name="modelno"></param>
        /// <returns></returns>
        public static string GetChipIDData(string chipid,string modelno)
        {
            return XMLToString(CreateChipIDXML(chipid, modelno));            
        }

        /// <summary>
        /// 根据指定的modelno创建对应的string格式xml
        /// </summary>
        /// <param name="modelno"></param>
        /// <returns></returns>
        public static string GetLotData(string modelno)
        {
            return XMLToString(CreateLotXML(modelno));
        }
        
        /// <summary>
        /// 根据指定的chip list和modelno创建对应string格式的xml
        /// </summary>
        /// <param name="list"></param>
        /// <param name="modelno"></param>
        /// <returns></returns>
        public static string GetTrackOutData(List<string> list,string modelno)
        {
            return XMLToString(CreateTrackOutXML(list, modelno));
        }
        #endregion

        #region create xml接口
        /// <summary>
        /// 读取本地标准xml文件然后适当修改节点内容生成xml
        /// </summary>
        /// <param name="chipid"></param>
        /// <param name="modelno"></param>
        /// <returns></returns>
        public static XmlDocument CreateChipIDXML(string chipid, string modelno)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {                
                xmlDoc.Load(CIM.Path_XML_ChipID);

                CIM.AssignXML(ref xmlDoc, modelno);
                XmlNode xn = xmlDoc.DocumentElement.SelectSingleNode("chip_id");
                xn.InnerText = chipid;                
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }

            //log
            try
            {
                string path = Log.CreateAllTimeFile(CIM.Path_Log_ChipID) + DateTime.Now.ToLongTimeString().Replace(":", "-") + "_" + chipid + ".xml";
                xmlDoc.Save(path);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
            return xmlDoc;
        }

        /// <summary>
        /// 读取本地标准xml文件后修改适当节点内容生成xml
        /// </summary>
        /// <param name="modelno"></param>
        /// <returns></returns>
        public static XmlDocument CreateLotXML(string modelno)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(CIM.Path_XML_Lot);

                CIM.AssignXML(ref xmlDoc, modelno);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }

            try
            {
                string path = Log.CreateAllTimeFile(CIM.Path_Log_Lot) + DateTime.Now.ToLongTimeString().Replace(":", "-") + "_Lot" + ".xml";
                xmlDoc.Save(path);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
            return xmlDoc;
        }

        /// <summary>
        /// 读取本地标准xml文件后修改适当节点内容生成xml
        /// </summary>
        /// <param name="chipid_list"></param>
        /// <param name="modelno"></param>
        /// <returns></returns>
        public static XmlDocument CreateTrackOutXML(List<string> chipid_list, string modelno)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(CIM.Path_XML_TrackOut);
                CIM.AssignXML(ref xmlDoc, modelno);

                XmlElement xe = (XmlElement)xmlDoc.SelectSingleNode(CIM.strRoot).SelectSingleNode("chip_info");                
                foreach(string item in chipid_list)
                {
                     XmlElement xe1 = xmlDoc.CreateElement("chip_id");
                     xe1.InnerText = item;
                     xe.AppendChild(xe1);
                } 
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }

            try
            {
                string path = Log.CreateAllTimeFile(CIM.Path_Log_TrackOut) + DateTime.Now.ToLongTimeString().Replace(":", "-") + ".xml";
                xmlDoc.Save(path);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }

            return xmlDoc;
        }
        #endregion

        #region 接口
        /// <summary>
        /// 格式转换，根据二维码把数据记录到本地
        /// </summary>
        /// <param name="strCode"></param>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static XmlDocument StringToXML(string strCode, string xmlString)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xmlString);

            try
            {
                string path = Log.CreateAllTimeFile(ParPathRoot.PathRoot + "软件运行记录\\Custom\\") + DateTime.Now.ToLongTimeString().Replace(":", "-") + "_" + strCode + "Reply.xml";
                xmldoc.Save(path);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
            return xmldoc;
        }

        /// <summary>
        /// 格式转换，不带二维码，不做记录
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static XmlDocument StringToXML(string xmlString)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xmlString.Trim());

            try
            {
                string path = Log.CreateAllTimeFile(CIM.Path_Log_Response) + DateTime.Now.ToLongTimeString().Replace(":", "-") + "_" + "Response.xml";
                xmldoc.Save(path);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }

            return xmldoc;
        }

        /// <summary>
        /// 格式转换
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static string XMLToString(XmlDocument xmlDoc)
        {
            return xmlDoc.InnerXml;
        }
        #endregion

        #region parse 接口
        /// <summary>
        /// 解析字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool ParseStr(string data)
        {
            return ParseXML(StringToXML(data));
        }

        /// <summary>
        /// 解析xml
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        private static bool ParseXML(XmlDocument xmlDoc)
        {
            try
            {
                XmlElement root = xmlDoc.DocumentElement;
                XmlNode xn = root.SelectSingleNode("lot_qty");                
                //如果有该节点，获取该节点的值作为lotnum显示到主界面，至于为什么要显示，原因不明
                if (xn != null && int.TryParse(xn.InnerText, out int lotnum))
                    UpdataLotNum_event?.Invoke(lotnum);

                xn = root.SelectSingleNode("result");
                //此处不区分trackout/validate_lot，只要返回值是ok的就返回true
                return xn.InnerText == "valid" || xn.InnerText == "success";
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                return false;
            }
        }
        #endregion
    }
}
