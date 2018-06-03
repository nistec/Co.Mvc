using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netcell.Lib;
using Netcell.Remoting;
using Netcell.Data;
using Nistec.Logging;
using Netcell.Data.DbServices.Entities;

namespace Netcell.Remoting
{
    public class RemoteAlertServer
    {

        //public static int Add_Alerts
        //   (
        //    ServicesAlertType AlertType,
        //    PlatformType PlatformMode,
        //    DateTime ExecTime,
        //    string Target,
        //    string Sender,
        //    int AccountId,
        //    int ItemId,
        //    int ArgId,
        //    string Subject,
        //    string Body,
        //    string ReplyTo,
        //    string Display,
        //    int UserId,
        //    int Units)
        //{
        //    DateTime Expiration = ExecTime.AddHours(2);

        //    Services_Alerts alert = new Services_Alerts()
        //    {
        //        AccountId = AccountId,
        //        AlertType = (int)AlertType,
        //        ArgId = ArgId,
        //        Body = Body,
        //        Display = Display,
        //        ExecTime = ExecTime,
        //        ItemId = ItemId,
        //        Platform = (int)PlatformMode,
        //        ReplyTo = ReplyTo,
        //        Sender = Sender,
        //        Subject = Subject,
        //        Target = Target,
        //        Units = Units,
        //        UserId = UserId
        //    };

        //    return Services_Alerts_Context.Insert(alert);
            
        //    //using (DalServices dal = new DalServices())
        //    //{
        //    //    return dal.Services_Alerts_Add(Guid.NewGuid(), (int)AlertType, (int)PlatformMode, ExecTime, Expiration, Target, Sender, AccountId, ItemId, ArgId, Subject, Body, ReplyTo, Display, UserId, Units, 0);
        //    //}
        //}

        public static int SendCampaignStatisticAlert(PlatformType PlatformMode, int AccountId, int CampaignId, string Target, string Sender, int UserId, int TemplateId, DateTime ExecTime)
        {
            ServicesAlertType AlertType = ServicesAlertType.CampaignStatistic;
            DateTime Expiration = ExecTime.AddHours(2);
            int ArgId = 0;
            int Units = 1;

            string sender = PlatformMode == PlatformType.Mail ? MailUtil.ValidateMailSender(Sender) : Sender;
            string body = null;
            string subject = null;
            if (TemplateId == 0)
            {
                CreateAlertMessage(AlertType, PlatformMode, CampaignId, AccountId, out body, out subject);
            }
            else
            {
                CreateAlertMessage(TemplateId, AlertType, PlatformMode, CampaignId, AccountId, out body, out subject);
            }
            //using (DalServices dal = new DalServices())
            //{
            //    Log.DebugFormat("Services_Alerts_Add CampaignId:{0}, Target:{1}, subject:{2}", CampaignId, Target, subject);
            //    return dal.Services_Alerts_Add(Guid.NewGuid(), (int)AlertType, (int)PlatformMode, ExecTime, Expiration, Target, sender, AccountId, CampaignId, ArgId, subject, body, null, null, UserId, Units, TemplateId);
            //}

            Netlog.DebugFormat("SendCampaignStatisticAlert PlatformMode: {0},CampaignId:{1},TemplateId:{2}, Target:{3},ExecTime:{4},subject:{5}, ", PlatformMode, CampaignId, TemplateId, Target, ExecTime, subject);


            Services_Alerts sa = new Services_Alerts()
            {
                AccountId = AccountId,
                AlertType = (int)AlertType,
                ArgId = ArgId,
                Body = body,
                Display = null,
                ExecTime = ExecTime,
                ItemId = CampaignId,
                Platform = (int)PlatformMode,
                ReplyTo = null,
                Sender = sender,
                State = 0,
                Subject = subject,
                Target = Target,
                TemplateId = TemplateId,
                Units = Units,
                UserId = UserId
            };
                //(int)AlertType, (int)PlatformMode, ExecTime, Target, sender, AccountId, CampaignId, ArgId, subject, body, null, null, UserId, Units, TemplateId)


            return Services_Alerts_Context.Insert(sa);
        }

        public static int SendCampaignStatisticAlert(PlatformType PlatformMode, int AccountId, int CampaignId, string Target, string Sender, int UserId, int TemplateId)
        {
            return RemoteAlertServer.SendCampaignStatisticAlert(PlatformMode, AccountId, CampaignId, Target, Sender, UserId, TemplateId, DateTime.Now);
        }

        public static int SendCampaignStatisticAlert(PlatformType PlatformMode, int AccountId, int CampaignId, string[] Targets, string Sender, int UserId, int TemplateId)
        {
            //Log.DebugFormat("SendCampaignStatisticAlert PlatformMode: {0},CampaignId:{1},TemplateId:{2} ", PlatformMode, CampaignId, TemplateId);

            int res = 0;
            foreach (string s in Targets)
            {
                res += RemoteAlertServer.SendCampaignStatisticAlert(PlatformMode, AccountId, CampaignId, s, Sender, UserId, TemplateId);
            }
            return res;
        }

        public static int SendCampaignStatisticAlert(PlatformType PlatformMode, int AccountId, int CampaignId, string[] Targets, string Sender, int UserId, int TemplateId, int addDays)
        {
            int res = 0;
            foreach (string s in Targets)
            {
                for (int i = 0; i <= addDays; i++)
                {
                    DateTime execTime = DateTime.Now.AddDays(i);
                    res += RemoteAlertServer.SendCampaignStatisticAlert(PlatformMode, AccountId, CampaignId, s, Sender, UserId, TemplateId, execTime);
                }
            }
            return res;
        }

        public static void CreateAlertMessage(ServicesAlertType alertType, PlatformType platform, int campaignId, int accountId, out string body, out string subject)
        {
            switch ((ServicesAlertType)alertType)
            {
                case ServicesAlertType.CampaignStatistic:
                    CampaignReportView view = new CampaignReportView(campaignId, accountId, (PlatformType)platform);

                    if ((PlatformType)platform == PlatformType.Mail)
                    {
                        view.CreateMailMessage(out body, out subject);

                    }
                    else if ((PlatformType)platform == PlatformType.Cell)
                    {
                        view.CreateCellMessage(out subject);
                        body = null;
                    }
                    else //if ((PlatformType)Platform == PlatformType.SMS)
                    {
                        view.CreateCellMessage(out subject);
                        body = null;
                    }
                    break;
                default:
                    body = null;
                    subject = null;
                    break;
            }
        }

        public static void CreateAlertMessage(int templateId,ServicesAlertType alertType, PlatformType platform, int campaignId, int accountId, out string body, out string subject)
        {
            switch ((ServicesAlertType)alertType)
            {
                case ServicesAlertType.CampaignStatistic:
                    CampaignReportView view = new CampaignReportView(campaignId, accountId, (PlatformType)platform);

                    if ((PlatformType)platform == PlatformType.Mail)
                    {
                        view.CreateMailMessage(templateId,out body, out subject);

                    }
                    else if ((PlatformType)platform == PlatformType.Cell)
                    {
                        view.CreateCellMessage(templateId, out subject);
                        body = null;
                    }
                    else //if ((PlatformType)Platform == PlatformType.SMS)
                    {
                        view.CreateCellMessage(templateId,out subject);
                        body = null;
                    }
                    break;
                default:
                    body = null;
                    subject = null;
                    break;
            }
        }


    }

}
