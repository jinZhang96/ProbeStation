using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DealCIM
{
    public partial class PostHelper
    {
        // public static PostHelper instance = new PostHelper();

        public static string PostChipID(string chipid, string modelno, out string key)
        {
            string data = XMLHelpler.GetChipIDData(chipid, modelno);
            
            return CIM.C_I.WriteData(data, out key);
        }

        public static string PostLot(string modelno, out string key)
        {
            string data = XMLHelpler.GetLotData(modelno);

            return CIM.C_I.WriteData(data, out key);
        }

        public static string PostTrackOut(List<string> list,string modelno, out string key)
        {
            string data = XMLHelpler.GetTrackOutData(list, modelno);

            return CIM.C_I.WriteData(data, out key);
        }
    }

    public struct PostInfo
    {
        public string correlationID;
        public PostType type;
    }
}
