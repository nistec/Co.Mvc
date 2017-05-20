using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Data;
using System.Globalization;
using Nistec.Data.Factory;
using Nistec.Data.Entities.Cache;
using Nistec.Generic;
using ProSystem.Data.Entities;
using System.Data;
using System.Web;

namespace ProSystem.Data
{

    public class ProCommands
    {
        public static string ViewLog()
        {
            using (var db = DbContext.Create<DbSystem>())
            return db.QueryJson("Select top 1000 * from Log order by logid desc");
        }
         
        //public static int Log(string Action,string folder, string LogText, string Client)
        //{
        //    return db.ExecuteCommand("insert into Log(Action,LogText,Client,Folder) values(@Action,@LogText,@Client)", CommandType.Text, "Action", Action, "LogText", LogText, "Client", Client);
        //}
        // public static int Log(string folder,string Action, string LogText, HttpRequestBase request, int LogType=0)
        //{
        //    string referrer = "";
        //    string clientIp = "";
        //    if (request != null)
        //    {
        //        clientIp = request.UserHostAddress;
        //        if (request.UrlReferrer != null)
        //            referrer = request.UrlReferrer.AbsoluteUri;
        //    }
        //    return db.ExecuteNonQuery("sp_Log", "Folder", folder, "Action", Action, "LogText", LogText, "Client", clientIp, "Referrer", referrer, "LogType", LogType);
        //}
        // public static int Log(string folder, string Action, string LogText, HttpRequest request, int LogType = 0)
        // {
        //     string referrer = "";
        //     string clientIp = "";
        //     if (request != null)
        //     {
        //         clientIp = request.UserHostAddress;
        //         if (request.UrlReferrer != null)
        //             referrer = request.UrlReferrer.AbsoluteUri;
        //     }
        //     return db.ExecuteNonQuery("sp_Log", "Folder", folder, "Action", Action, "LogText", LogText, "Client", clientIp, "Referrer", referrer, "LogType", LogType);
        // }
      
    }

}
