using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BasicClass;
using System.IO;
using DealFile;


namespace DealInsert
{
    /// <summary>
    /// InsertWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InsertWindow : Window
    {

        #region 定义
        private const string NameError = @"InsertWindow";
        private const string PathRoot = @"D:\Store\ConfigFile\";
        private const string Path = @"D:\Store\ConfigFile\InsertData.ini";
        private const string StrRecipeSection = @"InsertRecipe";
        private const string StrRegDataSection = @"InsertRegData";
        private const string IntNumCST = @"INTNUMCST";
        private const string IntStdDirection = @"IntStdDirection";
        private const string IntZComprehensiveDirection = @"IntZComprehensiveDirection";

        public static int NumCST
        {
            set
            {
                RegeditFile.R_I.WriteRegedit(IntNumCST, value.ToString());
            }
            get
            {
                return (int)RegeditFile.R_I.ReadRegeditInt(IntNumCST);
            }
        }

        public static int StdDirection
        {
            set
            {
                RegeditFile.R_I.WriteRegedit(IntStdDirection, value.ToString());
            }
            get
            {
                return (int)RegeditFile.R_I.ReadRegeditInt(IntStdDirection);
            }
        }

        public static int ZComprehensiveDirection
        {
            set
            {
                RegeditFile.R_I.WriteRegedit(IntZComprehensiveDirection, value.ToString());
            }
            get
            {
                return (int)RegeditFile.R_I.ReadRegeditInt(IntZComprehensiveDirection);
            }
        }
        #endregion

        #region 本地配方名称记录
                
        private const string Record_CSTRow = "卡塞行数序号";
        private const string Record_CSTColumn = "卡塞列数序号";
        private const string Record_CSTDisKeel = "卡塞龙骨间距序号";
        private const string Record_CSTDisFirstKeel = "卡塞第一根龙骨距离";
        private const string Record_CSTStepZ = "卡塞理论层高";
               
        //通用  
        private const string Record_InsertData_Index1 = "卡塞插篮X坐标一级序号";
        private const string Record_InsertData_Index2 = "卡塞插篮X坐标二级序号";
        private const string Record_CurInsertNum_Index1 = "卡塞当前已插篮数一级序号";
        private const string Record_CurInsertNum_Index2 = "卡塞当前已插篮数二级序号";
        private const string Record_InsertConfirm_Index1 = "卡塞数据确认信号一级序号";
        private const string Record_InsertConfirm_Index2 = "卡塞数据确认信号二级序号";
        private const string Record_CurStationNo_Index1 = "当前卡塞编号一级序号";
        private const string Record_CurStationNo_Index2 = "当前卡塞编号二级序号";
                
        //卡塞1
        private const string Record_CST1Std_Index1 = "卡塞1基准值一级序号";
        private const string Record_CST1Std_Index2 = "卡塞1基准值二级序号";
        private const string Record_CST1Comz_Index1 = "卡塞1高度补偿值一级序号";
        private const string Record_CST1Comz_Index2 = "卡塞1高度补偿值二级序号";
        private const string Record_CST1Stepz_Index1 = "卡塞1层高一级序号";
        private const string Record_CST1Stepz_Index2 = "卡塞1层高二级序号";
                
        //卡塞2 
        private const string Record_CST2Std_Index1 = "卡塞2基准值一级序号";
        private const string Record_CST2Std_Index2 = "卡塞2基准值二级序号";
        private const string Record_CST2Comz_Index1 = "卡塞2高度补偿值一级序号";
        private const string Record_CST2Comz_Index2 = "卡塞2高度补偿值二级序号";
        private const string Record_CST2Stepz_Index1 = "卡塞2层高一级序号";
        private const string Record_CST2Stepz_Index2 = "卡塞2层高二级序号";
                
                
        //卡塞3 
        private const string Record_CST3Std_Index1 = "卡塞3基准值一级序号";
        private const string Record_CST3Std_Index2 = "卡塞3基准值二级序号";
        private const string Record_CST3Comz_Index1 = "卡塞3高度补偿值一级序号";
        private const string Record_CST3Comz_Index2 = "卡塞3高度补偿值二级序号";
        private const string Record_CST3Stepz_Index1 = "卡塞3层高一级序号";
        private const string Record_CST3Stepz_Index2 = "卡塞3层高二级序号";
                
        //卡塞4 
        private const string Record_CST4Std_Index1 = "卡塞4基准值一级序号";
        private const string Record_CST4Std_Index2 = "卡塞4基准值二级序号";
        private const string Record_CST4Comz_Index1 = "卡塞4高度补偿值一级序号";
        private const string Record_CST4Comz_Index2 = "卡塞4高度补偿值二级序号";
        private const string Record_CST4Stepz_Index1 = "卡塞4层高一级序号";
        private const string Record_CST4Stepz_Index2 = "卡塞4层高二级序号";

        #endregion

        #region 窗口实例函数
        public InsertWindow()
        {
            
            InitializeComponent();

            ShowInsertRecipeIni();

            ShowInsertDataRegIni();

            TextBoxNumCST.Text = NumCST.ToString();

            if (StdDirection == 1)
            {
                test1.IsChecked = true;
                test2.IsChecked = false;
            }
            if (StdDirection == -1)
            {
                test1.IsChecked = false;
                test2.IsChecked = true;
            }
            if (ZComprehensiveDirection == 1)
            {
                test3.IsChecked = true;
                test4.IsChecked = false;
            }
            if (ZComprehensiveDirection == -1)
            {
                test3.IsChecked = false;
                test4.IsChecked = true;
            }

            txtCSTPath.Text = BaseDealInsert.c_PathCSTBase;

        }
        #endregion

        #region 饿汉调用
        private static InsertWindow g_InsertWindow = null;
        public static InsertWindow GetInstance()
        {
            if (g_InsertWindow == null)
            {
                g_InsertWindow = new InsertWindow();
            }
            return g_InsertWindow;
        }
        #endregion

        #region 软件初始化数据
        public static void ReadInsertData()
        {
            try
            {
                ReadInsertRecipeIni();
                ReadInsertRegDataIni();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        private static void ReadInsertRecipeIni()
        {
            try
            {
                ParInsertRecipe.P_I.key_conf_CSTRow = ReadInsertRecipeIniBaseInt(Record_CSTRow);
                ParInsertRecipe.P_I.key_conf_CSTCol = ReadInsertRecipeIniBaseInt(Record_CSTColumn);
                ParInsertRecipe.P_I.key_conf_KeelInterval = ReadInsertRecipeIniBaseInt(Record_CSTDisKeel);
                ParInsertRecipe.P_I.key_conf_Col1Interval = ReadInsertRecipeIniBaseInt(Record_CSTDisFirstKeel);
                ParInsertRecipe.P_I.key_conf_CSTLayerInterval = ReadInsertRecipeIniBaseInt(Record_CSTStepZ);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        #endregion

        #region 保存参数
        private void SavePar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((!(bool)test1.IsChecked)&&(!(bool)test2.IsChecked))|| ((!(bool)test3.IsChecked) && (!(bool)test4.IsChecked)))
                {
                    MessageBox.Show("请先选择机构方向，然后重新保存!");
                    return;
                }
                SaveParToMemory();
                SaveParToLocal();
                SaveOthers();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        
        private void SaveOthers()
        {
            try
            {
                if ((bool)test1.IsChecked)
                {
                    StdDirection = 1;
                }
                else
                {
                    StdDirection = -1;
                }

                if ((bool) test3.IsChecked)
                {
                    ZComprehensiveDirection = 1;
                }
                else
                {
                    ZComprehensiveDirection = -1;
                }
                NumCST = int.Parse(TextBoxNumCST.Text);
            }
            catch(Exception ex)
            {
                WriteLog(ex);
            }
        }

        private void SaveParToMemory()
        {
            try
            {
                ParInsertRecipe.P_I.key_conf_CSTRow = int.Parse(NoInsertRow.Text.ToString());
                ParInsertRecipe.P_I.key_conf_CSTCol = int.Parse(NoInsertColumn.Text.ToString());
                ParInsertRecipe.P_I.key_conf_KeelInterval = int.Parse(NoDisKeel.Text.ToString());
                ParInsertRecipe.P_I.key_conf_Col1Interval = int.Parse(NoDisFirstKeel.Text.ToString());
                ParInsertRecipe.P_I.key_conf_CSTLayerInterval = int.Parse(NoStepZ.Text.ToString());

                ParInsertRegData.P_I.addr_insertdata_index1 = int.Parse(TextBoxReserve1.Text.ToString());
                ParInsertRegData.P_I.addr_curinsertnum_index1 = int.Parse(TextBoxReserve2.Text.ToString());
                ParInsertRegData.P_I.addr_confirm_index1 = int.Parse(TextBoxReserve3.Text.ToString());
                ParInsertRegData.P_I.addr_curCSTNo_index1 = int.Parse(TextBoxReserve4.Text.ToString());

                ParInsertRegData.P_I.addr_insertdata_index2 = int.Parse(TextBoxReserve7.Text.ToString());
                ParInsertRegData.P_I.addr_curinsertnum_index2 = int.Parse(TextBoxReserve8.Text.ToString());
                ParInsertRegData.P_I.addr_confirm_index2 = int.Parse(TextBoxReserve9.Text.ToString());
                ParInsertRegData.P_I.addr_curCSTNo_index2 = int.Parse(TextBoxReserve10.Text.ToString());

                ParInsertRegData.P_I.addr_insert1std_index1 = int.Parse(TextBoxReserve13.Text.ToString());
                ParInsertRegData.P_I.addr_insert1comz_index1 = int.Parse(TextBoxReserve14.Text.ToString());
                ParInsertRegData.P_I.addr_insert1stepz_index1 = int.Parse(TextBoxReserve15.Text.ToString());

                ParInsertRegData.P_I.addr_insert1std_index2 = int.Parse(TextBoxReserve19.Text.ToString());
                ParInsertRegData.P_I.addr_insert1comz_index2 = int.Parse(TextBoxReserve20.Text.ToString());
                ParInsertRegData.P_I.addr_insert1stepz_index2 = int.Parse(TextBoxReserve21.Text.ToString());

                ParInsertRegData.P_I.addr_insert2std_index1 = int.Parse(TextBoxReserve16.Text.ToString());
                ParInsertRegData.P_I.addr_insert2comz_index1 = int.Parse(TextBoxReserve17.Text.ToString());
                ParInsertRegData.P_I.addr_insert2stepz_index1 = int.Parse(TextBoxReserve18.Text.ToString());

                ParInsertRegData.P_I.addr_insert2std_index2 = int.Parse(TextBoxReserve22.Text.ToString());
                ParInsertRegData.P_I.addr_insert2comz_index2 = int.Parse(TextBoxReserve23.Text.ToString());
                ParInsertRegData.P_I.addr_insert2stepz_index2 = int.Parse(TextBoxReserve24.Text.ToString());

                ParInsertRegData.P_I.addr_insert3std_index1 = int.Parse(TextBoxReserve25.Text.ToString());
                ParInsertRegData.P_I.addr_insert3comz_index1 = int.Parse(TextBoxReserve26.Text.ToString());
                ParInsertRegData.P_I.addr_insert3stepz_index1 = int.Parse(TextBoxReserve27.Text.ToString());

                ParInsertRegData.P_I.addr_insert3std_index2 = int.Parse(TextBoxReserve31.Text.ToString());
                ParInsertRegData.P_I.addr_insert3comz_index2 = int.Parse(TextBoxReserve32.Text.ToString());
                ParInsertRegData.P_I.addr_insert3stepz_index2 = int.Parse(TextBoxReserve33.Text.ToString());

                ParInsertRegData.P_I.addr_insert4std_index1 = int.Parse(TextBoxReserve28.Text.ToString());
                ParInsertRegData.P_I.addr_insert4comz_index1 = int.Parse(TextBoxReserve29.Text.ToString());
                ParInsertRegData.P_I.addr_insert4stepz_index1 = int.Parse(TextBoxReserve30.Text.ToString());

                ParInsertRegData.P_I.addr_insert4std_index2 = int.Parse(TextBoxReserve34.Text.ToString());
                ParInsertRegData.P_I.addr_insert4comz_index2 = int.Parse(TextBoxReserve35.Text.ToString());
                ParInsertRegData.P_I.addr_insert4stepz_index2 = int.Parse(TextBoxReserve36.Text.ToString());
                
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        private void SaveParToLocal()
        {
            try
            {
                WriteInsertRecipeIni(Record_CSTRow, int.Parse(NoInsertRow.Text.ToString()));
                WriteInsertRecipeIni(Record_CSTColumn, int.Parse(NoInsertColumn.Text.ToString()));
                WriteInsertRecipeIni(Record_CSTDisKeel, int.Parse(NoDisKeel.Text.ToString()));
                WriteInsertRecipeIni(Record_CSTDisFirstKeel, int.Parse(NoDisFirstKeel.Text.ToString()));
                WriteInsertRecipeIni(Record_CSTStepZ, int.Parse(NoStepZ.Text.ToString()));

                WriteInsertRegDataIni(Record_InsertData_Index1, int.Parse(TextBoxReserve1.Text.ToString()));
                WriteInsertRegDataIni(Record_InsertData_Index2, int.Parse(TextBoxReserve7.Text.ToString()));
                WriteInsertRegDataIni(Record_CurInsertNum_Index1, int.Parse(TextBoxReserve2.Text.ToString()));
                WriteInsertRegDataIni(Record_CurInsertNum_Index2, int.Parse(TextBoxReserve8.Text.ToString()));
                WriteInsertRegDataIni(Record_InsertConfirm_Index1, int.Parse(TextBoxReserve3.Text.ToString()));
                WriteInsertRegDataIni(Record_InsertConfirm_Index2, int.Parse(TextBoxReserve9.Text.ToString()));
                WriteInsertRegDataIni(Record_CurStationNo_Index1, int.Parse(TextBoxReserve4.Text.ToString()));
                WriteInsertRegDataIni(Record_CurStationNo_Index2, int.Parse(TextBoxReserve10.Text.ToString()));


                WriteInsertRegDataIni(Record_CST1Std_Index1, int.Parse(TextBoxReserve13.Text.ToString()));
                WriteInsertRegDataIni(Record_CST1Std_Index2, int.Parse(TextBoxReserve19.Text.ToString()));
                WriteInsertRegDataIni(Record_CST1Comz_Index1, int.Parse(TextBoxReserve14.Text.ToString()));
                WriteInsertRegDataIni(Record_CST1Comz_Index2, int.Parse(TextBoxReserve20.Text.ToString()));
                WriteInsertRegDataIni(Record_CST1Stepz_Index1, int.Parse(TextBoxReserve15.Text.ToString()));
                WriteInsertRegDataIni(Record_CST1Stepz_Index2, int.Parse(TextBoxReserve21.Text.ToString()));

                WriteInsertRegDataIni(Record_CST2Std_Index1, int.Parse(TextBoxReserve16.Text.ToString()));
                WriteInsertRegDataIni(Record_CST2Std_Index2, int.Parse(TextBoxReserve22.Text.ToString()));
                WriteInsertRegDataIni(Record_CST2Comz_Index1, int.Parse(TextBoxReserve17.Text.ToString()));
                WriteInsertRegDataIni(Record_CST2Comz_Index2, int.Parse(TextBoxReserve23.Text.ToString()));
                WriteInsertRegDataIni(Record_CST2Stepz_Index1, int.Parse(TextBoxReserve18.Text.ToString()));
                WriteInsertRegDataIni(Record_CST2Stepz_Index2, int.Parse(TextBoxReserve24.Text.ToString()));

                WriteInsertRegDataIni(Record_CST3Std_Index1, int.Parse(TextBoxReserve25.Text.ToString()));
                WriteInsertRegDataIni(Record_CST3Std_Index2, int.Parse(TextBoxReserve31.Text.ToString()));
                WriteInsertRegDataIni(Record_CST3Comz_Index1, int.Parse(TextBoxReserve26.Text.ToString()));
                WriteInsertRegDataIni(Record_CST3Comz_Index2, int.Parse(TextBoxReserve32.Text.ToString()));
                WriteInsertRegDataIni(Record_CST3Stepz_Index1, int.Parse(TextBoxReserve27.Text.ToString()));
                WriteInsertRegDataIni(Record_CST3Stepz_Index2, int.Parse(TextBoxReserve33.Text.ToString()));

                WriteInsertRegDataIni(Record_CST4Std_Index1, int.Parse(TextBoxReserve28.Text.ToString()));
                WriteInsertRegDataIni(Record_CST4Std_Index2, int.Parse(TextBoxReserve34.Text.ToString()));
                WriteInsertRegDataIni(Record_CST4Comz_Index1, int.Parse(TextBoxReserve29.Text.ToString()));
                WriteInsertRegDataIni(Record_CST4Comz_Index2, int.Parse(TextBoxReserve35.Text.ToString()));
                WriteInsertRegDataIni(Record_CST4Stepz_Index1, int.Parse(TextBoxReserve30.Text.ToString()));
                WriteInsertRegDataIni(Record_CST4Stepz_Index2, int.Parse(TextBoxReserve36.Text.ToString()));

                MessageBox.Show("信息录入成功!");
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                MessageBox.Show("信息录入失败!");
            }
        }
        #endregion

        #region Log
        private static void WriteLog(Exception ex)
        {
            Log.L_I.WriteError(NameError, ex);
        }
        #endregion

        private void ClosingWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                g_InsertWindow = null;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        private void WriteInsertRecipeIni(string strKey, int Number)
        {
            try
            {
                string str = Number.ToString();
                IniFile.I_I.WriteIni(StrRecipeSection, strKey, str, Path);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        private void WriteInsertRegDataIni(string strKey, int Number)
        {
            try
            {
                string str = Number.ToString();
                IniFile.I_I.WriteIni(StrRegDataSection, strKey, str, Path);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        private static int ReadInsertRegDataIniBaseInt(string key)
        {
            try
            {
                return IniFile.I_I.ReadIniInt(StrRegDataSection, key, Path);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                return 0;
            }
        }

        private static int ReadInsertRecipeIniBaseInt(string key)
        {
            try
            {
                return IniFile.I_I.ReadIniInt(StrRecipeSection, key, Path);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                return 0;
            }
        }

        public void ShowInsertRecipeIni()
        {
            try
            {
                NoInsertRow.Text = IniFile.I_I.ReadIniStr(StrRecipeSection, Record_CSTRow, Path);
                NoInsertColumn.Text = IniFile.I_I.ReadIniStr(StrRecipeSection, Record_CSTColumn, Path);
                NoDisKeel.Text = IniFile.I_I.ReadIniStr(StrRecipeSection, Record_CSTDisKeel, Path);
                NoDisFirstKeel.Text = IniFile.I_I.ReadIniStr(StrRecipeSection, Record_CSTDisFirstKeel, Path);
                NoStepZ.Text = IniFile.I_I.ReadIniStr(StrRecipeSection, Record_CSTStepZ, Path);     
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        public void ShowInsertDataRegIni()
        {
            try
            {
                TextBoxReserve1.Text = ParInsertRegData.P_I.addr_insertdata_index1.ToString();
                TextBoxReserve2.Text = ParInsertRegData.P_I.addr_curinsertnum_index1.ToString();
                TextBoxReserve3.Text = ParInsertRegData.P_I.addr_confirm_index1.ToString();
                TextBoxReserve4.Text = ParInsertRegData.P_I.addr_curCSTNo_index1.ToString();
                TextBoxReserve5.Visibility = Visibility.Hidden;
                TextBoxReserve6.Visibility = Visibility.Hidden;

                TextBoxReserve7.Text = ParInsertRegData.P_I.addr_insertdata_index2.ToString();
                TextBoxReserve8.Text = ParInsertRegData.P_I.addr_curinsertnum_index2.ToString();
                TextBoxReserve9.Text = ParInsertRegData.P_I.addr_confirm_index2.ToString();
                TextBoxReserve10.Text = ParInsertRegData.P_I.addr_curCSTNo_index2.ToString();
                TextBoxReserve11.Visibility = Visibility.Hidden;
                TextBoxReserve12.Visibility = Visibility.Hidden;

                TextBoxReserve13.Text = ParInsertRegData.P_I.addr_insert1std_index1.ToString();
                TextBoxReserve14.Text = ParInsertRegData.P_I.addr_insert1comz_index1.ToString();
                TextBoxReserve15.Text = ParInsertRegData.P_I.addr_insert1stepz_index1.ToString();
                TextBoxReserve16.Text = ParInsertRegData.P_I.addr_insert2std_index1.ToString();
                TextBoxReserve17.Text = ParInsertRegData.P_I.addr_insert2comz_index1.ToString();
                TextBoxReserve18.Text = ParInsertRegData.P_I.addr_insert2stepz_index1.ToString();

                TextBoxReserve19.Text = ParInsertRegData.P_I.addr_insert1std_index2.ToString();
                TextBoxReserve20.Text = ParInsertRegData.P_I.addr_insert1comz_index2.ToString();
                TextBoxReserve21.Text = ParInsertRegData.P_I.addr_insert1stepz_index2.ToString();
                TextBoxReserve22.Text = ParInsertRegData.P_I.addr_insert2std_index2.ToString();
                TextBoxReserve23.Text = ParInsertRegData.P_I.addr_insert2comz_index2.ToString();
                TextBoxReserve24.Text = ParInsertRegData.P_I.addr_insert2stepz_index2.ToString();

                TextBoxReserve25.Text = ParInsertRegData.P_I.addr_insert3std_index1.ToString();
                TextBoxReserve26.Text = ParInsertRegData.P_I.addr_insert3comz_index1.ToString();
                TextBoxReserve27.Text = ParInsertRegData.P_I.addr_insert3stepz_index1.ToString();
                TextBoxReserve28.Text = ParInsertRegData.P_I.addr_insert4std_index1.ToString();
                TextBoxReserve29.Text = ParInsertRegData.P_I.addr_insert4comz_index1.ToString();
                TextBoxReserve30.Text = ParInsertRegData.P_I.addr_insert4stepz_index1.ToString();

                TextBoxReserve31.Text = ParInsertRegData.P_I.addr_insert3std_index2.ToString();
                TextBoxReserve32.Text = ParInsertRegData.P_I.addr_insert3comz_index2.ToString();
                TextBoxReserve33.Text = ParInsertRegData.P_I.addr_insert3stepz_index2.ToString();
                TextBoxReserve34.Text = ParInsertRegData.P_I.addr_insert4std_index2.ToString();
                TextBoxReserve35.Text = ParInsertRegData.P_I.addr_insert4comz_index2.ToString();
                TextBoxReserve36.Text = ParInsertRegData.P_I.addr_insert4stepz_index2.ToString();
            }
            catch(Exception ex)
            {
                WriteLog(ex);
            }
        }


        private static void ReadInsertRegDataIni()
        {
            try
            {
                ParInsertRegData.P_I.addr_insertdata_index1 = ReadInsertRegDataIniBaseInt(Record_InsertData_Index1);
                ParInsertRegData.P_I.addr_insertdata_index2 = ReadInsertRegDataIniBaseInt(Record_InsertData_Index2);
                ParInsertRegData.P_I.addr_curinsertnum_index1 = ReadInsertRegDataIniBaseInt(Record_CurInsertNum_Index1);
                ParInsertRegData.P_I.addr_curinsertnum_index2 = ReadInsertRegDataIniBaseInt(Record_CurInsertNum_Index2);
                ParInsertRegData.P_I.addr_confirm_index1 = ReadInsertRegDataIniBaseInt(Record_InsertConfirm_Index1);
                ParInsertRegData.P_I.addr_confirm_index2 = ReadInsertRegDataIniBaseInt(Record_InsertConfirm_Index2);
                ParInsertRegData.P_I.addr_curCSTNo_index1 = ReadInsertRegDataIniBaseInt(Record_CurStationNo_Index1);
                ParInsertRegData.P_I.addr_curCSTNo_index2 = ReadInsertRegDataIniBaseInt(Record_CurStationNo_Index2);

                ParInsertRegData.P_I.addr_insert1std_index1 = ReadInsertRegDataIniBaseInt(Record_CST1Std_Index1);
                ParInsertRegData.P_I.addr_insert1std_index2 = ReadInsertRegDataIniBaseInt(Record_CST1Std_Index2);
                ParInsertRegData.P_I.addr_insert1comz_index1 = ReadInsertRegDataIniBaseInt(Record_CST1Comz_Index1);
                ParInsertRegData.P_I.addr_insert1comz_index2 = ReadInsertRegDataIniBaseInt(Record_CST1Comz_Index2);
                ParInsertRegData.P_I.addr_insert1stepz_index1 = ReadInsertRegDataIniBaseInt(Record_CST1Stepz_Index1);
                ParInsertRegData.P_I.addr_insert1stepz_index2 = ReadInsertRegDataIniBaseInt(Record_CST1Stepz_Index2);

                ParInsertRegData.P_I.addr_insert2std_index1 = ReadInsertRegDataIniBaseInt(Record_CST2Std_Index1);
                ParInsertRegData.P_I.addr_insert2std_index2 = ReadInsertRegDataIniBaseInt(Record_CST2Std_Index2);
                ParInsertRegData.P_I.addr_insert2comz_index1 = ReadInsertRegDataIniBaseInt(Record_CST2Comz_Index1);
                ParInsertRegData.P_I.addr_insert2comz_index2 = ReadInsertRegDataIniBaseInt(Record_CST2Comz_Index2);
                ParInsertRegData.P_I.addr_insert2stepz_index1 = ReadInsertRegDataIniBaseInt(Record_CST2Stepz_Index1);
                ParInsertRegData.P_I.addr_insert2stepz_index2 = ReadInsertRegDataIniBaseInt(Record_CST2Stepz_Index2);

                ParInsertRegData.P_I.addr_insert3std_index1 = ReadInsertRegDataIniBaseInt(Record_CST3Std_Index1);
                ParInsertRegData.P_I.addr_insert3std_index2 = ReadInsertRegDataIniBaseInt(Record_CST3Std_Index2);
                ParInsertRegData.P_I.addr_insert3comz_index1 = ReadInsertRegDataIniBaseInt(Record_CST3Comz_Index1);
                ParInsertRegData.P_I.addr_insert3comz_index2 = ReadInsertRegDataIniBaseInt(Record_CST3Comz_Index2);
                ParInsertRegData.P_I.addr_insert3stepz_index1 = ReadInsertRegDataIniBaseInt(Record_CST3Stepz_Index1);
                ParInsertRegData.P_I.addr_insert3stepz_index2 = ReadInsertRegDataIniBaseInt(Record_CST3Stepz_Index2);

                ParInsertRegData.P_I.addr_insert4std_index1 = ReadInsertRegDataIniBaseInt(Record_CST4Std_Index1);
                ParInsertRegData.P_I.addr_insert4std_index2 = ReadInsertRegDataIniBaseInt(Record_CST4Std_Index2);
                ParInsertRegData.P_I.addr_insert4comz_index1 = ReadInsertRegDataIniBaseInt(Record_CST4Comz_Index1);
                ParInsertRegData.P_I.addr_insert4comz_index2 = ReadInsertRegDataIniBaseInt(Record_CST4Comz_Index2);
                ParInsertRegData.P_I.addr_insert4stepz_index1 = ReadInsertRegDataIniBaseInt(Record_CST4Stepz_Index1);
                ParInsertRegData.P_I.addr_insert4stepz_index2 = ReadInsertRegDataIniBaseInt(Record_CST4Stepz_Index2);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        private void RefreshPar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!(MessageBox.Show("是否确定采用常规协议?","确认信息",MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                {
                    return;
                }

                GiveRefreshValue();
                SaveParToMemory();
                SaveParToLocal();
                SaveOthers();

            }
            catch(Exception ex)
            {
                WriteLog(ex);
            }
        }
        private void GiveRefreshValue()
        {
            try
            {
                TextBoxNumCST.Text = "1";
                test1.IsChecked = true;
                test2.IsChecked = false;
                test3.IsChecked = true;
                test4.IsChecked = false;

                NoInsertRow.Text = "12";
                NoInsertColumn.Text = "13";
                NoDisKeel.Text = "14";
                NoDisFirstKeel.Text = "16";
                NoStepZ.Text = "15";

                TextBoxReserve1.Text = "3";
                TextBoxReserve2.Text = "1";
                TextBoxReserve3.Text = "1";
                TextBoxReserve4.Text = "0";
                TextBoxReserve5.Visibility = Visibility.Hidden;
                TextBoxReserve6.Visibility = Visibility.Hidden;

                TextBoxReserve7.Text = "1";
                TextBoxReserve8.Text = "7";
                TextBoxReserve9.Text = "11";
                TextBoxReserve10.Text = "0";
                TextBoxReserve11.Visibility = Visibility.Hidden;
                TextBoxReserve12.Visibility = Visibility.Hidden;

                TextBoxReserve13.Text = "2";
                TextBoxReserve14.Text = "3";
                TextBoxReserve15.Text = "3";
                TextBoxReserve16.Text = "2";
                TextBoxReserve17.Text = "3";
                TextBoxReserve18.Text = "3";

                TextBoxReserve19.Text = "0";
                TextBoxReserve20.Text = "2";
                TextBoxReserve21.Text = "8";
                TextBoxReserve22.Text = "1";
                TextBoxReserve23.Text = "14";
                TextBoxReserve24.Text = "20";

                TextBoxReserve25.Text = "2";
                TextBoxReserve26.Text = "3";
                TextBoxReserve27.Text = "3";
                TextBoxReserve28.Text = "2";
                TextBoxReserve29.Text = "3";
                TextBoxReserve30.Text = "3";

                TextBoxReserve31.Text = "0";
                TextBoxReserve32.Text = "26";
                TextBoxReserve33.Text = "32";
                TextBoxReserve34.Text = "3";
                TextBoxReserve35.Text = "38";
                TextBoxReserve36.Text = "44";

            }
            catch(Exception ex)
            {
                WriteLog(ex);
            }
        }

        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                folderBrowserDialog.SelectedPath = BaseDealInsert.c_PathCSTBase;
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    BaseDealInsert.c_PathCSTBase = (folderBrowserDialog.SelectedPath + "\\");
                }
                if (!Directory.Exists(PathRoot))
                {
                    Directory.CreateDirectory(PathRoot);
                }

                this.Dispatcher.Invoke(new Action(() => txtCSTPath.Text = BaseDealInsert.c_PathCSTBase));
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
    }
}
