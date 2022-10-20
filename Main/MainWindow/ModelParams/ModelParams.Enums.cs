namespace Main
{
    #region robot std
    public enum BotStd
    {
        /// <summary>
        /// 粗定位取片位置
        /// </summary>
        PickPos = 1,
        /// <summary>
        /// 精定位位置
        /// </summary>
        PrecisePos,
        /// <summary>
        /// 二维码位置
        /// </summary>
        QrCodePos,
        /// <summary>
        /// 平台1位置
        /// </summary>
        Platform1,
        /// <summary>
        /// 平台2位置
        /// </summary>
        Platform2,
        /// <summary>
        /// 插栏交接位置
        /// </summary>
        InsertArmPos,
        /// <summary>
        /// 抛料位置
        /// </summary>
        DumpPos,
        /// <summary>
        /// 皮带线取片基准值
        /// </summary>
        BeltPickPos,
    }
    #endregion

    #region robot adj
    public enum BotAdj
    {
        /// <summary>
        /// 粗定位取片位置
        /// </summary>
        PickPos = 1,
        /// <summary>
        /// 精定位位置
        /// </summary>
        PrecisePos,
        /// <summary>
        /// 二维码位置
        /// </summary>
        QrCodePos,
        /// <summary>
        /// 平台1位置
        /// </summary>
        Platform1,
        /// <summary>
        /// 平台2位置
        /// </summary>
        Platform2,
        /// <summary>
        /// 插栏交接位置
        /// </summary>
        InsertArmPos,
        /// <summary>
        /// 抛料位置
        /// </summary>
        DumpPos,
        /// <summary>
        /// 皮带线取片基准值
        /// </summary>
        BeltPickPos,
        PickOffset,
    }
    #endregion

    #region plc寄存器
    /// <summary>
    /// 配方寄存器 d5020
    /// </summary>
    public enum RecipeRegister
    {
        /// <summary>
        /// 玻璃X
        /// </summary>
        GlassX,
        /// <summary>
        /// 玻璃Y
        /// </summary>
        GlassY,
        /// <summary>
        /// 玻璃厚度
        /// </summary>
        Thickness,
        /// <summary>
        /// 二维码X
        /// </summary>
        QrCodeX,
        /// <summary>
        /// 二维码Y
        /// </summary>
        QrCodeY,
        /// <summary>
        /// 上电极宽度
        /// </summary>
        TopE,
        /// <summary>
        /// 下电极宽度
        /// </summary>
        BottomE,
        /// <summary>
        /// 左电极宽度
        /// </summary>
        LeftE,
        /// <summary>
        /// 右电极宽度
        /// </summary>
        RightE,
        /// <summary>
        /// 卡塞行数
        /// </summary>
        CSTRows,
        /// <summary>
        /// 卡塞列数
        /// </summary>
        CSTCols,
        /// <summary>
        /// 龙骨间距
        /// </summary>
        KeelInterval,
        /// <summary>
        /// 龙骨层间距
        /// </summary>
        KeelSpacing,
        /// <summary>
        /// 第一列龙骨间距
        /// </summary>
        DisCol1,
        /// <summary>
        /// 插栏方向
        /// </summary>
        DIR_Insert,
        /// <summary>
        /// 抛料方向
        /// </summary>
        DIR_Dump,
        /// <summary>
        /// 中片取片总数
        /// </summary>
        SUMPick,
        /// <summary>
        /// 插栏深度
        /// </summary>
        InsertDepth,
        /// <summary>
        /// 卡塞高度
        /// </summary>
        CSTHeight,
        /// <summary>
        /// 大小卡塞
        /// </summary>
        CSTSize,
        /// <summary>
        /// 龙骨深度
        /// </summary>
        KeelDepth,
        /// <summary>
        /// 插栏起始行
        /// </summary>
        StartRow,
        /// <summary>
        /// 插栏起始列
        /// </summary>
        StartCol,
        /// <summary>
        /// 隔几行插栏
        /// </summary>
        RowInterval,
        /// <summary>
        /// 残材平台工位号
        /// </summary>
        PlatStaionNo=25,
    }
    /// <summary>
    /// 数据寄存器1，d1200
    /// </summary>
    public enum DataRegister1
    {
        PCAlarm = 1,
        /// <summary>
        /// 皮带线系数
        /// </summary>
        BeltRatioY = 4,
        /// <summary>
        /// 当前工位取片完成
        /// </summary>
        NextStation,
        /// <summary>
        /// 二维码读取结果
        /// </summary>
        QrCodeResult,
        /// <summary>
        /// 插栏数据确认
        /// </summary>
        InsertDataConfirm,
        /// <summary>
        /// 当前插栏总数
        /// </summary>
        CurrentInsertSum,
        /// <summary>
        /// 搬运臂插栏方向
        /// </summary>
        InsertDirectionForTrans,
        /// <summary>
        /// 搬运臂抛料方向
        /// </summary>
        DumpDirectionForTrans,
        /// <summary>
        /// Trackout结果
        /// </summary>
        TrackOutResult,
        /// <summary>
        /// 巡边交接偏差确认
        /// </summary>
        InspDeviationConfirm,
        /// <summary>
        /// 皮带线停止
        /// </summary>
        StopBelt,
        /// <summary>
        /// 重启机器人
        /// </summary>
        RestartBot,
        /// <summary>
        /// LotNum
        /// </summary>
        BeltRatioX,
        /// <summary>
        /// 残材连续低清晰度报警
        /// </summary>
        WastageAlarm,
    }
    /// <summary>
    /// 数据寄存器2，d1250
    /// </summary>
    public enum DataRegister2
    {
        /// <summary>
        /// 插栏1基准值
        /// </summary>
        StdCSTPos1,
        /// <summary>
        /// 插栏2基准值
        /// </summary>
        StdCSTPos2,
        /// <summary>
        /// 插栏3基准值
        /// </summary>
        StdCSTPos3,
        /// <summary>
        /// 插栏4基准值
        /// </summary>
        StdCSTPos4,
        /// <summary>
        /// 翻转平台处玻璃角度
        /// </summary>
        PlatAngle,
        /// <summary>
        /// 平台处玻璃X
        /// </summary>
        WidthAtPlat,
        /// <summary>
        /// 平台处玻璃Y
        /// </summary>
        HeightAtPlat,
        /// <summary>
        /// 平台处二维码X
        /// </summary>
        CodeXAtPlat,
        /// <summary>
        /// 平台处二维码Y
        /// </summary>
        CodeYAtPlat,
        /// <summary>
        /// 平台处上电极宽度
        /// </summary>
        TopEAtPlat,
        /// <summary>
        /// 平台处下电极宽度
        /// </summary>
        BottomEAtPlat,
        /// <summary>
        /// 平台处左电极宽度
        /// </summary>
        LeftEAtPlat,
        /// <summary>
        /// 平台处右电极宽度
        /// </summary>
        RightEAtPlat,
      
    }
    /// <summary>
    /// 数据寄存器3，d1300
    /// </summary>
    public enum DataRegister3
    {
        InsertStdCom,
        InsertData,
        InsertComZ1,
        KeelSpacing1 = 8,
    }
    #endregion

    #region plc报警代码
    public enum PCAlarm_Enum
    {
        标定失败 = 1,
        卡塞计算失败 = 2,
    }
    #endregion

    #region 运行模式设定
    public enum RunningMode
    {
        PassCamera3,
        PassCamera4,
        PassCamera5,
        Mirror,
        RecordData,
        UseRealRC
    }
    #endregion
}
