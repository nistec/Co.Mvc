using System;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Nistec.Data.Entities;
using Netcell.Data.Db;
using Nistec.Logging;
using Nistec;
using Nistec.Web;

namespace Netcell.Remoting
{

    public static class RemoteSystemAlerts
    {

        #region SystemAlerts

        public static void ExecuteSystemAlert(int actionType, int status, int accountId, string message)
        {
            ActiveSystemAlert.AsyncAlertAction(actionType, (AckStatus)status, accountId, message);
        }

        public static void ExecuteSystemAlert(int actionType, int status, int accountId, string message, int priority, string method)
        {
            ExecuteTrace_Exception(message, priority, method, status, accountId);
            ActiveSystemAlert.AsyncAlertAction(actionType, (AckStatus)status, accountId, message);
        }

        public static void ExecuteTrace_Exception(string message, int priority, string method, int status, int accountId)
        {
            using (DalTrace dal = new DalTrace())
            {
                dal.Exceptions_Insert(message, priority, method, status, accountId);
            }
        }

        #endregion
    }

    /// <summary>
    /// Summary description for ActiveAlertException.
    /// </summary>
    public class ActiveSystemAlert : EntityView
	{

        

        static readonly SyncAlerts Sync = new SyncAlerts();
        static ActiveSystemAlert Active=new ActiveSystemAlert();
        static DateTime LastSync = DateTime.Now;

        public static void AlertAction(int actionType, AckStatus status, int accountId, string message)
        {
            if (DateTime.Now.Subtract(LastSync).TotalMinutes > 60)
            {
                Active = new ActiveSystemAlert();
            }
            else if (Active == null || Active.IsEmpty)
            {
                Active = new ActiveSystemAlert();
            }
            //ActiveSystemAlert aa = new ActiveSystemAlert(actionType);

            Active.SendAlert(actionType, CreateMessage(status, accountId, message));

        }

		#region Ctor

        /// <summary>
        /// ActiveSystemAlert ctor
        /// </summary>
        /// <param name="dt"></param>
        private ActiveSystemAlert(DataTable dt)
		{
            try
            {
                base.Init(dt,false);
                if (base.IsEmpty)
                {
                    //Warning("ActiveAlertException is emtpy ");
                }
            }
            catch (Exception ex)
            {
               Netlog.Error(" Could not load  ActiveAlert dt Exception" + ex.Message);
            }
		}
        /// <summary>
        /// ActiveSystemAlert ctor
        /// </summary>
        /// <param name="dt"></param>
        private ActiveSystemAlert(int actionType)
        {
           //Netlog.DebugFormat("ActiveSystemAlert start...");

            try
            {
                DataTable dt = DalRule.Instance.SystemAlertsAction(actionType);
                base.Init(dt, false);
                if (base.IsEmpty)
                {
                    //Warning("ActiveAlertException is emtpy ");
                }
            }
            catch (Exception ex)
            {
              Netlog.Error(" Could not load  ActiveAlert Exception" + ex.Message);
            }
        }
        /// <summary>
        /// ActiveSystemAlert ctor
        /// </summary>
        /// <param name="dt"></param>
        private ActiveSystemAlert()
        {
            //Netlog.DebugFormat("ActiveSystemAlert start...");
            DataTable dt = null;
            try
            {
                using (var dl = DalRule.Instance)
                    dt = dl.SystemAlertsAction();
                base.Init(dt, false);
                if (base.IsEmpty)
                {
                    //Warning("ActiveAlertException is emtpy ");
                }
            }
            catch (Exception ex)
            {
               Netlog.Error(" Could not load  ActiveAlert Exception" + ex.Message);
            }
        }
		#endregion

		#region properties

        //UserId, ExceptionId, AlertType, UserName,MailAddress, Phone

        /// <summary>
        /// Get UserId
        /// </summary>
		public int UserId
		{
			get{return base.GetValue<int>("UserId");}
		}
        /// <summary>
        /// Get ActionId
        /// </summary>
		public int ActionId
		{
            get { return base.GetValue<int>("ActionId"); }
		}
         /// <summary>
        /// Get ActionType
        /// </summary>
		public int ActionType
		{
            get { return base.GetValue<int>("ActionType"); }
		}
        /// <summary>
        /// Get AlertType
        /// </summary>
		public int AlertType
		{
			get{return base.GetValue<int>("AlertType");}
		}
        /// <summary>
        /// Get UserName
        /// </summary>
 		public string UserName
		{
			get{return base.GetValue<string>("UserName");}
		}
        /// <summary>
        /// Get MailAddress
        /// </summary>
		public string MailAddress
		{
			get{return base.GetValue<string>("MailAddress");}
		}
        /// <summary>
        /// Phone
        /// </summary>
		public string Phone
		{
			get{return base.GetValue<string>("Phone");}
		}
        ///// <summary>
        ///// Get Url
        ///// </summary>
        //public string Url
        //{
        //    get { return base.GetStringValue("Url"); }
        //}

        /// <summary>
        /// Get AccountId
        /// </summary>
        public int AccountId
        {
            get { return base.GetValue<int>("AccountId"); }
        }
		#endregion

        #region methods

        const string AlertSubject = "Alert from netcell system";

        public static string CreateMessage(AckStatus status, string message)
        {
            return string.Format("Exception {0} : {1}",status.ToString(),message); 
        }
        public static string CreateMessage(AckStatus status,int accountId, string message)
        {
            return string.Format("Exception {0} : {1},{2}", status.ToString(),accountId, message);
        }
        public static string GetMailFrom()
        {
            string from = ViewConfig.SystemMailFrom;

            if (string.IsNullOrEmpty(from))
                return "support@my-t.co.il";
            return from;
        }

        public void SendAlert(int actionType, string message)
        {
            //Log.DebugFormat("SendAlert start...");

            if (IsEmpty)
                return;

            for (int i = 0; i < this.Count; i++)
            {
                this.EntityDataSource.Index = i;
                try
                {
                    if (ActionType != actionType)
                    {
                        continue;
                    }
                    switch (AlertType)
                    {
                        case 1://Warning
                            SendMail(message);
                            break;
                        case 2://Error
                            //SendSms(message);
                            LaunchCommandLineApp(message);
                            break;
                        case 3://Fatal
                            SendMail(message);
                            //SendSmsOut(message);
                            LaunchCommandLineApp(message);
                            break;
                    }

                }
                catch (Exception ex)
                {
                   Netlog.ErrorFormat("ActiveSystemAlert error: {0}", ex.Message);
                }
            }
        }

        private void SendMail(string message)
        {
            //TODO:Fix This

           Netlog.DebugFormat("SendMail " + message);
            try
            {

                //if (string.IsNullOrEmpty(MailAddress))
                //    return;
                //EmailSender.SendEmail(MailAddress, GetMailFrom(), AlertSubject, message);
            }
            catch (Exception ex)
            {
               Netlog.ErrorFormat("SendMail error:{0}", ex.Message);
            }
        }

        private void SendSms(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = AlertSubject;
            if (message.Length > 70)
            {
                message = message.Substring(0, 70);
            }
            //MsgUtil.SendSms(AccountId, Phone, Config.DefaultSmsSender, message,"SystemAlert");
            SendSmsSystem(message, "SystemAlert");
        }

        private void SendSmsOut(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = AlertSubject;
            if (message.Length > 70)
            {
                message = message.Substring(0, 70);
            }
            //MsgUtil.SendSmsOut(AccountId, Phone, Config.DefaultSmsSender, message, "SystemAlert");
            //TODO add request for broker
            SendSmsSystem(message, "SystemAlert");
        }

        public static string SendSmsSystem(string message, string module)
        {
            try
            {

                DataTable dt = DalRule.Instance.Users_Relation(ViewConfig.SystemAccount, 12);
                string[] targets = GetTargets(dt, "Phone");
                if (targets == null || targets.Length == 0)
                    return string.Empty;

                string target = Strings.ArrayToString(targets, ';', true);
                string user = Types.NZ(dt.Rows[0]["LogInName"], "");
                string pass = Types.NZ(dt.Rows[0]["Pass"], "");
                string sender = ViewConfig.SystemSender;

                //return SendSms(Config.SystemAccount, targets, Config.SystemSender, message, module);
                string postData = RemoteUtil.BuildSmsCBQs(user, pass, sender, message, target);
                string url = ViewConfig.SystemSmsUrl;
                return HttpUtil.DoRequest("http://ws.my-t.co.il/sendsms.aspx", postData,"POST", "utf-8");

                

                //string cli = Config.SystemCli;
                //return SendSms(Config.SystemAccount, cli, cli, message, module);
            }
            catch
            {
                return string.Empty;
            }
        }

        void LaunchCommandLineApp(string args)
        {

            try
            {
                // For the example
                string alarmFileName = ViewConfig.SystemAlarmUrl;
                // Use ProcessStartInfo class
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = false;
                startInfo.FileName = alarmFileName;// "Netcell.Alarm.exe";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.Arguments = args;


                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception ex)
            {
               Netlog.ErrorFormat("LaunchCommandLineApp error:{0}", ex.Message);
            }
        }


        public static int SendCreditAlert(int accountId, string message)
        {
            try
            {
                DataRow dr = DalRule.Instance.Users_Alert(accountId);
                if (dr == null)
                    return -1;

                string target = Types.NZ(dr["CellAlert"], "");
                string user = Types.NZ(dr["LogInName"], "");
                string pass = Types.NZ(dr["Pass"], "");
                string sender = ViewConfig.SystemSender;
                CLI cli = new CLI(target);
                if (!cli.IsValid)
                    return -1;


                string postData = RemoteUtil.BuildSmsCBQs(user, pass, sender, message, cli.CellNumber);
                string url = ViewConfig.SystemSmsUrl;
                string response = HttpUtil.DoRequest(url, postData, "POST", "utf-8");
                return 1;
                //return SendSms(accountId, targets, ViewConfig.SystemSender, ViewConfig.CreditMessage, "CreditAlert");
            }
            catch
            {
                return -1;
            }
        }
        
        public static string[] GetTargets(DataTable dt, string colName)
        {
            if (dt == null || dt.Rows.Count == 0)
                return null;
            List<string> list = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string cell = Types.NZ(dr[colName], "");
                CLI cli = new CLI(cell);
                if (cli.IsValid)
                {
                    list.Add(cli.CellNumber);
                }
            }
            return list.ToArray();
        }


        #endregion


        // it will call asynchronously.
        public delegate void AsyncAlertCaller(int actionType, AckStatus status,int accountId, string message);

        public static void AsyncAlertAction(int actionType, AckStatus status, int accountId, string message)
        {
            if(!Sync.ShouldAlert(actionType, status, accountId, message))
            {
                return;
            }

            //Log.DebugFormat("AsyncAlertAction ActionType:{0}", actionType);

            // Create an instance of the exec class.
            AsyncAlert ad = new AsyncAlert();

            // Create the delegate.
            AsyncAlertCaller caller = new AsyncAlertCaller(ad.ExecAlert);

            // Initiate the asychronous call.
            IAsyncResult result = caller.BeginInvoke(actionType,status,accountId,
                message, null, null);

            // Poll while simulating work.
            while (!result.IsCompleted)
            {
                System.Threading.Thread.Sleep(10);
            }

            // Call EndInvoke to retrieve the results.
            caller.EndInvoke(result);


            //Log.DebugFormat("AsyncAlertAction finished");

        }

        private class AsyncAlert
        {
            // The method to be executed asynchronously.
            public void ExecAlert(int actionType, AckStatus status,int accountId, string message)
            {
                //Log.DebugFormat("ExecAlert start...");

                try
                {
                     //ActiveSystemAlert aa = new ActiveSystemAlert(actionType);
                    //if (aa.IsEmpty)
                    //{
                    //   Netlog.ErrorFormat("ActiveSystemAlert ActionType:{0} IsEmpty", actionType);
                    //    return;
                    //}
                    //aa.SendAlert(ActiveSystemAlert.CreateMessage(status,accountId, message));

                    using (DalTrace dal = new DalTrace())
                    {
                        dal.Exceptions_Insert(message, 0, status.ToString(), (int)status, accountId);
                    }

                    ActiveSystemAlert.AlertAction(actionType, status, accountId, message);



                }
                catch (Exception ex)
                {
                   Netlog.ErrorFormat("ExecAlert error:{0}", ex.Message);
                }
            }



        }


        public class SyncAlerts : Dictionary<string, DateTime>
        {

            static DateTime LastAlert = DateTime.Now;

            public bool ShouldAlert(int actionType, AckStatus status, int accountId, string message)
            {

                string key = string.Format("{0}_{1}_{2}_{3}", actionType, status, accountId, message);
                bool ok = true;

                if (DateTime.Now.Subtract(LastAlert).TotalSeconds > 360)
                {
                    this.Clear();
                     ok= true;
                }
                else if (this.ContainsKey(key))
                {
                    DateTime time = this[key];
                    if (DateTime.Now.Subtract(time).TotalSeconds > 60)
                    {
                        ok= true;
                    }
                    ok= false;
                }
                this[key] = DateTime.Now;
                LastAlert = DateTime.Now;
                return ok;
            }


        }

       
    }

    
}
