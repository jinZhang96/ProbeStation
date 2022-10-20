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
using System.IO;
using System.Diagnostics;
using BasicDisplay;
using Main_EX;


namespace Main
{
    public partial class DealComprehensiveResult6 : BaseDealComprehensiveResult_Main
    {

        /// <summary>
        /// 位置1处理
        /// </summary>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public override StateComprehensive_enum DealComprehensiveResultFun1(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            //int pos = 1;
            bool blResult = true;//结果是否正确
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    return DealResult(1, string.Format("相机{0}空跑默认OK", g_NoCamera));
                }

                //StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                //return BaseDealInsert.B_I.DealCSTDetect(trigerSource_e, 1, g_NoCamera, g_regClearCamera, g_regFinishPhoto, htResult);
                return CSTProcessing(trigerSource_e, 1, 1, out htResult);
                
            }
            catch (Exception ex)
            {
                LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录

                Display(Pos_enum.Pos1, htResult, blResult, sw);

                #endregion 显示和日志记录
            }
        }

        /// <summary>
        /// 位置2
        /// </summary>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public override StateComprehensive_enum DealComprehensiveResultFun2(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            //int pos = 1;
            bool blResult = true;//结果是否正确
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    return DealResult(1, string.Format("相机{0}空跑默认OK", g_NoCamera));
                }

                //StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                //return BaseDealInsert.B_I.DealCSTDetect(trigerSource_e, 2, g_NoCamera, g_regClearCamera, g_regFinishPhoto, htResult);
                return CSTProcessing(trigerSource_e, 2, 1, out htResult);
            }
            catch (Exception ex)
            {
                LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录

                Display(Pos_enum.Pos1, htResult, blResult, sw);
                #endregion 显示和日志记录
            }
        }

        /// <summary>
        /// 位置3
        /// </summary>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public override StateComprehensive_enum DealComprehensiveResultFun3(TriggerSource_enum trigerSource_e,
            out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            int pos = 1;
            bool blResult = true;//结果是否正确
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    FinishPhotoPLC(1);
                    ShowState(string.Format("空跑模式，相机{0}第3次拍照默认ok", g_NoCamera));
                    return StateComprehensive_enum.True;
                }

                //StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                //return BaseDealInsert.B_I.DealCSTDetect(trigerSource_e, 3, g_NoCamera, g_regClearCamera, g_regFinishPhoto, htResult);
                return CSTProcessing(trigerSource_e, 3, 1, out htResult);
            }
            catch (Exception ex)
            {
                LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录
                Display(Pos_enum.Pos1, htResult, blResult, sw);
                #endregion 显示和日志记录
            }
        }

        /// <summary>
        /// 位置4
        /// </summary>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public override StateComprehensive_enum DealComprehensiveResultFun4(TriggerSource_enum trigerSource_e,
            out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            int pos = 1;
            bool blResult = true;//结果是否正确
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    FinishPhotoPLC(1);
                    ShowState(string.Format("空跑模式，相机{0}第4次拍照默认ok", g_NoCamera));
                    return StateComprehensive_enum.True;
                }

                //StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                //return BaseDealInsert.B_I.DealCSTDetect(trigerSource_e, 4, g_NoCamera, g_regClearCamera, g_regFinishPhoto, htResult);
                return CSTProcessing(trigerSource_e, 3, 2, out htResult);
            }
            catch (Exception ex)
            {
                LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录
                Display(Pos_enum.Pos1, htResult, blResult, sw);
                #endregion 显示和日志记录
            }
        }
    }
}
