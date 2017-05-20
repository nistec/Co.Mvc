using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Pro.Server
{
    
    public class AdminServicesServer : IAdminServices
    {

        public string GetData(string value)
        {
            return string.Format("You entered: {0}", value);
        }

        public DataTable GetCurrentQueueStatistic()
        {
            DataTable dt = null; 
            try
            {
                dt = Nistec.Messaging.RemoteManager.GetStatistic();
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("AdminServices GetCurrentQueueStatistic Error: {0}", ex.Message);
            }
            return dt;
        }

        public DataTable GetQueueStatistic(string server)
        {
            DataTable dt = null;
            try
            {
                using (AdminServicesClient client = new AdminServicesClient(server))
                {
                    dt = client.Proxy.GetCurrentQueueStatistic();
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("AdminServices GetQueueStatistic Error: {0}", ex.Message);
            }
            return dt;

        }
    }
}
