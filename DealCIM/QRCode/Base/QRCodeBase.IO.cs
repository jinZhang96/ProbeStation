using BasicClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DealCIM
{
    partial class QRCodeBase
    {
        #region 定义
        string data = string.Empty;
        public static StrAction GetData_event;
        #endregion

        #region read
        public virtual void StartMonitor()
        {
            try
            {
                blReadEnabled = true;
                taskRead = Task.Factory.StartNew(Read);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
        }

        public virtual void Read()
        {
            while (blReadEnabled)
            {
                Thread.Sleep(100);
                try
                {
                    string content = serialPort.ReadExisting();
                    if (!content.Contains('\n'))
                        data += content;
                    else
                    {
                        content = content.Replace("\n", "").Replace("\r", "");
                        data += content;
                        GetData_event(data);
                    }
                }
                catch (Exception ex)
                {
                    Log.L_I.WriteError(ClassName, ex);
                }
                finally
                {
                    data = string.Empty;
                }
            }
        }
        #endregion
    }
}
