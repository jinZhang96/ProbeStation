using BasicClass;
using DealPLC;
using System;

namespace Main
{
    /// <summary>
    /// PLC的相关定义和实现都在WinInitMain类里面 20181219-zx
    /// </summary>
    partial class MainWindow
    {
        #region PLC触发响应
        /// <summary>
        /// 报警
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_PLCAlarm_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("设备发送报警信息!");
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 物料信息
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void L_I_PLCMaterial_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {

                ShowState("设备发送物料信息!");
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 语音信息
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void L_I_VoiceState_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowVoice(i);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 设备状态
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="intState"></param>
        protected override void LogicPLC_Inst_PLCState_event(TriggerSource_enum trrigerSource_e, int intState)
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
        /// 机器人状态
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="intState"></param>
        protected override void LogicPLC_Inst_RobotState_event(TriggerSource_enum trrigerSource_e, int intState)
        {

        }

        /// <summary>
        /// 数据超限
        /// </summary>
        /// <param name="str"></param>
        protected override void L_I_WriteDataOverFlow(string str)
        {
            ShowAlarm("PLC输出数据超出范围");

            LogicPLC.L_I.PCAlarm();
        }
        #endregion PLC触发响应

        #region PLC换型相关
        /// <summary>
        /// 换型的时候写入PLC的值
        /// </summary>
        public override void WritePLCModelPar()
        {
            try
            {

                //判断配方有没有输错
                VerifyRecipe();

                LogicPLC.L_I.WriteRegData2((int)DataRegister2.WidthAtPlat, ModelParams.WastageX);
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.HeightAtPlat, ModelParams.WastageY);
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.CodeXAtPlat, ModelParams.CodeXInWastage);
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.CodeYAtPlat, ModelParams.CodeYInWastage);
                //LogicPLC.L_I.WriteRegData2((int)DataRegister2.MarkXAtPlat, ModelParams.MarkXInWastage);
                //LogicPLC.L_I.WriteRegData2((int)DataRegister2.MarkXAtPlat, ModelParams.MarkYInWastage);
                ShowState(string.Format("残才平台玻璃角度:{0}", ModelParams.WastageAngle));
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.TopEAtPlat, ModelParams.GetCurElectordeWidth(0));
                ShowState(string.Format("残才平台上电极宽度:{0}", ModelParams.GetCurElectordeWidth(0)));
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.LeftEAtPlat, ModelParams.GetCurElectordeWidth(1));
                ShowState(string.Format("残才平台左电极宽度:{0}", ModelParams.GetCurElectordeWidth(1)));
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.BottomEAtPlat, ModelParams.GetCurElectordeWidth(2));
                ShowState(string.Format("残才平台下电极宽度:{0}", ModelParams.GetCurElectordeWidth(2)));
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.RightEAtPlat, ModelParams.GetCurElectordeWidth(3));
                ShowState(string.Format("残才平台右电极宽度:{0}", ModelParams.GetCurElectordeWidth(3)));
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.PlatAngle, (-ModelParams.WastageAngle + 360) % 360);
                LogicPLC.L_I.WriteRegData1((int)DataRegister1.BeltRatioY, ModelParams.BeltRatioY);

                //戴金林处SPJ特有
                LogicPLC.L_I.WriteRegData1((int)DataRegister1.BeltRatioY, ModelParams.BeltRatioY);
                ShowState(string.Format("玻璃Y方向抛料运行时间:{0}s", ModelParams.BeltRatioY / 10));
                LogicPLC.L_I.WriteRegData1((int)DataRegister1.BeltRatioX, ModelParams.BeltRatioX);
                ShowState(string.Format("玻璃X方向抛料运行时间:{0}s", ModelParams.BeltRatioX / 10));

                LogicPLC.L_I.WriteRegData1(3, 1);

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void VerifyRecipe()
        {
            int cnt = 0;
            bool blError = false;
            foreach (var item in ModelParams.ElectrodeArray)
            {
                if (item != 0)
                    cnt++;
                if (cnt > 2)
                {
                    blError = true;
                    ShowAlarm("配方中电极宽度出错");
                    break;
                }
            }

            if(!(ModelParams.confLayerSpacing>0))
            {
                blError = true;
                ShowAlarm("配方中卡塞层间距错误");
            }

            if(blError)
            {                
                LogicPLC.L_I.PCAlarm();
            }
                
        }
        #endregion PLC换型相关
    }
}
