using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Messaging;
using Netcell.Data;
using Nistec;
using Netcell;
using Netcell.Data.DbServices.Entities;

namespace Netcell.Remoting
{
    public class QueueItemBuilder
    {

        #region Create Message Item

        public static IQueueItem CreateRemoteCellItem(int accountId, string method, string sender, string message, string target, int campaignId, int ContactId)
        {
            CLI cli = new CLI(target);
            if (!cli.IsValid)
                return null;
            bool concat = true;
            MethodType methodType = MsgMethod.ToMethodType(method);
            BillingType billType = BillingType.CB;
            decimal price = 0;
            byte bunch = UnitsItem.GetBunch(accountId);
            BillingItem.ValidateCredit(accountId, methodType, 1, UnitsItem.CreateUnitsItem(methodType, message, true,bunch), ref price);

            int batchId = 0;
            int msgId = 0;

            
            using (Counters counter = new Counters())
            {
                //-batchId = counter.AddBatch(accountId, 0, (int)methodType, (int)billType, 0, ViewConfig.Server.ToString(), "MessageBuilder.CreateRemoteCellItem");

                batchId= counter.AddBatch(accountId, campaignId, 1, BatchTypes.Single, 0,PlatformType.Cell, (int)methodType, price,null);

                msgId = counter.AddMessage(campaignId, batchId, DateTime.Now, cli.CellNumber, null, sender, (int)PlatformType.Cell, ContactId);
            }
            IQueueItem item = CreateCellQueueItem(Priority.Normal, msgId, campaignId, batchId, accountId, methodType, billType, sender, cli.CellNumber, message, price, concat);
            return item;

        }


        public static IQueueItem CreateRemoteMailItem(int accountId, string from, string display, string replyTo, string target, string subject, string body, int campaignId,string charset, Priority priority, int ContactId)
        {
            MethodType methodType = MethodType.MALMT;
            int itemsCount = 1;
            int batchId = 0;
            int msgId = 0;

            BillingItem bi = BillingItem.CreateBillingItem(accountId, methodType, new UnitsItem(methodType, body, true,0), itemsCount, 0, CreditMode.Active);

         
            using (Counters counter = new Counters())
            {
                //-batchId = counter.AddBatch(accountId, 0, (int)methodType, (int)BillingType.CB, 0, ViewConfig.Server.ToString(), "MessageBuilder.CreateRemoteMailItem");

                batchId = counter.AddBatch(accountId, campaignId, 1, BatchTypes.Single, 0, PlatformType.Cell, (int)methodType, bi.ItemPrice,null);

                msgId = counter.AddMessage(campaignId, batchId, DateTime.Now, target, null, from, (int)PlatformType.Mail, ContactId);
            }
            
            //mailhost
            IMailHost mcnn = Mail_Host.CreateDefaultConnection(accountId);//.CreateQuickConnection(accountId);
            IQueueItem item = CreateMailQueueItem(priority, msgId, campaignId, batchId, accountId, from, display, replyTo, target, subject, body, bi.ItemPrice, bi.ItemUnits, mcnn.OperatorId, charset,mcnn.HostId);

            return item;
        }

        public static string GetContext(string message)
        {
            if (message == null)
                return "";
            bool isLatin = RemoteUtil.IsLatin(message);
            return string.Format("{0}:{1}", isLatin ? "en" : "he", message.Length);
        }

        #region Gateway methods

        public static IQueueItem CreateMoQueueItem(int transId, BillingType billingType, string SC, string target, string message, int operatorId, string Trid, string OP, string xml)
        {
            Priority priority = (Priority)RemoteUtil.GetPriority(MethodType.SMSMO.ToString(), billingType.ToString());
            QueueItem item = new QueueItem(priority);
            item.Body = xml;
            item.Destination = target;
            item.Sender = SC;
            item.OperationId = operatorId;
            item.Subject = MethodType.SMSMO.ToString();
            item.Label = billingType.ToString();
            item.ClientContext = message;
            item.Notify = OP;
            item.TransactionId = Trid;
            item.MessageId = transId;

            return item as IQueueItem;
        }
/*
        public static IQueueItem CreateRBQueueItem(int serviceId, int campaignId, int transId, int accountId, string method, string sender, string clientContext, string xml, decimal price, string timeToSend, string notify, string reference)
        {
            string billingType = "RB";
            MethodType Method = MsgMethod.ToMethodType(method, true);
            BillingType BillingType = MsgMethod.ToBillingType(billingType);

            Priority priority = (Priority)RemoteUtil.GetPriority(method, billingType.ToString());
            QueueItem item = new QueueItem(priority);
            
            item.Body = xml;
            item.SenderId = accountId;
            item.TransactionId = reference;
            item.Subject = method;
            item.Label = billingType.ToString();
            item.ClientContext = clientContext;
            item.AppSpecific = campaignId;
            item.Identifer = serviceId;
            item.Sender = sender;
            item.Notify = notify;
            item.MessageId = transId;
            item.Price = price;

            if (!string.IsNullOrEmpty(timeToSend))
            {
                item.SentTime = Types.ToDateTime(timeToSend, DateTime.Now.AddMinutes(-5));
            }

            return item;
        }
*/
        public static IQueueItem CreateBulkQueueItem(int campaignId, int transId, int accountId, string method, string sender, string clientContext, string xml, decimal price, string timeToSend, string notify, string reference)
        {
            string billingType = "CB";
            MethodType Method = MsgMethod.ToMethodType(method, true);
            BillingType BillingType = MsgMethod.ToBillingType(billingType);

            Priority priority = (Priority)RemoteUtil.GetPriority(method, billingType.ToString());
            QueueItem item = new QueueItem(priority);

            item.Body = xml;
            item.SenderId = accountId;
            item.TransactionId = reference;
            item.Subject = method;
            item.Label = billingType.ToString();
            item.ClientContext = clientContext;
            item.AppSpecific = campaignId;
            //item.Identifer = serviceId;
            item.Sender = sender;
            item.Notify = notify;
            item.MessageId = transId;
            item.Price = price;

            if (!string.IsNullOrEmpty(timeToSend))
            {
                item.SentTime = Types.ToDateTime(timeToSend, DateTime.Now.AddMinutes(-5));
            }

            return item;
        }


           //public static IQueueItem CreateMtMessage(IMessageCell m,int transId,int accountId,int serviceId,decimal price)
        //{

        //    //IQueueItem item = null;
        //    //if (billingType == BillingType.RB)

        //    //    item = QueueItemBuilder.CreateRBQueueItem(serviceId, campaignId, transId, AccountId, method, sender, clientContext, xml, price, timeToSend, notify, reference);
        //    //else
        //    //    item = QueueItemBuilder.CreateBulkQueueItem(campaignId, transId, AccountId, method, sender, clientContext, xml, price, timeToSend, notify, reference);

        //    Priority priority = (Priority)RemoteUtil.GetPriority(m.Method.ToString(), m.Billing.ToString());

        //    QueueItem item = new QueueItem(priority);
        //    //TODO: SET PRIORITY
        //    item.Body = m.SerializeBody();
        //    item.SenderId = accountId;
        //    item.TransactionId = m.Reference;
        //    item.Subject = m.Method.ToString();
        //    item.ClientContext = m.Context;
        //    item.Identifer = serviceId;
        //    item.Label = m.Billing.ToString();
        //    item.Sender = m.Sender;
        //    item.Notify = m.Notify;
        //    item.AppSpecific = m.CampaignId;
        //    item.MessageId = transId;
        //    item.Price = price;

        //    if (!string.IsNullOrEmpty(m.TimeToSend))
        //    {
        //        item.SentTime = Types.ToDateTime(m.TimeToSend, DateTime.Now.AddMinutes(-5));
        //    }

        //    return item as IQueueItem;
        //}

        #endregion

        public static IQueueItem CreateCellQueueItem(Priority priority, int msgId, int campaignId, int batchId, int accountId, string method, string billingType, string sender, string target, string message, decimal price, bool concat)
        {
            MethodType Method=MsgMethod.ToMethodType(method,true);
            BillingType BillingType = MsgMethod.ToBillingType(billingType);

            return CreateCellQueueItem(priority, msgId, campaignId, batchId, accountId, Method, BillingType, sender, target, message, price, concat);
        }
        public static IQueueItem CreateCellQueueItem(Priority priority, int msgId, int campaignId, int batchId, int accountId, string method, string billingType, string sender, string target, string message, decimal price, bool concat,int operatorId,string notify)
        {
            MethodType Method = MsgMethod.ToMethodType(method, true);
            BillingType BillingType = MsgMethod.ToBillingType(billingType);

            IQueueItem item= CreateCellQueueItem(priority, msgId, campaignId, batchId, accountId, Method, BillingType, sender, target, message, price, concat);
            item.OperationId = operatorId;
            item.Notify = notify;
            return item;
        }
        public static IQueueItem CreateCellQueueItem(Priority priority, int msgId, int campaignId, int batchId, int accountId, MethodType method, BillingType billingType, string sender, string target, string message, decimal price, bool concat)
        {

            int operatorId = 0;

            try
            {

                if (billingType == BillingType.LB)
                    operatorId = ViewConfig.LabOperator;
                else
                {
                    CLI cli = new CLI(target);
                    if (!cli.IsValid)
                    {
                        throw new Exception("Cli no valid");
                    }
                    target = cli.CellNumber;
                    operatorId = cli.OperatorId;
                }

                QueueItem item = new QueueItem(priority);

                item.MessageId = batchId;
                item.AppSpecific = campaignId;
                item.Body = message;
                item.ClientContext = GetContext(message);
                item.Destination = target;
                item.Identifer = msgId;
                item.Label = billingType.ToString();
                item.Price = price;
                item.Segments = concat ? 1 : 0;
                item.Sender = sender;
                item.SenderId = accountId;
                item.Server = ViewConfig.Server;
                item.Subject = method.ToString();
                item.OperationId = operatorId;

                return item as IQueueItem;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("CreateMessageItem Error:{0} ", ex.Message);
                return null;
            }
        }

        public static IQueueItem CreateMailQueueItem(Priority priority,int msgId, int campaignId, int batchId, int accountId, string sender, string display, string replyTo, string target, string subject, string body, decimal price, int segments,int operatorId, string charset, string hostId)
        {
            try
            {

                QueueItem item = new QueueItem(priority);

                item.MessageId = batchId;
                item.AppSpecific = campaignId;
                item.Body = body;
                item.ClientContext = display;
                item.Destination = target;
                item.Identifer = msgId;
                item.Label = hostId;
                //item.ClientContext = Types.NzOr(charset, ViewConfig.DefaultMailCharset);

                item.Price = price;
                item.Segments = segments;
                item.Sender = sender;
                item.SenderId = accountId;
                item.Server = ViewConfig.Server;
                item.Subject = subject;
                item.Notify = replyTo;
                item.OperationId = operatorId;

                return item as IQueueItem;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Camapign CreateMailQueuItem Error:{0} ", ex.Message);
                return null;
            }
        }

        #endregion

    }
}
