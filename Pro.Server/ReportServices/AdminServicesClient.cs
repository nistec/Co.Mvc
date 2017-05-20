using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data;

namespace Pro.Server
{
   
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IAdminServices
    {
        [OperationContract]
        DataTable GetQueueStatistic(string server);

        [OperationContract]
        DataTable GetCurrentQueueStatistic();

        [OperationContract]
        string GetData(string value);
    }

    public class AdminServicesClient : IDisposable
    {

        #region ctor

        bool isWcf;
        //string m_channelName = "AdminServices_Current";

        public AdminServicesClient()
            : this("AdminServices_Current")
        {
         
        }

        public AdminServicesClient(string channelName)
        {
            isWcf = true;
            string m_channelName = string.IsNullOrEmpty(channelName) ? "AdminServices_Current" : channelName;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(1033);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(1033);

            //Console.WriteLine("Press any key when the service is available.");
            //Console.ReadKey();

            factory = new ChannelFactory<IAdminServices>(m_channelName);
            //InvokeService(true, 1);
            //InvokeServiceTest(true, 10);
        }
        public void Dispose()
        {
            Close();
        }
        #endregion

        #region proxy
              

        IAdminServices m_proxy;
        private static ChannelFactory<IAdminServices> factory;

        public IAdminServices Proxy
        {
            get
            {
                if (m_proxy == null)
                {
                    //IMailerService proxy = null;
                    if (isWcf)
                        m_proxy = factory.CreateChannel();
                    else
                        m_proxy = (IAdminServices)Activator.GetObject(typeof(IAdminServices), "tcp://localhost:63099/ServiceMailerRemoting.rem");
                }
                return m_proxy;
            }
        }

        public void Close()
        {
            try
            {
                if (isWcf && m_proxy != null)
                {
                    if (((System.ServiceModel.Channels.IChannel)m_proxy).State == CommunicationState.Opened)
                        ((System.ServiceModel.Channels.IChannel)m_proxy).Close();
                    if (factory != null && factory.State == CommunicationState.Opened)
                    {
                        factory.Close();
                    }
                }
            }
            catch { }
        }
        #endregion

        #region Methods

        static string sample()
        {
            using (AdminServicesClient client = new AdminServicesClient())
            {
               return client.Proxy.GetData("test");
            }
        }

        //public void InvokeItem(QueueItem message)
        //{
        //    InvokeService(message, true);
        //}

        //private static decimal InvokeService(bool wcf, int times)
        //{
        //    Stopwatch s = new Stopwatch();
        //    s.Start();
        //    MessageQueue message = new MessageQueue();
        //    message.QueueName = "";
        //    message.SerilaizedValue = CreateMessageItem().Serialize();
        //    for (int i = 0; i < times; i++)
        //    {

        //        InvokeServiceMethod(wcf, message);
        //        //GC.Collect();
        //    }

        //    s.Stop();

        //    return s.ElapsedMilliseconds;
        //}


        #endregion

        /*
        private static void InvokeService(QueueItem message, bool wcf)
        {
            IAdminServices proxy = null;
            if (wcf)
                proxy = factory.CreateChannel();
            else
                proxy = (IAdminServices)Activator.GetObject(typeof(IAdminServices), "tcp://localhost:63099/ServiceMailerRemoting.rem");

            bool ok = proxy.EnqueueItem(message);

            Console.WriteLine("Result:" + ok.ToString());

            if (wcf)
                ((System.ServiceModel.Channels.IChannel)proxy).Close();
        }

        private static void InvokeServiceTest(string message, bool wcf)
        {
            IAdminServices proxy = null;
            if (wcf)
                proxy = factory.CreateChannel();
            else
                proxy = (IAdminServices)Activator.GetObject(typeof(IAdminServices), "tcp://localhost:63099/ServiceMailerRemoting.rem");

            string res = proxy.GetData(message);

            Console.WriteLine("Result:" + res);

            if (wcf)
                ((System.ServiceModel.Channels.IChannel)proxy).Close();
        }
        */
    }
}
