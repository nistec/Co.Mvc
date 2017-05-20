using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Nistec.Generic;

namespace Pro.Server
{

    public enum ServerType
    {
        Master,
        MailServer,
        DbServer,
        AppServer
    }

    public enum ServerStatus
    {
        Enabled,
        Disabled
    }

    public class AdminServer
    {
        public string ServerName;
        public string ServerIP;
        public ServerType ServerType;
        public ServerStatus ServerStatus = ServerStatus.Enabled;

        public string ServiceName
        {
            get { return "AdminServices_" + ServerName; }
        }

        public AdminServer(string name, string ip,ServerType type)
        {
            ServerName = name;
            ServerIP = ip;
            ServerType = type;
        }

        public static AdminServer[] Create()
        {
            List<AdminServer> servers = new List<AdminServer>();
            servers.Add(new AdminServer("myt-srv", "62.219.21.26", ServerType.Master));
            servers.Add(new AdminServer("myt-srv02", "62.219.21.29", ServerType.MailServer));
            servers.Add(new AdminServer("myt-srv03", "62.219.21.58", ServerType.DbServer));

            return servers.ToArray();
        }

        public string Print()
        {
            return string.Format("ServerName:{0}, ServerIP:{1}, ServerType:{2}", ServerName, ServerIP, ServerType.ToString());
        }

        public string[] GetReports()
        {
            List<string> reports = new List<string>();
            reports.Add("GetQueueStatistic");
            reports.Add("GetCurrentQueueStatistic");

            return reports.ToArray();
        }

        public DataTable DoReportAsync(string name, string args)
        {
            GenericTasker<DataTable> ta = new GenericTasker<DataTable>();
            //ta.TaskCompleted += new EventHandler(ta_TaskCompleted);
            try
            {
                return ta.Execute(() => DoReport(name, args), 60);

                Console.WriteLine("Result");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Task error:" + ex.Message);

                return null;
            }
        }

        public DataTable DoReport(string name, string args)
        {
            //try
            //{
                switch (name)
                {
                    case "GetQueueStatistic":
                        using (AdminServicesClient client = new AdminServicesClient(ServiceName))
                        {
                            return client.Proxy.GetQueueStatistic(args);
                        }
                    case "GetCurrentQueueStatistic":

                        using (AdminServicesClient client = new AdminServicesClient(ServiceName))
                        {
                            return client.Proxy.GetCurrentQueueStatistic();
                        }
                    default:
                        return null;
                }
            //}
            //catch { return null; }
        }

    }
}
