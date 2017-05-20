using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.ComponentModel;
using Pro.Server;
using Pro.Server.Data;
using Nistec.Data;
using Nistec.Data.Entities;

namespace Pro.Server
{

    public class SchedulerCommand 
    {
        int Position;
        int Count;
        List<ServerTask> Tasks;

        public bool IsEmpty
        {
            get { return (Tasks != null && Tasks.Count > 0); }
        }

        #region Ctor

        public SchedulerCommand()
        {
            //Netcell.Log.Debug("Start ActiveCommand");
            try
            {
                DataTable dt = DalAdmin.Instance.Scheduler_job();
                if (dt!=null && dt.Rows.Count>0)
                {
                   Tasks= EntityDataExtension.DataTableToEntityList<ServerTask>(dt);
                }
            }
            catch (Exception ex)
            {
                new ServerException("Could Not load SchedulerCommand Error","SchedulerCommand", ex);
            }
        }

        public void Dispose()
        {

        }

        #endregion


        delegate void  ExecuteCommandsDelegate();

        public void ExecuteCommandsAsync()
        {
            ExecuteCommandsDelegate callBack = new ExecuteCommandsDelegate(ExecuteCommands);
            IAsyncResult ar = callBack.BeginInvoke(null, null);
            callBack.EndInvoke(ar);
        }

        public void ExecuteCommands()
        {
            try
            {
                //Netcell.Log.Debug("Start CommandProcess");
                if (Tasks==null || Tasks.Count==0)
                    return;

                Position = 0;
                Count = Tasks.Count;

                //Netcell.Log.DebugFormat("Fetch Command Command {0} Scheduler: " + SchedulerId.ToString(), CommandId);
                
                for (int i = 0; i < Count; i++)
                {
                    Position = i;

                    ServerTask task=Tasks[i];

                    switch (task.CommandType)
                    {
                        case 0://sql
                            ExecuteDB(task);
                            break;
                        case 1://process
                            ExecuteProcess(task);
                            break;
                        case 2://web
                            ExecuteWeb(task);
                            break;
                        case 3://App
                            ExecuteRemoteApp(task);
                            break;
                        case 4://Zip
                            ExecuteZip(task);
                            break;
                        case 5://Cleaner
                            ExecuteCleaner(task);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                new ServerException("Run Process has Error","SchedulerCommand", ex);
            }
        }

        private void ExecuteDB(ServerTask task)
        {
            bool execHistory = true;
            try
            {
                switch (task.CommandName.ToLower())
                {
                    case "netcelldb":
                        DalNetcell.Instance.ExecuteCommand(task.CommandText, task.Timeout, task.Async);
                        break;
                    case "netcell_sb":
                        DalCo.Instance.ExecuteCommand(task.CommandText, task.Timeout, task.Async);
                        break;
                    case "netcell_services":
                        DalAdmin.Instance.ExecuteCommand(task.CommandText, task.Timeout, task.Async);
                        execHistory = false;
                        break;
                }
                if (execHistory)
                {
                    DalAdmin.Instance.Scheduler_History(task.SchedulerId, task.CommandId, task.CommandName, 0, "ok");
                }
            }
            catch (Exception ex)
            {
                DalAdmin.Instance.Scheduler_History(task.SchedulerId, task.CommandId, task.CommandName, 1, ex.Message);
                new ServerException("ExecuteDB Error", "SchedulerCommand", ex);
            }
        }

        private void ExecuteProcess(ServerTask task)
        {
            // These are the Win32 error code for file not found or access denied.
            const int ERROR_FILE_NOT_FOUND = 2;
            const int ERROR_ACCESS_DENIED = 5;

            Process proc = new Process();

            try
            {
                // Get the path that stores user documents.
                string processFolder = System.Configuration.ConfigurationManager.AppSettings["ProcessFolder"];

                proc.StartInfo.FileName = System.IO.Path.Combine(processFolder, task.CommandText);
                //proc.StartInfo.Verb = CommandName;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.Arguments = task.Arguments;

                bool ok=proc.Start();

                DalAdmin.Instance.Scheduler_History(task.SchedulerId, task.CommandId, task.CommandName, 0, ok.ToString());
            }
            catch (Win32Exception e)
            {
                if (e.NativeErrorCode == ERROR_FILE_NOT_FOUND)
                {
                    Console.WriteLine(e.Message + ". Check the path.");
                }

                else if (e.NativeErrorCode == ERROR_ACCESS_DENIED)
                {
                    // Note that if your word processor might generate exceptions
                    // such as this, which are handled first.
                    Console.WriteLine(e.Message +
                        ". You do not have permission to print this file.");
                }
                DalAdmin.Instance.Scheduler_History(task.SchedulerId, task.CommandId, task.CommandName, 1, e.Message);
            }
            catch (Exception ex)
            {
                DalAdmin.Instance.Scheduler_History(task.SchedulerId, task.CommandId, task.CommandName, 1, ex.Message);
                new ServerException("ExecuteProcess Error", "SchedulerCommand", ex);
            }

        }

        private void ExecuteWeb(ServerTask task)
        {
            string result = "";
            try
            {
                if (task.CommandName.ToUpper() == "POST")
                {
                    Nistec.Web.HttpUtil hu = new Nistec.Web.HttpUtil(task.CommandText);
                    result = hu.DoRequest(task.Arguments);
                }
                else
                {
                    result = Nistec.Web.HttpUtil.DoGet(task.CommandText, task.Arguments, "UTF-8");
                }
                DalAdmin.Instance.Scheduler_History(task.SchedulerId, task.CommandId, task.CommandName, 0, result);
            }
          
            catch (Exception ex)
            {
                DalAdmin.Instance.Scheduler_History(task.SchedulerId, task.CommandId, task.CommandName, 1, ex.Message);
                new ServerException("ExecuteWeb Error", "SchedulerCommand", ex);
            }

        }

        private void ExecuteRemoteApp(ServerTask task)
        {
            //string result = "";
            //try
            //{
            //    int appId=Nistec.Types.ToInt(CommandText,0);
            //    string [] args=null;
            //    if(!string.IsNullOrEmpty(Arguments))
            //    {
            //        args= Arguments.Split(';');
            //    }
            //    Netcell.Remoting.RemoteSyncClient client=new Netcell.Remoting.RemoteSyncClient();
            //    Netcell.Messaging.IAppResult appResult= client.ExecuteApplication(appId,args);
            //    result=appResult.ErrorMessage;
            //    DalAdmin.Instance.Scheduler_History(SchedulerId, CommandId, CommandName, 0, result);
            //}
            //catch (Exception ex)
            //{
            //    DalAdmin.Instance.Scheduler_History(SchedulerId, CommandId, CommandName, 1, ex.Message);
            //}

        }

        private void ExecuteZip(ServerTask task)
         {
             string result = "";
             try
             {

                 Zipper zip = new Zipper();
                 int count = zip.DoZip();
                 result = count.ToString() + " files zipped";
                 DalAdmin.Instance.Scheduler_History(task.SchedulerId, task.CommandId, task.CommandName, 0, result);
             }

             catch (Exception ex)
             {
                 DalAdmin.Instance.Scheduler_History(task.SchedulerId, task.CommandId, task.CommandName, 1, ex.Message);
                 new ServerException("ExecuteZip Error", "SchedulerCommand", ex);
             }

         }

        private void ExecuteCleaner(ServerTask task)
         {
             string result = "";
             try
             {

                 LogCleaner cleaner = new  LogCleaner();
                 int count = cleaner.DoClean();
                 result = count.ToString() + " files cleaned";
                 DalAdmin.Instance.Scheduler_History(task.SchedulerId, task.CommandId, task.CommandName, 0, result);
             }

             catch (Exception ex)
             {
                 DalAdmin.Instance.Scheduler_History(task.SchedulerId, task.CommandId, task.CommandName, 1, ex.Message);
                 new ServerException("ExecuteCleaner Error", "SchedulerCommand", ex);
             }

         }
    }
}
