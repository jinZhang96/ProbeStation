using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealPLC;
using System.Threading;
using System.Threading.Tasks;
using DealFile;
using DealComprehensive;
using BasicClass;
using System.Collections;
using DealResult;
using DealConfigFile;
using DealCalibrate;
using System.Diagnostics;
using Common;


namespace DealInsert
{
    public class ParInsertRecipe
    {
        #region 创建详情
        //本类用于存储修改配方中的卡塞资料
        #endregion

        #region 静态单例
        public static ParInsertRecipe P_I = new ParInsertRecipe();
        #endregion

        #region 从配方中读取的卡塞资料
        //卡塞行数
        public int key_conf_CSTRow = 0;
        //卡塞列数
        public int key_conf_CSTCol = 0;
        //龙骨间距
        public int key_conf_KeelInterval = 0;
        //第一列龙骨距离基准值的距离
        public int key_conf_Col1Interval = 0;
        //卡塞理论层高
        public int key_conf_CSTLayerInterval = 0;
        #endregion

        #region 属性

        //第一列龙骨到基准的距离
        public double confCol1Interval
        {
            get
            {
                return ParConfigPar.P_I.ParProduct_L[key_conf_Col1Interval].DblValue;
            }
        }

        //卡塞理论层高
        public double stdCSTLayerInterval
        {
            get
            {
                return ParConfigPar.P_I.ParProduct_L[key_conf_CSTLayerInterval].DblValue;
            }
        }

        //龙骨根数
        public int KeelCol
        {
            get
            {
                return confCSTCol + 1;
            }
        }

        //卡塞列数
        public int confCSTCol
        {
            get
            {
                return (int)ParConfigPar.P_I.ParProduct_L[key_conf_CSTCol].DblValue;
            }
        }

        //卡塞行数
        public int confCSTRow
        {
            get
            {
                return (int)ParConfigPar.P_I.ParProduct_L[key_conf_CSTRow].DblValue;
            }
        }

        //卡塞龙骨间距
        public double confKeelInterval
        {
            get
            {
                return ParConfigPar.P_I.ParProduct_L[key_conf_KeelInterval].DblValue;
            }
        }


        public double CST1ComX
        {
            get
            {
                string adj = "adj" + BaseDealInsert.B_I.XComprehensive;
                string std = "std" + BaseDealInsert.B_I.XComprehensive;
                return ParAdjust.Value1(adj) + ParStd.Value1(std);
            }
        }
        public double CST2ComX
        {
            get
            {
                string adj = "adj" + (BaseDealInsert.B_I.XComprehensive + 1);
                string std = "std" + (BaseDealInsert.B_I.XComprehensive + 1);
                return ParAdjust.Value1(adj) + ParStd.Value1(std);
            }
        }
        public double CST3ComX
        {
            get
            {
                string adj = "adj" + (BaseDealInsert.B_I.XComprehensive + 2);
                string std = "std" + (BaseDealInsert.B_I.XComprehensive + 2);
                return ParAdjust.Value1(adj) + ParStd.Value1(std);
            }
        }
        public double CST4ComX
        {
            get
            {
                string adj = "adj" + (BaseDealInsert.B_I.XComprehensive + 3);
                string std = "std" + (BaseDealInsert.B_I.XComprehensive + 3);
                return ParAdjust.Value1(adj) + ParStd.Value1(std);
            }
        }

        #endregion
    }
}
