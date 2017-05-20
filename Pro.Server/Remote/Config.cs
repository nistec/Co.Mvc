using System;
using System.Collections.Generic;
using System.Text;
using Nistec;

namespace Pro.Server
{
    public class ConfigSrv
    {
        public const int DefaultIntervalSetting = 60000;

        public static int AdminIntervalSetting = DefaultIntervalSetting;
        public static int AdminMaxThread = 1;
        public static int Server = 0;
        public static string ProcessFolder;
        public static bool Enable_Scheduler_Commands=true;
        public static string ClientAuth;
        static ConfigSrv()
        {
            System.Collections.Specialized.NameValueCollection settings = System.Configuration.ConfigurationManager.AppSettings;
            AdminIntervalSetting = Types.ToInt(settings["AdminIntervalSetting"], 60000);
            AdminMaxThread = Types.ToInt(settings["AdminMaxThread"], 1);
            Server = Types.ToInt(settings["Server"], 0);
            ProcessFolder = Types.NZ(settings["ProcessFolder"], "c:\\Nistec");
            Enable_Scheduler_Commands = Types.ToBool(settings["Enable_Scheduler_Commands"], false);
            ClientAuth = Types.NZ(settings["ClientAuth"], "");
        }

     }
}
