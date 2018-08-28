using System;
using System.Data;
using System.ComponentModel;
using Nistec.Data;
using Netcell.Data;
using System.Collections.Generic;
using Nistec;
using System.Collections;
using Nistec.Collections;
using Netcell.Data.Server;


namespace Netcell.Remoting
{
    public enum ServerMode
    {
        Default=0,
        SingleMode=1,
        NoActive=5
    }

    public class AppServers : CircleCollection<IAppServer> //Dictionary<int, IAppServer>
    {
        public readonly int CurrentServer = 0;

        public static int NextServer(PlatformType platform)
        {
            AppServers app = new AppServers(platform);
            return app.NextServerId();
        }

        public AppServers()
        {
            CurrentServer = ViewConfig.Server;// serverId;
            DataTable dt = DalRule.Instance.AppActiveServers();
            LoadServers(dt);
        }

        public AppServers(PlatformType platform)
        {
            CurrentServer = ViewConfig.Server;// serverId;
            DataTable dt = DalRule.Instance.AppActiveServers((int)platform);
            LoadServers(dt);
        }

        public void LoadServers(DataTable dt)
        {
            this.Clear();
            List<IAppServer> servers = new List<IAppServer>();
            bool isSingleMode = false;

            //check if it in single mode
            foreach (DataRow dr in dt.Rows)
            {
                IAppServer server = new AppServer(dr);

                ServerMode mode =(ServerMode) server.ServerMode;
                switch (mode)
                {
                    case ServerMode.SingleMode:
                        if (server.ServerId == CurrentServer)
                        {
                            isSingleMode = true;
                            this.Add(server);
                        }
                        break;
                    default:
                        servers.Add(server);
                        break;
                }
                if (isSingleMode)
                    break;
                // this.Add(server);
            }

            if (!isSingleMode)
            {

                foreach (IAppServer server in servers)
                {
                    this.Add(server);
                }
            }

            if (Count == 0)
            {
                throw new MsgException(AckStatus.ConfigurationException, "no such server found");
            }
        }

        public void LoadServers(AppServer[] servers)
        {
            this.Clear();
            foreach (IAppServer server in servers)
            {
                this.Add(server);
            }
            if (Count == 0)
            {
                throw new MsgException(AckStatus.ConfigurationException, "no such server found");
            }
        }

        public int NextServerId()
        {
            return Next().ServerId;
        }

        //public int NextServerId(PlatformType platform,int count)
        //{
        //    return Next().ServerId;
        //}


        //public static AppServers LoadServers(PlatformType platform)
        //{
        //    AppServers allservers = new AppServers();
        //    AppServers appservers = new AppServers();

        //    allservers.LoadServers();

        //    foreach (IAppServer server in allservers.Values)
        //    {

        //        if (platform == PlatformType.Cell && server.EnableCellBatch)
        //        {
        //            appservers.Add(server.ServerId, server);
        //        }
        //        else if (platform == PlatformType.Mail && server.EnableMailBatch)
        //        {
        //            appservers.Add(server.ServerId, server);
        //        }

        //    }
        //    return appservers;

        //}

       

        //public static AppServers GetServers(PlatformType  platform)
        //{
        //    string servers = ViewConfig.App_Servers;

        //    AppServers appservers = new AppServers();
           
        //    //0,1,1:2,0,0

        //    string[] srv = servers.Split(':');
            
        //    foreach (string server in srv)
        //    {
        //        string[] args = servers.Split(',');
        //        int srvId = Types.ToInt(args[0].Trim(),0);
        //        bool enableMail = Types.ToInt(args[1].Trim(), 1)>0;
        //        bool enableCell = Types.ToInt(args[2].Trim(), 1)>0;

        //        if (platform == PlatformType.Cell && enableCell)
        //        {
        //            appservers.Add(srvId, new AppServer(srvId, enableMail, enableCell));
        //        }
        //        else if (platform == PlatformType.Mail && enableMail)
        //        {
        //            appservers.Add(srvId, new AppServer(srvId, enableMail, enableCell));
        //        }

        //    }
        //    return appservers;
        //}
    }

    public interface IAppServer
    {
        int ServerId { get; }
        //string ServerAddress { get; }
        string ServerName { get; }
        int EnablePlatform { get; }
        bool EnableMailBatch { get; }
        bool EnableCellBatch { get; }
        bool EnableMailSender { get; }
        bool EnableCellSender { get; }
        bool EnablePending { get; }
        bool EnableScheduler { get; }
        bool EnableAlerts { get; }
        int ServerMode { get; }
        bool EnableQueue { get; }
        //void Dispose();
    }

    public class AppServer : IAppServer
    {

        #region members and ctor
        //int id;
        ////string serverAddress;
        //string serverName;
        //bool enableMailBatch;
        //bool enableCellBatch;
        //bool enablePending;
        //bool enableScheduler;
        //bool enableMailSender;
        //bool enableCellSender;
        //int enablePlatform;
        //int serverMode;

        //bool isActive;
        //int mailCapacity;
        //int cellCapacity;
        int mailQueue;
        int cellQueue;

       
        public AppServer(int serverid, bool enableMailBatch, bool enableCellBatch)
        {
            this.ServerId = serverid;
            this.EnableMailBatch = enableMailBatch;
            this.EnableCellBatch = enableCellBatch;
        }

        public AppServer(DataRow dr)
        {
            this.ServerId = Types.ToInt(dr["ServerId"], 0);
            //this.serverAddress = Types.NZ(dr["ServerAddress"], "");
            this.ServerName = Types.NZ(dr["ServerName"], "");
            this.EnableMailBatch = Types.ToBool(dr["EnableMailBatch"], false);
            this.EnableCellBatch = Types.ToBool(dr["EnableCellBatch"], false);
            this.EnablePending = Types.ToBool(dr["EnablePending"], false);
            this.EnableScheduler = Types.ToBool(dr["EnableScheduler"], false);
            this.EnableMailSender = Types.ToBool(dr["EnableMailSender"], false);
            this.EnableCellSender = Types.ToBool(dr["EnableCellSender"], false);
            this.EnablePlatform = Types.ToInt(dr["EnablePlatform"], 0);
            this.EnableAlerts = Types.ToBool(dr["EnableAlerts"], false);
           
            this.IsActive = Types.ToBool(dr["IsActive"], false);
            this.MailCapacity = Types.ToInt(dr["MailCapacity"], 0);
            this.CellCapacity = Types.ToInt(dr["CellCapacity"], 0);
            this.ServerMode = Types.ToInt(dr["ServerMode"], 0);

            this.mailQueue = Types.ToInt(dr["MailQueue"], 0);
            this.cellQueue = Types.ToInt(dr["CellQueue"], 0);

            this.EnableQueue = Types.ToBool(dr["EnableQueue"], false);

        }

        public AppServer(IDictionary dr)
        {
            this.ServerId = Types.ToInt(dr["ServerId"], 0);
            //this.serverAddress = Types.NZ(dr["ServerAddress"], "");
            this.ServerName = Types.NZ(dr["ServerName"], "");
            this.EnableMailBatch = Types.ToBool(dr["EnableMailBatch"], false);
            this.EnableCellBatch = Types.ToBool(dr["EnableCellBatch"], false);
            this.EnablePending = Types.ToBool(dr["EnablePending"], false);
            this.EnableScheduler = Types.ToBool(dr["EnableScheduler"], false);
            this.EnableMailSender = Types.ToBool(dr["EnableMailSender"], false);
            this.EnableCellSender = Types.ToBool(dr["EnableCellSender"], false);
            this.EnablePlatform = Types.ToInt(dr["EnablePlatform"], 0);
            this.EnableAlerts = Types.ToBool(dr["EnableAlerts"], false);

            this.IsActive = Types.ToBool(dr["IsActive"], false);
            this.MailCapacity = Types.ToInt(dr["MailCapacity"], 0);
            this.CellCapacity = Types.ToInt(dr["CellCapacity"], 0);
            this.ServerMode = Types.ToInt(dr["ServerMode"], 0);
            this.mailQueue = Types.ToInt(dr["MailQueue"], 0);
            this.cellQueue = Types.ToInt(dr["CellQueue"], 0);
            this.EnableQueue = Types.ToBool(dr["EnableQueue"], false);
        }

        //public void Dispose()
        //{
        //    serverAddress = null;
        //    serverName = null;
        //}

        #endregion

        #region properties

        public int ServerId
        {
            get;
            set;
        }
        public int EnablePlatform
        {
            get;
            set;
        }
        public int ServerMode
        {
            get;
            set;
        }

        public string ServerName
        {
            get;
            set;
        }

        public bool EnableMailBatch
        {
            get;
            set;
        }

        public bool EnableCellBatch
        {
            get;
            set;
        }
        public bool EnableMailSender
        {
            get;
            set;
        }

        public bool EnableCellSender
        {
            get;
            set;
        }
        public bool EnablePending
        {
            get;
            set;
        }

        public bool EnableScheduler
        {
            get;
            set;
        }
        public bool EnableQueue
        {
            get;
            set;
        }
        public bool EnableAlerts
        {
            get;
            set;
        }
        public bool IsActive
        {
            get;
            set;
        }
        public int MailCapacity
        {
            get;
            set;
        }

        public int CellCapacity
        {
            get;
            set;
        }

        #endregion

        #region static

        const string TableName = "App_Servers";
        const string FilterFormat = "ServerId={0}";

        static string Filter
        {
            get { return string.Format(FilterFormat, ViewConfig.Server); }
        }


        //public static AppServer GetAppServer()
        //{
        //    IDictionary dr= Nistec.Caching.Remote.RemoteDataClient.RemoteClient.GetRow(TableName, Filter);
        //    return new AppServer(dr);
        //}

      
        #endregion
    }
}
