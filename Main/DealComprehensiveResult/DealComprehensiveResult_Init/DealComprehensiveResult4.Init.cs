using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealPLC;
using System.Threading;
using System.Threading.Tasks;
using DealFile;
using DealComprehensive;
using Common;
using SetPar;
using ParComprehensive;
using BasicClass;
using Camera;
using System.Collections;
using DealResult;
using DealConfigFile;
using DealCalibrate;
using DealRobot;
using DealMath;
using DealImageProcess;
using BasicComprehensive;
using System.Diagnostics;
using BasicDisplay;
using Main_EX;

namespace Main
{
    partial class DealComprehensiveResult4
    {
        #region 静态类实例
        public static DealComprehensiveResult4 D_I = new DealComprehensiveResult4();
        #endregion 静态类实例

        #region 初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        public DealComprehensiveResult4()
        {
            try
            {
                base.NameClass = "DealComprehensiveResult4";
                //图像处理参数
                base.g_BaseParComprehensive = ParComprehensive4.P_I;
                base.g_BaseDealComprehensive = DealComprehensive4.D_I;
                base.g_DealComprehensiveBase = DealComprehensive4.D_I;

                g_NoCamera = 4;
                NoCamera_e = NoCamera_enum.Camera4;

                //初始化PLC寄存器
                InitPLCReg();

                //判断并设置显示的独立线程
                InitDisplay_Task();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("DealComprehensiveResult2", ex);
            }
        }

        /// <summary>
        /// 初始化DealPLC寄存器
        /// </summary>
        void InitPLCReg()
        {
            try
            {
                if (ParSetPLC.P_I.TypePLC_e != TypePLC_enum.Null)//三菱PLC
                {
                    if (ParCameraWork.NumCamera > 3)
                    {
                        base.g_regClearCamera = ParSetPLC.P_I.regClearCamera4;
                        base.g_regFinishPhoto = ParSetPLC.P_I.regFinishPhoto_Camera4;
                        base.g_regData_L.Add(ParSetPLC.P_I.regDataX_Camera4);
                        base.g_regData_L.Add(ParSetPLC.P_I.regDataY_Camera4);
                        base.g_regData_L.Add(ParSetPLC.P_I.regDataZ_Camera4);
                        base.g_regData_L.Add(ParSetPLC.P_I.regDataR_Jamera4);
                        base.g_regFinishData = ParSetPLC.P_I.regFinsihData_Camera4;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化

    }
}
