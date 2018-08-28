using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netcell.Data.Client;
using Netcell.Data.DbWeb.Entities;
using Netcell.Remoting;
using Nistec.Data;
using Netcell.Lib;
using Netcell.Entities;
//using System.Threading.Tasks;
//using Nistec.Trace;

namespace Netcell.Lib
{
    public class ApiSiteAction
    {
        /*
        SitesPropsEntity prop; 
        int accountId; 
        int campaignId; 
        string target; 
        string sender;
        */

        public ApiSiteAction()//SitesPropsEntity prop, int accountId, int campaignId, string target, string sender)
        {

        }
        
        public void DoAction(SitesPropsEntity prop, int accountId, int campaignId, string target, string sender)
        {
            try
            {
                string pname = prop.PropName;

                if (pname == null)
                {
                    //TODO:LOGIT
                    return;
                }

                string user = null;
                string pass = null;
                int userId = 0;
                using (DalAccounts dal = new DalAccounts())
                {
                    var dr = dal.GetFirstUser(accountId);
                    if (dr == null)
                    {
                        //TODO:LOGIT
                        return;
                    }

                    user = dr.Get<string>("LogInName");
                    pass = dr.Get<string>("Pass");
                    userId = dr.Get<int>("UserId",0);
                }
                if (user == null || pass == null)
                {
                    //TODO:LOGIT
                    return;
                }

                string timeToSend = null;

                switch (pname.ToLower())
                {
                    case "sendsms":
                        string res = RemoteApi.Instance.ProcessMessageEx(MethodType.SMSMT.ToString(), user, pass, sender, true, timeToSend, prop.PropValue, null, 0, "", target);
                        OnCompleted(new Ack(res));
                        break;
                    case "sendmail":

                        break;

                    case "sendcampaignsms":
                        {
                            if (campaignId > 0)
                            {
                                RemoteAck ack = CampaignRemote.InvokeBatch(MethodType.SMSMT.ToString(), accountId, target, campaignId, false, sender, timeToSend, userId);
                                OnCompleted(ack);
                            }
                            break;
                        }
                    case "sendcampaignmail":
                        {
                            if (campaignId > 0)
                            {
                                RemoteAck ack = CampaignRemote.InvokeBatch(MethodType.MALMT.ToString(), accountId, target, campaignId, false, sender, timeToSend, userId);
                                OnCompleted(ack);
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                Log.Exception("ApiSiteAction.DoAction error: ", ex);
            }
        }

        void OnCompleted(RemoteAck ack)
        {
            Log.DebugFormat("ApiSiteAction.OnCompleted: {0}", ack.ToString());
        }
        void OnCompleted(Ack ack)
        {
            Log.DebugFormat("ApiSiteAction.OnCompleted: {0}", ack.ToString());
        }
    }
}


