using BasicClass;
using DealFile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace ModulePackage
{
    public class ConfigManager : INotifyPropertyChanged
    {
        public static ConfigManager instance = new ConfigManager();
        const string ClassName = "ConfigManager";
        const string commonSection = "ConifgManager";
        string Path_Dir => ParPathRoot.PathRoot + @"Store\Custom\Module\";
        string Path_Config => Path_Dir + "config.ini";
        #region variables
        /// <summary>
        /// 机器人放置方向
        /// </summary>
        public DirBot_Enum DirBotEnum { get { return _dirBotEnum; } set { _dirBotEnum = value; NotifyPropertyChanged("DirBotEnum"); } }
        DirBot_Enum _dirBotEnum = DirBot_Enum.Forward;
        /// <summary>
        /// 画面显示方向
        /// </summary>
        public DirDisplay_Enum DirDisplayEnum { get { return _dirDisplayEnum; } set { _dirDisplayEnum = value; NotifyPropertyChanged("DirDisplayEnum"); } }
        DirDisplay_Enum _dirDisplayEnum = DirDisplay_Enum.正常;
        /// <summary>
        /// 画面X是否镜像
        /// </summary>
        public bool IsMirrorX { get { return _isMirrorX; } set { _isMirrorX = value; NotifyPropertyChanged("IsMirrorX"); } }
        bool _isMirrorX = true;
        /// <summary>
        /// 画面Y是否镜像
        /// </summary>
        public bool IsMirrorY { get { return _isMirrorY; } set { _isMirrorY = value; NotifyPropertyChanged("IsMirrorY"); } }
        public bool _isMirrorY = true;
        /// <summary>
        /// 背光放置方向
        /// </summary>
        public DirBL_Enum DirBLEnum { get { return _dirBLEnum; } set { _dirBLEnum = value; NotifyPropertyChanged("DirBLEnum"); } }
        DirBL_Enum _dirBLEnum = DirBL_Enum.正常;
        /// <summary>
        /// 工位1平台放片位置
        /// </summary>
        public PlatformPlacePos_Enum PlatformPlacePosEnum { get { return _platformPlacePosEnum; }set { _platformPlacePosEnum = value;NotifyPropertyChanged("PlatformPlacePosEnum"); } }
        PlatformPlacePos_Enum _platformPlacePosEnum = PlatformPlacePos_Enum.LeftTop;
        /// <summary>
        /// 单电极玻璃在平台1放置时，电极的朝向
        /// </summary>
        public bool IsHorizontal { get { return _isHorizontal; }set { _isHorizontal = value;NotifyPropertyChanged("IsHorizontal"); } }
        bool _isHorizontal = true;
        /// <summary>
        /// 卡塞相机安装方向
        /// </summary>
        public DirCstCamera_Enum DirCstCameraEnum { get { return _dirCstCameraEnum; } set { _dirCstCameraEnum = value; NotifyPropertyChanged("DirCstCameraEnum"); } }
        DirCstCamera_Enum _dirCstCameraEnum = DirCstCamera_Enum.Backward;
        /// <summary>
        /// 卡塞插栏方向
        /// </summary>
        public DirInsert_Enum DirInsertEnum { get { return _dirInsertEnum; } set { _dirInsertEnum = value; NotifyPropertyChanged("DirInsertEnum"); } }
        DirInsert_Enum _dirInsertEnum = DirInsert_Enum.NToP;
        /// <summary>
        /// z轴补偿所使用的机构种类
        /// </summary>
        public TypeModuleZ_Enum TypeModuleZEnum { get { return _typeModuleZEnum; } set { _typeModuleZEnum = value; NotifyPropertyChanged("TypeZEnum"); } }
        TypeModuleZ_Enum _typeModuleZEnum = TypeModuleZ_Enum.ModuleUp;
        /// <summary>
        /// 画面X是否镜像
        /// </summary>
        public bool CstIsMirrorX { get { return _cstIsMirrorX; } set { _cstIsMirrorX = value; NotifyPropertyChanged("CstIsMirrorX"); } }
        bool _cstIsMirrorX = true;

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region 读写接口
        /// <summary>
        /// 从配置文件读取保存在本地的配置
        /// </summary>
        public void LoadParams()
        {
            try
            {
                DirBotEnum = (DirBot_Enum)Enum.Parse(typeof(DirBot_Enum), GetConfig(ConfigParams.DirBotEnum.ToString()));
                DirDisplayEnum = (DirDisplay_Enum)Enum.Parse(typeof(DirDisplay_Enum), GetConfig(ConfigParams.DirDisplayEnum.ToString()));
                IsMirrorX = Boolean.Parse(GetConfig(ConfigParams.IsMirrorX.ToString()));
                IsMirrorY = Boolean.Parse(GetConfig(ConfigParams.IsMirrorY.ToString()));
                DirBLEnum = (DirBL_Enum)Enum.Parse(typeof(DirBL_Enum), GetConfig(ConfigParams.DirBLEnum.ToString()));
                PlatformPlacePosEnum = (PlatformPlacePos_Enum)Enum.Parse(typeof(PlatformPlacePos_Enum), GetConfig(ConfigParams.PlatformPlacePosEnum.ToString()));
                IsHorizontal = Boolean.Parse(GetConfig(ConfigParams.IsHorizontal.ToString()));
                DirCstCameraEnum = (DirCstCamera_Enum)Enum.Parse(typeof(DirCstCamera_Enum), GetConfig(ConfigParams.DirCstCameraEnum.ToString()));
                DirInsertEnum = (DirInsert_Enum)Enum.Parse(typeof(DirInsert_Enum), GetConfig(ConfigParams.DirInsertEnum.ToString()));
                TypeModuleZEnum = (TypeModuleZ_Enum)Enum.Parse(typeof(TypeModuleZ_Enum), GetConfig(ConfigParams.TypeModuleZEnum.ToString()));
                CstIsMirrorX = Boolean.Parse(GetConfig(ConfigParams.CstIsMirrorX.ToString()));
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
        }

        /// <summary>
        /// 封装个接口，少写几个变量，方便统一改动
        /// </summary>
        /// <param name="key">需要读取的变量</param>
        /// <param name="section">部分参数根据mode保存，也要根据mode读取</param>
        /// <returns></returns>
        private string GetConfig(string key, string section = commonSection)
        {
            try
            {
                return IniFile.I_I.ReadIniStr(section, key, "", Path_Config);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 将所有参数写入到ini，部分以mode为section保存，其余是公共参数
        /// </summary>
        public void WriteConfig()
        {
            if (!Directory.Exists(Path_Dir))
                Directory.CreateDirectory(Path_Dir);
            WriteConfig(ConfigParams.DirBotEnum.ToString(), DirBotEnum.ToString());
            WriteConfig(ConfigParams.DirDisplayEnum.ToString(), DirDisplayEnum.ToString());
            WriteConfig(ConfigParams.IsMirrorX.ToString(), IsMirrorX.ToString());
            WriteConfig(ConfigParams.IsMirrorY.ToString(), IsMirrorY.ToString());
            WriteConfig(ConfigParams.DirBLEnum.ToString(), DirBLEnum.ToString());
            WriteConfig(ConfigParams.PlatformPlacePosEnum.ToString(), PlatformPlacePosEnum.ToString());
            WriteConfig(ConfigParams.IsHorizontal.ToString(), IsHorizontal.ToString());
            WriteConfig(ConfigParams.DirCstCameraEnum.ToString(), DirCstCameraEnum.ToString());
            WriteConfig(ConfigParams.DirInsertEnum.ToString(), DirInsertEnum.ToString());
            WriteConfig(ConfigParams.TypeModuleZEnum.ToString(), TypeModuleZEnum.ToString());
            WriteConfig(ConfigParams.CstIsMirrorX.ToString(), CstIsMirrorX.ToString());
        }

        /// <summary>
        /// 将CIM参数记录到本地
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void WriteConfig(string key, string value, string section = commonSection)
        {
            try
            {
                IniFile.I_I.WriteIni(section, key, value, Path_Config);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
        }
        #endregion
    }
}
