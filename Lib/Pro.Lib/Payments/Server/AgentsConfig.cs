using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;
using Nistec;

namespace Pro.Lib.Payments.Server
{

    public class AgentsConfig
    {
        public const int DefaultIntervalSetting = 6000;
        public const string DefaultQueueName = "Controller";

        //public static int Server = 0;

        public static int MaxQuickCount = 10;
        
        public static int SchedulerInterval = 6000;
        public static int SchedulerMaxThread = 5;
        public static bool SchedulerEnabled = true;
        public static int SchedulerMaxConnection = 30;
        public static int SchedulerMode = 0;


        public static string QueueName = DefaultQueueName;
        public static int DbQueueInterval = 60000;
        public static int DbQueueMaxThread = 1;
        public static bool DbQueueEnabled = true;
        public static int DbQueueMaxConnection = 10;
        public static int DbQueueMode = 0;

        //public static int MessageQueueInterval = 1000;
        //public static int FileQueueInterval = 60000;
        //public static int MessageQueueMaxThread = 1;
        public static bool MessageQueueEnabled = true;
        //public static int QueueCoverMode = 5;// None = 0, FileSystem = 5

#if(PENDING)

        public static bool EnablePendingCB = true;
        //public static bool EnablePendingRB = true;
        public static bool EnableCampaigns = false;
        public static bool EnableCampaignsMail = false;
        //public static bool EnableNotofication = true;
        public static bool EnableWatch = true;
        public static bool EnableQueueReminder = false;
        public static bool EnableFixed = false;
        public static bool EnableAlerts = false;
#endif

        //public static bool EnablePending = true;
        //public const int PendingInterval = 60000;
        //public const int PendingMaxThread = 1;
        //public const int PendingMaxConnection = 10;

        //========================================

        public static int Server = 0;

        public static int IntervalNotofication = 60000;
        public static int NotoficationMaxThread = 1;
        public static bool EnableNotofication = true;
        public static int NotifyAddDays = 0;

        //========================================

        
        public static bool EnableController = true;
        //public static bool EnableSender = true;
        //public static bool EnableMailsender = true;
        
        public static bool EnableRemote = true;
        public static int TcpPort = 9005;
        public static string PortName = "portRemoteSync";

        public const int RemoteMaxListnerThread = 1;
        
        //public static bool EnableAdmin = true;

        static AgentsConfig()
        {
            System.Collections.Specialized.NameValueCollection settings = System.Configuration.ConfigurationManager.AppSettings;

            //Server = Types.ToInt(settings["Server"], 0);

            MaxQuickCount = Types.ToInt(settings["MaxQuickCount"], 10);

            SchedulerInterval = Types.ToInt(settings["SchedulerInterval"], DefaultIntervalSetting);
            SchedulerMaxThread = Types.ToInt(settings["SchedulerMaxThread"], 5);
            SchedulerEnabled = Types.ToBool(settings["SchedulerEnabled"], true);
            SchedulerMaxConnection = Types.ToInt(settings["SchedulerMaxConnection"], 30);
            SchedulerMode = Types.ToInt(settings["SchedulerMode"], 0);
            
            QueueName = Types.NZ(settings["QueueName"], DefaultQueueName);
            DbQueueInterval = Types.ToInt(settings["DbQueueInterval"], DefaultIntervalSetting);
            DbQueueMaxThread = Types.ToInt(settings["DbQueueMaxThread"], 1);
            DbQueueEnabled = Types.ToBool(settings["DbQueueEnabled"], true);
            DbQueueMaxConnection = Types.ToInt(settings["DbQueueMaxConnection"], 10);
            DbQueueMode = Types.ToInt(settings["DbQueueMode"], 0);

            //MessageQueueInterval = Types.ToInt(settings["MessageQueueInterval"], 1000);
            //FileQueueInterval = Types.ToInt(settings["FileQueueInterval"], 60000);
            //MessageQueueMaxThread = Types.ToInt(settings["MessageQueueMaxThread"], 1);
            MessageQueueEnabled = Types.ToBool(settings["MessageQueueEnabled"], true);
            //QueueCoverMode = Types.ToInt(settings["QueueCoverMode"], 0);


#if(PENDING)
            EnableCampaigns = Types.ToBool(settings["EnableCampaigns"], false);
            EnableCampaignsMail = Types.ToBool(settings["EnableCampaignsMail"], false);

            EnablePendingCB = Types.ToBool(settings["EnablePendingCB"], true);
            EnableWatch = Types.ToBool(settings["EnableWatch"], true);
            EnableQueueReminder = Types.ToBool(settings["EnableQueueReminder"], true);
            EnableFixed = Types.ToBool(settings["EnableFixed"], true);
            EnableAlerts = Types.ToBool(settings["EnableAlerts"], true);
#endif            
            

            //======================================================

            Server = Types.ToInt(settings["Server"], 0);

            EnableRemote = Types.ToBool(settings["EnableRemote"], true);
            TcpPort = Types.ToInt(settings["TcpPort"], 9005);
            PortName = Types.NZ(settings["PortName"],"portRemoteSync");
            
            IntervalNotofication = Types.ToInt(settings["IntervalNotofication"], DefaultIntervalSetting);
            NotoficationMaxThread = Types.ToInt(settings["NotoficationMaxThread"], 1);
            EnableNotofication = Types.ToBool(settings["EnableNotofication"], true);
            NotifyAddDays = Types.ToInt(settings["NotifyAddDays"], 0);

            //======================================================

            EnableController = Types.ToBool(settings["EnableController"], true);
            
            //EnablePending = Types.ToBool(settings["EnablePending"], true);

            //EnableSender = Types.ToBool(settings["EnableSender"], true);
            //EnableMailsender = Types.ToBool(settings["EnableMailsender"], true);
            
            //EnableAdmin = Types.ToBool(settings["EnableAdmin"], true);

            //======================================================

            //MappingSection sec = new MappingSection();
            //VinsList.Add(sec.MappingElements);
        }

    }

}
