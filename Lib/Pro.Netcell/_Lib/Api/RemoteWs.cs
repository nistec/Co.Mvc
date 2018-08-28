using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netcell.Lib.Mobile;

using Netcell.Remoting;
using Netcell.Web;
using Netcell.Lib;
using Nistec;
using Nistec.Sys;

namespace Netcell.Lib
{
    public class RemoteWs
    {


        //public static IRemoteApi RemoteApi
        //{
        //    get { return RemoteApi.RemoteClient; }
        //}

        static int DecodeAccountId(string arg)
        {
            return Types.ToInt( Encryption.DecryptPass(arg),0);
        }

        #region Remote messages

        public static MsgStatus SendSms(string arg, string sender, string message, int campaignId, string target)
        {
            return SendMessage(arg, MethodType.SMSMT, sender, message, null, campaignId, target);
        }

        public static MsgStatus SendSwp(string arg, string sender, string message, string url, int campaignId, string target)
        {
            message = WapUtil.FormatSwpMessage(url, message);
            return SendMessage(arg, MethodType.SMSWP, sender, message, url, campaignId, target);
        }

        //public static MsgStatus SendWap(string arg, string sender, string message, string url, int campaignId, string target)
        //{
        //    return SendMessage(arg, MethodType.WAPMT, sender, message, url, campaignId, target);
        //}



        public static MsgStatus SendMessage(string arg, MethodType method, string sender, string message, string url, int campaignId, string target)
        {
            int accountId = DecodeAccountId(arg);
            string reference = null;
            string notify = null;
            BillingItem.ValidateCredit(accountId, method, 1, new UnitsItem(message, 0, true, UnitsItem.GetBunch(accountId)));
            return (MsgStatus)RemoteApi.Instance.ProcessSenderCB(accountId, method.ToString(), sender, message, url, campaignId, reference, notify, target);
        }

/*
        public static MsgStatus SendMessageRB(string arg, MethodType method, string sender, string message, string url, int priceCode, int campaignId, int media, string reference, string target)
        {
            int accountId = DecodeAccountId(arg);
            string notify = null;
            return (MsgStatus)RemoteApi.Instance.ProcessSenderRB(accountId, method.ToString(), sender, message, url, priceCode, campaignId, media, reference, notify, target);
        }
*/
        /*
         public static MsgStatus SendSms(string arg, string sender, string message, int campaignId, string target)
        {
            return SendMessage(arg, MethodType.SMSMT, sender, message, null, campaignId, target);
        }

        public static MsgStatus SendWap(string arg, string sender, string message, string url, int campaignId, string target)
        {
            return SendMessage(arg, MethodType.WAPMT, sender, message, url, campaignId, target);
        }
        public static MsgStatus SendSwp(string arg, string sender, string message, string url, int campaignId, string target)
        {
            message = WapUtil.FormatSwpMessage(url, message);
            return SendMessage(arg, MethodType.SMSWP, sender, message, url, campaignId, target);
        }
        public static MsgStatus SendMessage(string arg, MethodType method, string sender, string message, string url, int campaignId, string target)
        {
            int accountId = DecodeAccountId(arg);
            string billingType = "CB";
            string module = "Netcell.ClientApi.Ws";
            string serviceCode = null;
            int media = 0;
            string reference = null;
            string notify = null;

            BillingItem.ValidateCredit(accountId, method, 1, new UnitsItem(message, 0, true));



            return (MsgStatus)RemoteApi.ProcessSenderAsync(accountId, method.ToString(), billingType, module, sender, message, url, serviceCode, campaignId, media, reference, notify, target);
        }

        public static MsgStatus SendMessageRB(string arg, MethodType method, string sender, string message, string url, string serviceCode, int campaignId, int media, string reference, string target)
        {
            int accountId = DecodeAccountId(arg);
            string billingType = "RB";
            string module = "Netcell.ClientApi.Ws";
            string notify = null;
            return (MsgStatus)RemoteApi.ProcessSenderAsync(accountId, method.ToString(), billingType, module, sender, message, url, serviceCode, campaignId, media, reference, notify, target);
        }
        */
        #endregion

        /*
        [Obsolete("not used")]
        public static MsgStatus SendMail(string arg, string sender, string display, string replyTo, string subject, string message, int campaignId, string target)
        {
            int accountId = DecodeAccountId(arg);
         
            BillingItem.ValidateCredit(accountId, MethodType.MALMT, 1, 1);

            return (MsgStatus)RemoteApi.Instance.ExecuteTransRemote(MethodType.MALMT.ToString(), accountId, sender, display, replyTo, target, subject, message, campaignId);
        }
        */

        #region Static
        /*
        public static bool ValidateCredit(int AccountId, string Method, int DestinationCount, string Message, bool concat)
        {
            int units = ActiveCredit.CalcBillingUnits(AccountId, Method, Message, concat);
            
            return ValidateCredit(AccountId, Method, DestinationCount, units);

        }
        public static bool ValidateCredit(int AccountId, string Method, int DestinationCount, int ItemUnits)
        {

            if (ItemUnits <= 0)
                ItemUnits = 1;
            CreditStatus cs = ActiveCredit.GetCreditAndPrice(AccountId, Method.ToString(), DestinationCount, ItemUnits);
            if (cs == null)
            {
                throw new MsgException(AckStatus.BillingException, "Billing error for account: " + AccountId.ToString());
            }
            if (!cs.HasCredit)
            {
                throw new MsgException(AckStatus.NotEnoughCredit, "Not Enough Credit error for account: " + AccountId.ToString());
            }
            //decimal Cost = cs.TotalCost;
            //decimal ItemPrice = cs.ItemPrice;
            //int TotalUnits = cs.Units;
            return cs.HasCredit;
        }
        */
        public static int ValidateMessageAccount(string Modul, string HostName, MethodType Method, string BillingType, string User, string Password, string Sender, bool Concatenate, string ServiceCode, string TimeToSend, string Message, string Url, int CampaignId, string notifyUrl, string Reference, int media, string Targets, bool CheckCredit)
        {
            int accountId = 0;

            Netcell.Log.InfoFormat(Modul + ": User={0}, Password={1}, Sender={2}, ServiceCode={3}, TimeToSend={4},Message={5},Url={6}, Targets={7}, HostName={8}", User, Password, Sender, ServiceCode, TimeToSend, Message, Url, Targets, HostName);
            string result = "ok";// ValidateMessage(User, Password, Sender, ServiceCode, TimeToSend, Message, Url, CampaignId, notifyUrl, Reference, Targets);

            #region validation


            if (!RemoteUtil.IsAlphaNumeric(User, Password))
            {
                result = "Illegal UserName or password";
                goto Label_exit;
            }

            if (!RemoteUtil.RegexMatch(@"^[0-9\s\-]+$", Sender))
            {
                result = "Illegal Sender required Valid Phone number or ShortCode";
                goto Label_exit;
            }

            if (BillingType == "RB")
            {
                if (string.IsNullOrEmpty(ServiceCode))
                {
                    result = "Invalid ServiceCode for RB";
                    goto Label_exit;
                }
                else if (ServiceCode.Length > 50)
                {
                    result = "Illegal ServiceCode";
                    goto Label_exit;
                }
            }

            if (MsgMethod.IsSwp(Method))
            {
                if (string.IsNullOrEmpty(Url))
                {
                    result = "Invalid Url";
                    goto Label_exit;
                }
                else //if (!string.IsNullOrEmpty(Url))
                {
                    if (!Url.ToLower().StartsWith("http://"))
                    {
                        result = "Illegal Url";
                        goto Label_exit;
                    }
                    //else
                    //    Url = HttpUtility.UrlEncode(Url);
                }
            }

            if (!string.IsNullOrEmpty(notifyUrl))
            {
                if (!notifyUrl.ToLower().StartsWith("http://"))
                    notifyUrl = "";// result = "Illegal notifyUrl:" + notifyUrl;
            }

            if (!string.IsNullOrEmpty(TimeToSend))
            {
                if (DateHelper.IsDateTime(TimeToSend))
                {
                    result = "Invalid TimeToSend format, rquired DateTime formt dd/MM/yyyy HH:mm or yyyy-MM-dd HH:mm";
                    goto Label_exit;
                }
            }

            if (!string.IsNullOrEmpty(Reference))
            {
                if (Reference.Length > 50)
                    Reference = "";// result = "Reference length is too long";
            }

            if (string.IsNullOrEmpty(Message))
            {
                result = "Invalid Message";
                goto Label_exit;
            }

            if (Message.Length > 500)
            {
                result = "Message length is too long";
                goto Label_exit;
            }

            if (string.IsNullOrEmpty(Targets))
            {
                result = "Invalid Targets";
                goto Label_exit;
            }
            #endregion

            UserInfo au = new UserInfo(User, Password);
            if (au.IsEmpty)
            {
                result = "Access denied";
                throw new MsgException(AckStatus.AccessDenied, result, null);
             }
            accountId = au.AccountId;
            if (BillingType == "CB" && CheckCredit)
            {
                int count = TargetListItem.GetTargetsCount(Targets);
                //int units = ActiveCredit.CalcBillingUnits(accountId, MethodType, Message, Concatenate);
                //ValidateCredit(accountId, MethodType, count, units);

                BillingItem.ValidateCredit(accountId, Method, count, UnitsItem.CreateUnitsItem(Method, Message, Concatenate, UnitsItem.GetBunch(accountId)));
            }

        Label_exit:

            if (result != "ok")
            {
                Netcell.Log.WarnFormat(Modul + ": Result {0}", result);
                throw new MsgException(AckStatus.ArgumentException, result, null);
            }


            return accountId;

        }

        public static void ValidateMessageAccount(string Modul, string HostName, MethodType Method, UserAuth User, string Sender, string TimeToSend, string Message, string notifyUrl, string Reference, int TargetsCount, bool CheckCredit)
        {
            if (User==null)
            {
                throw new MsgException(AckStatus.AccessDenied, "Invalid User", null);
            }
            Netcell.Log.InfoFormat(Modul + ": AccountId={0}, Sender={1}, TimeToSend={2},Message={3}, Targets={4}, HostName={5}", User.AccountId, Sender, TimeToSend, Message, TargetsCount, HostName);
            string result = "ok";

            #region validation


            if (!RemoteUtil.RegexMatch(@"^[0-9\s\-]+$", Sender))
            {
                result = "Illegal Sender required Valid Phone number or ShortCode";
                goto Label_exit;
            }

            if (!string.IsNullOrEmpty(notifyUrl))
            {
                if (!notifyUrl.ToLower().StartsWith("http://"))
                    notifyUrl = "";// result = "Illegal notifyUrl:" + notifyUrl;
            }

            if (!string.IsNullOrEmpty(TimeToSend))
            {
                if (DateHelper.IsDateTime(TimeToSend))
                {
                    result = "Invalid TimeToSend format, rquired DateTime formt dd/MM/yyyy HH:mm or yyyy-MM-dd HH:mm";
                    goto Label_exit;
                }
            }

            if (!string.IsNullOrEmpty(Reference))
            {
                if (Reference.Length > 50)
                    Reference = "";// result = "Reference length is too long";
            }

            if (string.IsNullOrEmpty(Message))
            {
                result = "Invalid Message";
                goto Label_exit;
            }

            if (Message.Length > 500)
            {
                result = "Message length is too long";
                goto Label_exit;
            }

            //if (string.IsNullOrEmpty(Targets))
            //{
            //    result = "Invalid Targets";
            //    goto Label_exit;
            //}

            if (TargetsCount<=0)
            {
                result = "Invalid Targets";
                goto Label_exit;
            }
            #endregion


            int accountId = User.AccountId;
            if (CheckCredit)
            {
                //int count = TargetListItem.GetTargetsCount(Targets);
                BillingItem.ValidateCredit(accountId, Method, TargetsCount, UnitsItem.CreateUnitsItem(Method, Message, true, UnitsItem.GetBunch(accountId)));
            }

        Label_exit:

            if (result != "ok")
            {
                Netcell.Log.WarnFormat(Modul + ": Result {0}", result);
                throw new MsgException(AckStatus.ArgumentException, result, null);
            }

        }
        public static void ValidateMessageAccount(string Modul, string HostName, MethodType Method, int userId,int accountId, string Sender, string TimeToSend, string Message, string notifyUrl, string Reference, int TargetsCount, bool CheckCredit)
        {
            if (userId <= 0 || accountId<=0)
            {
                throw new MsgException(AckStatus.AccessDenied, "Invalid User", null);
            }
            Netcell.Log.InfoFormat(Modul + ": AccountId={0}, Sender={1}, TimeToSend={2},Message={3}, Targets={4}, HostName={5}", accountId, Sender, TimeToSend, Message, TargetsCount, HostName);
            string result = "ok";

            #region validation


            if (!RemoteUtil.RegexMatch(@"^[0-9\s\-]+$", Sender))
            {
                result = "Illegal Sender required Valid Phone number or ShortCode";
                goto Label_exit;
            }

            if (!string.IsNullOrEmpty(notifyUrl))
            {
                if (!notifyUrl.ToLower().StartsWith("http://"))
                    notifyUrl = "";// result = "Illegal notifyUrl:" + notifyUrl;
            }

            if (!string.IsNullOrEmpty(TimeToSend))
            {
                if (DateHelper.IsDateTime(TimeToSend))
                {
                    result = "Invalid TimeToSend format, rquired DateTime formt dd/MM/yyyy HH:mm or yyyy-MM-dd HH:mm";
                    goto Label_exit;
                }
            }

            if (!string.IsNullOrEmpty(Reference))
            {
                if (Reference.Length > 50)
                    Reference = "";// result = "Reference length is too long";
            }

            if (string.IsNullOrEmpty(Message))
            {
                result = "Invalid Message";
                goto Label_exit;
            }

            //if (Message.Length > 500)
            //{
            //    result = "Message length is too long";
            //    goto Label_exit;
            //}

            //if (string.IsNullOrEmpty(Targets))
            //{
            //    result = "Invalid Targets";
            //    goto Label_exit;
            //}

            if (TargetsCount <= 0)
            {
                result = "Invalid Targets";
                goto Label_exit;
            }
            #endregion

            if (CheckCredit)
            {
                //int count = TargetListItem.GetTargetsCount(Targets);
                BillingItem.ValidateCredit(accountId, Method, TargetsCount, UnitsItem.CreateUnitsItem(Method, Message, true, UnitsItem.GetBunch(accountId)));
            }

        Label_exit:

            if (result != "ok")
            {
                Netcell.Log.WarnFormat(Modul + ": Result {0}", result);
                throw new MsgException(AckStatus.ArgumentException, result, null);
            }

        }

        /*
        public static MsgStatus ProcessMessageEx(string Modul, string HostName, string MethodType, string BillingType, string User, string Password, string Sender, bool Concatenate, string ServiceCode, string TimeToSend, string Message, string Url, int CampaignId, string notifyUrl, string Reference, int media, string Targets)
        {
            int accountId = 0;
            try
            {
                accountId = ValidateMessageAccount(Modul, HostName, MethodType, BillingType, User, Password, Sender, Concatenate, ServiceCode, TimeToSend, Message, Url, CampaignId, notifyUrl, Reference, media, Targets);

                MsgStatus status = MsgStatus.None;
                if (BillingType == "RB")
                {
                    status = SendMessageRB(accountId, MethodType, Sender, Message, Url, ServiceCode, CampaignId, media, Reference, Targets);

                }
                else
                {
                    status = SendMessage(accountId, MethodType, Sender, Message, Url, CampaignId, Targets);
                }
                return status;
            }
            catch (MsgException mex)
            {
                throw mex;
            }
            catch (Exception ex)
            {
                throw new MsgException(AckStatus.UnExpectedError, "Unexpected Error", null);
            }
        }
        */
        #endregion

    }
}