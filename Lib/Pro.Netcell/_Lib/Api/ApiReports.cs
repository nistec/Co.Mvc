using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Netcell.Data.Reports;
using Nistec.Data;
using MControl.;

namespace Netcell.Lib
{
    public class ApiReports
    {

        public static bool IsEndWith(string s, string[] args)
        {
            foreach (string arg in args)
            {
                if (s.EndsWith(arg))
                    return true;
            }
            return false;
        }
  

        public static DataTable Trans_Item_View_Table(int MessageId)
        {
            DataTable dt = null;
            using (DalReports dal = new DalReports())
            {
                dt = dal.Trans_Item_View_Table(MessageId);
            }
            if (dt != null && dt.Rows.Count>0)
            {
                DataRow dr=dt.Rows[0];
                string body=dr.Get<string>("MessageView");
                string display=dr.Get<string>("PersonalDisplay");
                string personal=dr.Get<string>("Personal");
                string message = Remoting.RemoteUtil.BuildMessage(body, personal, display);

                dr["MessageView"] = message;
            }

            return dt;
        }

        public static string Trans_Item_View_Message(int MessageId)
        {
            string message = "";
            DataRow dr = null;
            using (DalReports dal = new DalReports())
            {
                dr = dal.Trans_Item_View_Row(MessageId);
            }
            if (dr != null)
            {
                string body = dr.Get<string>("MessageView");
                string display = dr.Get<string>("PersonalDisplay");
                string personal = dr.Get<string>("Personal");
                message = Remoting.RemoteUtil.BuildMessage(body, personal, display);
            }

            return message;
        }

        
    }
}
