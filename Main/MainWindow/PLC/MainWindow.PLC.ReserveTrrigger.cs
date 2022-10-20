using BasicClass;
using DealPLC;
using Main_EX;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Main
{
    /// <summary>
    /// 20181219-zx,
    /// </summary>
    partial class MainWindow
    {
        #region 定义
        public Task beltScan = null;
        CancellationTokenSource cts = new CancellationTokenSource();
        #endregion 定义

        /// <summary>
        /// 保留触发1 插栏数据请求
        /// </summary>
        protected override void LogicPLC_Inst_Reserve1_event(TriggerSource_enum trigerSource_e, int i)
        {
            try
            {
                //TODO: 发送插栏数据
                //SendInsertData(i);
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    ShowState("空跑，默认发送插栏数据0");
                    LogicPLC.L_I.WriteRegData3((int)DataRegister3.InsertData, 67);
                    LogicPLC.L_I.WriteRegData1((int)DataRegister1.InsertDataConfirm, 1);
                    return;
                }

                SendInsertData();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发2 卡塞切换工位
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve2_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ChangCSTSationNum(i);
                ChangeCol();
                //TODO: 插栏计数清0
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发3  插栏完成
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve3_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发4  卡塞插满
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve4_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                //TODO: 插满，过账
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发5  换班
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve5_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                //换班
                ShowState("PLC触发换班，产能重置");
                RegeditMain.R_I.ID = 0;
                ProductivityReport.WriteReportIni(i);
                ProductivityReport.ClearReportData();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发6  残材抛料
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve6_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                //记录残材抛料
                if (i == 1)
                    RegeditMain.R_I.WastageNG1++;
                else if (i == 2)
                    RegeditMain.R_I.WastageNG2++;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 二维码拍照
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve7_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                //TODO: 二维码拍照

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 语音
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve8_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                //播放语音
                ShowVoice(i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发9
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve9_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                try
                {
                    ShowState("插栏换列");
                    ChangeCol();
                }
                catch (Exception ex)
                {
                    Log.L_I.WriteError(NameClass, ex);
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发10
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve10_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            finally
            {

            }
        }

        /// <summary>
        /// 保留触发11,刷RunCard
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve11_event(TriggerSource_enum trrigerSource_e, int i)
        {
        }


        /// <summary>
        /// 保留触发12
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve12_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发13 第一拍照位
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve13_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("新中片到达第一拍照位置!");
                RegeditMain.R_I.PickCnt = 0;
                ModelParams.DumpList_Solid.Clear();
                foreach (string item in ModelParams.DumpList_Active)
                {
                    ModelParams.DumpList_Solid.Add(item);
                }

                ModelParams.DumpList_Active.Clear();
                string tmp = "";
                foreach (string index in ModelParams.DumpList_Solid)
                {
                    tmp += index + ",";
                }
                ShowState("当前中片标记NG序号:" + tmp);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发14
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve14_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发15
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve15_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发16
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve16_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发17
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve17_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发18
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve18_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发19
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve19_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发20
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve20_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
    }
}
