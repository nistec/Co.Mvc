using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netcell.Data.Client;
using Nistec.Generic;

namespace Netcell.Remoting
{
    public class RemoteCmd
    {

        public RemoteCmd(int timeout)
        {
            Timeout = timeout;
        }

        public int Timeout { get; set; }

        public static RemoteCmd Instance { get { return new RemoteCmd(0); } }

        public void ExecTask(string cmd, params object[] args)
        {
            GenericTasker ta = new GenericTasker();
            //ta.TaskCompleted += new EventHandler(ta_TaskCompleted);
            ta.Execute(() => Exec(cmd, args), Timeout);
            Console.WriteLine("TaskCompleted");
        }

        static void ta_TaskCompleted(object sender, EventArgs e)
        {
            Console.WriteLine("Result:{0}", sender);
        }


        public T ExecTask<T>(string cmd, params object[] args)
        {
            GenericTasker<object> ta = new GenericTasker<object>();
            object res = (object)ta.Execute(() => Exec(cmd, args), Timeout);
            Console.WriteLine("Result:{0}", res);
            return (T)res;
        }

        public object Exec(string cmd, params object[] args)
        {
            try
            {
                switch (cmd.ToLower())
                {
                    case "refresh statistic":
                        using (DalCampaign dal = new DalCampaign())
                        {
                            return DalCampaign.Instance.Campaigns_Statistic_Refresh((int)args[0], (int)args[1], true);
                        }
                    //case "block cell":
                    //    if (args == null || args.Length < 1)
                    //        return 0;
                    //    CLI cli = new CLI(args[0].ToString());
                    //    int accountid = (int)args[1];
                    //    int groupId = (int)args[2];
                    //    string remark = args[3].ToString();
                    //    using (DalContacts dal = new DalContacts())
                    //    {
                    //        return dal.Contacts_Cli_Block(cli.CellNumber, accountid, groupId, remark, "Mo");
                    //    }

                }
            }
            catch (MsgException mex)
            {
                throw mex; ;
            }
            catch (Exception ex)
            {
                throw new MsgException(AckStatus.UnExpectedError, "RemoteCmd Error: " + ex.Message);
            }
            return null;
        }


        public static object Command(string cmd, params object[] args)
        {
            return RemoteCmd.Instance.Exec(cmd, args);
        }

        public static void CommandAsync(string cmd, params object[] args)
        {
            RemoteCmd.Instance.ExecTask(cmd, args);
        }

        public static T CommandAsync<T>(string cmd, params object[] args)
        {
            return RemoteCmd.Instance.ExecTask<T>(cmd, args);
        }

    }
}
