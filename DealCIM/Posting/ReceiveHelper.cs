using BasicClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DealCIM
{
    public partial class ReceiveHelper
    {
        #region 定义
        static string ClassName = "ReceiveHelper";
        #endregion

        public static void Monitor()
        {
            try
            {
                string data = string.Empty;
                string key = string.Empty;
                int i = 0;
                while (i++ < 5)
                {
                    Thread.Sleep(300);
                    if (CIM.C_I.ReadData(out data, out key) == "0000")
                    {
                        CIM.AddDic(key, XMLHelpler.ParseStr(data));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
        }

        public static bool CheckResult(string key,out bool value)
        {
            value = false;
            if(CIM.CheckDic(key as string,out value))
            {
                return true;
            }
            return false;
        }
    }
}
