using Netcell.Data.Entities;
using Netcell.Remoting;
using Nistec.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netcell.Lib
{
    
    public class CampaignNotifyServer
    {

        public static bool ShouldNotifyBeginCampaign(CampaignNotifyType notifyType, int BatchIndex, BatchTypes BatchType)
        {
            if (BatchType == BatchTypes.Preview)
                return false;

            switch (notifyType)
            {
                case CampaignNotifyType.None:
                    return false;
                //case CampaignNotifyType.OnBatchBegin:
                //case CampaignNotifyType.BothAll:
                case CampaignNotifyType.BeginAndReply:
                case CampaignNotifyType.BothAndReply:
                case CampaignNotifyType.Both:
                    return true;
                case CampaignNotifyType.OnStart:
                    return BatchIndex == 0;
                default:
                    return false;
            }
        }

        public static bool ShouldNotifyEndCampaign(CampaignNotifyType notifyType, Scheduler_Queue schedulerItem)
        {
            if (schedulerItem.GetBatchType() == BatchTypes.Preview)
                return false;

            switch (notifyType)
            {
                case CampaignNotifyType.None:
                    return false;
                //case CampaignNotifyType.OnBatchEnd:
                //case CampaignNotifyType.BothAll:
                //     return true;
                case CampaignNotifyType.BothAndReply:
                case CampaignNotifyType.Both:
                case CampaignNotifyType.OnEnd:
                case CampaignNotifyType.OnReplyOnly:
                    if (/*schedulerItem.ItemType > 0 &&*/ schedulerItem.ItemRange > 0)//multi
                        return schedulerItem.ItemIndex == schedulerItem.ItemRange - 1;
                    return true;
                default:
                    return false;
            }
        }

        public static bool ShouldNotifyEndCampaign(CampaignNotifyType notifyType, int BatchIndex, int BatchRange, int BatchType)
        {
            if ((BatchTypes)BatchType == BatchTypes.Preview)
                return false;
            switch (notifyType)
            {
                case CampaignNotifyType.None:
                    return false;
                //case CampaignNotifyType.OnBatchEnd:
                //case CampaignNotifyType.BothAll:
                //     return true;
                case CampaignNotifyType.BothAndReply:
                case CampaignNotifyType.Both:
                case CampaignNotifyType.OnEnd:
                case CampaignNotifyType.OnReplyOnly:
                    if (BatchType > 0 && BatchRange > 0)//multi
                        return BatchIndex == BatchRange - 1;
                    return true;
                default:
                    return false;
            }
        }

       
        public static void ProcessCampaignNotifyBegin(CampaignEntity campaign, CampaignNotifyType notifyType, int BatchIndex)
        {
            if (string.IsNullOrEmpty(campaign.NotifyCells))
                return;
            //CampaignNotifyType notifyType = campaign.FeaturesItem.NotifyOptions;

            Netlog.DebugFormat("Process Campaign Notify Begin, NotifyType: {0} ", notifyType);
            try
            {
                string[] cells = campaign.NotifyCells.Split(';');
                if (cells == null || cells.Length == 0)
                    return;
                PlatformType notifyPlatform = NotifyTemplateTypes.GetCampaignNotifyPlatform(cells[0]);
                if (notifyPlatform == PlatformType.NA)
                    return;
                string sender = campaign.Sender;
                if (notifyPlatform == PlatformType.Cell)
                    sender = campaign.Platform == (int)PlatformType.Cell ? campaign.Sender : AccountInfo.GetDefaultSender(campaign.AccountId);
                else
                    sender = "services";

                switch (notifyType)
                {
                    case CampaignNotifyType.OnStart://"בתחילת הקמפיין";
                    case CampaignNotifyType.Both://"בתחילה ובסיום הקמפיין";
                    case CampaignNotifyType.BothAndReply://"בתחילה ובסיום הקמפיין";
                    case CampaignNotifyType.BeginAndReply:
                        if (BatchIndex == 0 && campaign.NotifyType > 0)
                        {
                            //notifyMsg = GetNotifyMessage(CampaignNotifyType.OnStart, campaign.CampaignId, campaign.BatchIndex);
                            int templateId = NotifyTemplateTypes.GetCampaignNotifyTemplateId(notifyPlatform, NotifyActionType.Begin);
                            RemoteAlertServer.SendCampaignStatisticAlert(notifyPlatform, campaign.AccountId, campaign.CampaignId, cells, sender, 0, templateId);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MsgException.Trace(AckStatus.ApplicationException, campaign.AccountId, "ProcessCampaignNotify error: " + ex.Message);
            }
        }

        public static void ProcessCampaignNotifyEnd(CampaignEntity campaign, CampaignNotifyType notifyType, int BatchIndex, int BatchRange)
        {
            if (string.IsNullOrEmpty(campaign.NotifyCells))
                return;
            if (BatchIndex < BatchRange)
                return;

            //CampaignNotifyType notifyType = campaign.FeaturesItem.NotifyOptions;

            int addDays = 0;// AgentsConfig.NotifyAddDays;

            Netlog.DebugFormat("Process Campaign Notify End, NotifyType: {0} ", notifyType);
            try
            {
                string[] cells = campaign.NotifyCells.Split(';');
                if (cells == null || cells.Length == 0)
                    return;
                PlatformType notifyPlatform = NotifyTemplateTypes.GetCampaignNotifyPlatform(cells[0]);
                if (notifyPlatform == PlatformType.NA)
                    return;

                string sender = campaign.Sender;

                if (notifyPlatform == PlatformType.Cell)
                    sender = campaign.Platform == (int)PlatformType.Cell ? campaign.Sender : AccountInfo.GetDefaultSender(campaign.AccountId);
                else
                    sender = "services";

                int templateId = NotifyTemplateTypes.GetCampaignNotifyTemplateId(notifyPlatform, NotifyActionType.End);

                switch (notifyType)
                {
                    case CampaignNotifyType.OnEnd://"בסיום הקמפיין";
                    case CampaignNotifyType.Both://"בסיום ובסיום הקמפיין";
                        RemoteAlertServer.SendCampaignStatisticAlert(notifyPlatform, campaign.AccountId, campaign.CampaignId, cells, sender, 0, templateId);
                        break;
                    case CampaignNotifyType.BothAndReply://"בסיום הקמפיין וסטטיסטיקה";
                    case CampaignNotifyType.BeginAndReply://"בהתחלה וסטטיסטיקה";
                        RemoteAlertServer.SendCampaignStatisticAlert(notifyPlatform, campaign.AccountId, campaign.CampaignId, cells, sender, 0, templateId);
                        //statistic alerts
                        templateId = NotifyTemplateTypes.GetCampaignNotifyTemplateId(notifyPlatform, NotifyActionType.Statistic);
                        RemoteAlertServer.SendCampaignStatisticAlert(notifyPlatform, campaign.AccountId, campaign.CampaignId, cells, sender, 0, templateId, addDays);
                        break;
                    case CampaignNotifyType.OnReplyOnly://"סטטיסטיקה בלבד";
                        templateId = NotifyTemplateTypes.GetCampaignNotifyTemplateId(notifyPlatform, NotifyActionType.Statistic);
                        RemoteAlertServer.SendCampaignStatisticAlert(notifyPlatform, campaign.AccountId, campaign.CampaignId, cells, sender, 0, templateId, addDays);
                        break;
                }

            }
            catch (Exception ex)
            {
                MsgException.Trace(AckStatus.ApplicationException, campaign.AccountId, "ProcessCampaignNotify error: " + ex.Message);
            }
        }
    }
}
