#define TRACE

using BasicClass;
using McmqApi;
using System;
using System.Security.Cryptography;
using System.Threading;

namespace DealCIM
{
    public partial class CIM
    {
        #region 静态类实例
        public static CIM C_I = new CIM();

        /// <summary>
        /// 私有构造函数，避免在外部手动创建实例
        /// </summary>
        private CIM()
        {
            InitParams();
        }
        #endregion 静态类实例

        #region 定义
        static string ClassName = "CIM";
        /// <summary>
        /// 友达提供的库，接口实例
        /// </summary>
        cMcmqCommApi mcmq = new cMcmqCommApi();
        /// <summary>
        /// Unique ID，每次通信时的唯一ID
        /// </summary>
        string correlationID = string.Empty;
        /// <summary>
        /// 重连次数n，则实际重连n-1次
        /// </summary>
        const int ReconnectCnt = 4;
        /// <summary>
        /// 互锁，线程保护
        /// </summary>
        object locker = new object();
        #endregion 定义

        #region 连接断开
        /// <summary>
        /// 使用mcmq与服务器进行连接
        /// </summary>
        /// <returns></returns>
        public string Connect()
        {
            try
            {
                string strResult = mcmq.connectMCMQ(StrIP, Int16.Parse(StrPort));
                return strResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("CIM", ex);
                return "";
            }
        }

        /// <summary>
        /// 断开与服务器的连接
        /// </summary>
        /// <returns></returns>
        public string DisConnect()
        {
            string strResult = string.Empty;
            try
            {
                strResult = mcmq.disconnect();
                return strResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("CIM", ex);
                return strResult;
            }
        }

        /// <summary>
        /// 重连
        /// </summary>
        public bool ReConnect()
        {
            try
            {
                lock (locker)
                {
                    return DisConnect() == "0000" && Connect() == "0000";
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("CIM", ex);
                return false;
            }
        }
        #endregion 连接断开

        #region 发送数据
        /// <summary>
        /// 发送数到服务器，并抛出当前通信连接的unique id，用于接收结果时进行校验
        /// </summary>
        /// <param name="strData">需要发送的数据</param>
        /// <param name="key">unique id</param>
        /// <returns>返回值信息，包含return code/error code/exception msg</returns>
        public string WriteData(string strData, out string key)
        {
            key = string.Empty;
            try
            {
                //以防万一，基本不可能抛异常
                if (!GenerateID())
                {
                    return "Generate ID Error";
                }

                string strResult = string.Empty;
                int i = 0;
                //格式转换
                byte[] bytes = System.Text.Encoding.Default.GetBytes(strData);

                lock (locker)
                {
                    do
                    {
                        try
                        {
                            //计数超过1则表明获取反馈结果失败，所以重连之后再读取
                            if (i++ > 0)
                                ReConnect();
#if TRACE
                            Log.L_I.WriteError("MCMQ", "putQueue", "sendQueue:" + StrSendQueue + '\n' + " readQueue:" + StrReadQueue + '\n' + " correlationID:" + correlationID + '\n' + strData);
#endif
                            strResult = mcmq.putQueue(StrSendQueue, bytes, StrReadQueue, correlationID);
#if TRACE
                            Log.L_I.WriteError("MCMQ", "ReturnValue:" + strResult);
#endif
                        }
                        catch (Exception ex)
                        {
                            Log.L_I.WriteError("ERROR", "QueueHandle：" + mcmq.strQueueHandle);
                            Log.L_I.WriteError("ERROR", "CorrelationID：" + mcmq.getCorrelationID);
                            Log.L_I.WriteError("CIM", ex);
                        }
                        //返回结果OK时得到的返回值是0000
                    } while (strResult != "0000" && i < ReconnectCnt);

                    key = correlationID;
                    return strResult;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("CIM", ex);
                return string.Empty;
            }
        }
        #endregion 发送数据

        #region 读取数据
        /// <summary>
        /// 打开队列
        /// </summary>
        /// <returns></returns>
        private string OpenQueue()
        {
            string strResult = string.Empty;
            try
            {
                strResult = mcmq.openQueue(StrReadQueue, 1000000, 5000, 500000, 2500, 130, false, "TEST_QUEUE");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("CIM", ex);
            }

            return strResult;
        }

        /// <summary>
        /// 从队列中获取反馈结果
        /// </summary>
        /// <returns></returns>
        private string GetQueue()
        {
            int i = 0;

            string strResult = string.Empty;
            //线程锁
            lock (locker)
            {
                do
                {
                    try
                    {
                        if (i++ > 0)
                            ReConnect();
#if TRACE
                        Log.L_I.WriteError("MCMQ", "QueueHandle:" + mcmq.strQueueHandle);
#endif
                        strResult = mcmq.getQueue(mcmq.strQueueHandle, 60000);
#if TRACE
                        Log.L_I.WriteError("MCMQ", "ReturnValue:" + strResult + '\n' + "CorrelationID:" + mcmq.getCorrelationID);
#endif
                    }
                    catch (Exception ex)
                    {
                        Log.L_I.WriteError("ERROR", "QueueHandle：" + mcmq.strQueueHandle);
                        Log.L_I.WriteError("ERROR", "CorrelationID：" + mcmq.getCorrelationID);
                        Log.L_I.WriteError("CIM", ex);
                    }
                } while (strResult != "0000" && i < ReconnectCnt);

                return strResult;
            }
        }

        /// <summary>
        /// 关闭队列
        /// </summary>
        /// <returns></returns>
        private string CloseQueue()
        {
            string strResult = string.Empty;
            try
            {
                strResult = mcmq.closeQueue(mcmq.strQueueHandle);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("CIM", ex);
            }

            return strResult;
        }

        /// <summary>
        /// 读取队列中的反馈结果，是对getqueue的封装，是老版本，在读取非对应id的情况下会抛掉结果重新读取，存在的问题是当出现时序错位时，可能导致数据丢失
        /// </summary>
        /// <param name="strData">输出读取到的return code/exception msg/error code</param>
        /// <param name="strMsgID">correlation id只读取对应id的数据，如果不是对应id则继续读取</param>
        /// <returns></returns>
        public string ReadData(out string strData, string strMsgID)
        {
            strData = "";
            try
            {
                string strResult = OpenQueue();
                if (strResult != "0000")
                {
                    return strResult;
                }
                int times = 0;
                LblRead:
                strResult = GetQueue();
                if (strResult != "0000")
                {
                    return strResult;
                }

                byte[] reply_message = mcmq.getBinMessage;
                if (!(reply_message.Length > 0))
                    return "BinMsg is Empty";

                strData = System.Text.Encoding.ASCII.GetString(reply_message);
                if (!(strData.Length > 0))
                    return "BinMsg to String is Empty";

                if (!strData.Contains(strMsgID) && times < 10 && (correlationID != mcmq.getCorrelationID))
                {
                    times++;
                    Thread.Sleep(50);
                    goto LblRead;
                }

                if (times >= 10)
                    return "Dont Receive Correct Repsonse";

                mcmq.cleanQueue(mcmq.strQueueHandle);
                strResult = mcmq.closeQueue(mcmq.strQueueHandle);

                return strResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("CIM", ex);
                return "";
            }
        }

        /// <summary>
        /// 新版本使用的读取队列中的结果，并且返回读取的数据以及unique id
        /// </summary>
        /// <param name="strData"></param>
        /// <param name="correlationID"></param>
        /// <returns></returns>
        public string ReadData(out string strData, out string correlationID)
        {
            strData = "";
            correlationID = string.Empty;
            try
            {
                //打开队列
                string strResult = OpenQueue();
                if (strResult != "0000")
                {
                    return strResult;
                }
                //尝试从队列中获取数据
                strResult = GetQueue();
                if (strResult != "0000")
                {
                    return strResult;
                }
                //如果能够读取数据，判断读取的数据是否为空
                byte[] reply_message = mcmq.getBinMessage;
                if (!(reply_message.Length > 0))
                    return "BinMsg is Empty";
                //格式转换，顺便做一次判断，估计没用，加了看看
                strData = System.Text.Encoding.ASCII.GetString(reply_message);
                if (!(strData.Length > 0))
                    return "BinMsg to String is Empty";
                //获取当前信息对应的unique id
                correlationID = mcmq.getCorrelationID;
                //读取完之后要清队列
                mcmq.cleanQueue(mcmq.strQueueHandle);
                //关闭队列
                strResult = mcmq.closeQueue(mcmq.strQueueHandle);

                return strResult;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("CIM", ex);
                return "";
            }
        }
        #endregion 读取数据

        /// <summary>
        /// 生成随机的correlationID
        /// </summary>
        /// <returns></returns>
        private bool GenerateID()
        {
            try
            {
                byte[] randomBytes = new byte[4];
                RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
                rngServiceProvider.GetBytes(randomBytes);
                correlationID = BitConverter.ToInt32(randomBytes, 0).ToString();
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("CIM", ex);
                return false;
            }
        }
    }
}
