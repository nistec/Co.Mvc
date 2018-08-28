using System;
using System.Collections.Generic;
using System.Text;
using Nistec;
using System.Web;

namespace Netcell.Remoting
{

    public struct ApiAck
    {
        public readonly string Message;
        public readonly bool IsOk;
        public readonly int Id;
        public ApiAck(bool ok, string msg)
        {
            IsOk = ok;
            Message = msg;
            Id = 0;
        }
        public ApiAck(bool ok, string msg, int id)
        {
            IsOk = ok;
            Message = msg;
            Id = id;
        }
        public override string ToString()
        {
            return Message;
        }
    }
 
    #region Ack structs

    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://my-t.co.il/Ws/")]
    [Serializable]
    public struct Ack
    {
        public int TransId;
        public string Status;
        public int Code;
        public string Description;
        private TargetListAck _Targets;

        public static Ack Empty
        {
            get { return new Ack(); }
        }

        public static Ack Unexpected
        {
            get { return new Ack(0, "", -1, "Unexpected error"); }
        }
        public bool IsOk
        {
            get
            {
               AckStatus state = (AckStatus)this.Code;
               return state == AckStatus.Ok || state == AckStatus.Delivered || state == AckStatus.Received 
                   || state == AckStatus.MsgDelivered || state == AckStatus.MsgCompleted || state == AckStatus.MsgReceived || state == AckStatus.MsgOk;
            }
            set { }
        }

        //public bool IsEmpty
        //{
        //    get { return TransId <= 0; }
        //    set { }
        //}

        /*
         * TODO:fix this
        public Ack(MsgException mex)
        {
            TransId = 0;
            Status = mex.Status.ToString();
            Code = (int)mex.Status;
            Description = mex.Message;
            _Targets = null;
        }
        */
        public Ack(int transId, string status, int code, string description, TargetListAck targets)
        {
            TransId = transId;
            Status = status;
            Code = code;
            Description = description;
            _Targets = targets;
        }

        public Ack(int transId, AckStatus status, string description)
        {
            TransId = transId;
            Status = status.ToString();
            Code = (int)status;
            Description = description;
            _Targets = null;
        }

        public Ack(int transId, string status, int code, string description)
        {
            TransId = transId;
            Status = status;
            Code = code;
            Description = description;
            _Targets = null;
        }

        public TargetListAck Targets
        {
            get
            {
                if (_Targets == null)
                {
                    _Targets = new TargetListAck();
                }
                return _Targets;
            }
        }

        private string GetTargets()
        {
            if (_Targets == null)
                return null;
            int success = 0;
            StringBuilder sb = new StringBuilder();
            foreach (TargetAck t in Targets)
            {
                sb.AppendFormat("<To cli='{0}' opid='{1}'>", t.To, t.OpId);
                sb.AppendFormat("<status code='{0}'>{1}</status>", (int)t.Status, t.Status.ToString());
                sb.Append("</To>");
                if ((int)t.Status < 1000)
                    success++;
            }

            string head = string.Format("<Targets count='{0}' successful='{1}'>", Targets.Count, success);

            string end = "</Targets>";

            return string.Concat(head, sb.ToString(), end);
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(@"<?xml version='1.0' encoding='utf-8' ?>");
            sb.Append("<Ack>");
            sb.AppendFormat("<TransId>{0}</TransId>", TransId);
            sb.AppendFormat("<Code>{0}</Code>", Code);
            sb.AppendFormat("<Status>{0}</Status>", Status);
            sb.AppendFormat("<Description>{0}</Description>", Description);

            if (_Targets != null)
                sb.Append(GetTargets());

            sb.Append("</Ack>");

            return sb.ToString();
        }

        public static Ack Deserialize(string response)
        {
            return Nistec.Xml.XSerializer.Deserialize<Ack>(response);
        }

        /// <summary>
        /// Ack ctor
        /// </summary>
        /// <param name="response"></param>
        public Ack(string response, string rootTag="Ack")
        {
            Nistec.Xml.XmlParser parser = new Nistec.Xml.XmlParser(response);

            System.Xml.XmlNode root = parser.SelectSingleNode(rootTag, false);
            if (root == null)
              root=  parser.Document.FirstChild;

            //System.Xml.XmlNode root = parser.SelectSingleNode("RESULT", true);
            //System.Xml.XmlNode root = parser.SelectSingleNode("Ack", true);
            TransId = parser.GetNodeInnerText(root, "TransId", 0);
            Code = parser.GetNodeInnerText(root, "Code", 0);
            Status = parser.GetNodeInnerText(root, "Status", "");
            Description = parser.GetNodeInnerText(root, "Description", "");
            _Targets = null;
        }

        /// <summary>
        /// Ack ctor
        /// </summary>
        /// <param name="response"></param>
        public Ack(MessageAck ack)
        {
            TransId = ack.TransId;
            Code = (int)ack.Status;
            Status = ack.Status.ToString();
            Description = ack.Description;
            _Targets = ack.Targets;
        }
    }


    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://my-t.co.il/Ws/")]
    [Serializable]
    public struct AckPass
    {
        public string CellNumber;
        public string Password;
        public bool Status;
        public string Description;

        public AckPass(string cellNumber, string password, bool status, string ack)
        {
            CellNumber = cellNumber;
            Password = password;
            Status = status;
            Description = ack;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(@"<?xml version='1.0' encoding='utf-8' ?>");
            sb.Append("<AckPass>");
            sb.AppendFormat("<CellNumber>{0}</CellNumber>", CellNumber);
            sb.AppendFormat("<Password>{0}</Password>", Password);
            sb.AppendFormat("<Status>{0}</Status>", Status);
            sb.AppendFormat("<Description>{0}</Description>", Description);
            sb.Append("</AckPass>");

            return sb.ToString();
        }
    }

    #endregion

    public class MessageAck
    {

        public static string AccessDeniedFormat()
        {
            return (string)MessageAck.AckToXml(AckService.None, AckStatus.AccessDenied, "Access is denied");
        }
      
        public readonly int TransId;
        public readonly string Description;
        public readonly AckStatus Status;

        public string Target;

        private TargetListAck _Targets;

        public TargetListAck Targets
        {
            get
            {
                if(_Targets==null)
                {
                    _Targets = new TargetListAck();
                }
                return _Targets;
            }
        }
        public MessageAck(int transId, AckStatus status)
        {
            this.TransId = transId;
            this.Status = status;
            this.Description = status.ToString();
            this.Target = "";
        }
        public MessageAck(int transId, AckStatus status, string description)
        {
            this.TransId = transId;
            this.Status = status;
            this.Description = description;
            this.Target = "";
        }

        public MessageAck(int transId, AckStatus status, string description, bool sendSystemAlert)
        {
            this.TransId = transId;
            this.Status = status;
            this.Description = description;
            this.Target = "";
            if (sendSystemAlert)
            {
                //TODO:fix this
                //RemoteApi.Instance.ExecuteSystemAlert(1, (int)status, 0, description);


            }
        }

        public MessageAck(int transId, AckStatus status, string description, string target)
        {
            this.TransId = transId;
            this.Status = status;
            this.Description = description;
            this.Targets.Add(new TargetAck(target, TargetAck.ToStatus(status), 0));
        }

        public MessageAck(int transId, AckStatus status, string description, TargetListAck targets)
        {
            this.TransId = transId;
            this.Status = status;
            this.Description = description;
            this._Targets = targets;
        }

        public static MessageAck MessageReceived(int transId, bool ok, string trueMessage, string falseMessage)
        {
            AckStatus status = ok ? AckStatus.Received : AckStatus.UnExpectedError;
            return new MessageAck(transId, status, ok ? trueMessage : falseMessage);
        }

        public static MessageAck MessageReceived(int transId, string target, bool ok, string trueMessage, string falseMessage)
        {
            AckStatus status = ok ? AckStatus.Received : AckStatus.UnExpectedError;
            return new MessageAck(transId, status, ok ? trueMessage : falseMessage,target);
        }

        public static AckStatus ToAckStatus(MsgStatus status)
        {
            switch (status)
            {
                case MsgStatus.Delivered:
                case MsgStatus.Completed:
                    return AckStatus.Delivered;
                case MsgStatus.Blocked:
                    return AckStatus.BlockedByOperator;
                case MsgStatus.Rejected:
                    return AckStatus.RejectedByOperator;
                case MsgStatus.Pending:
                case MsgStatus.Process:
                    return AckStatus.Received;
                default:
                    return AckStatus.UnExpectedError;
            }
        }

        public bool IsOk
        {
            get { return this.Status == AckStatus.Ok || this.Status == AckStatus.Delivered || this.Status == AckStatus.Received
                || this.Status == AckStatus.MsgDelivered || this.Status == AckStatus.MsgCompleted || this.Status == AckStatus.MsgReceived || this.Status == AckStatus.MsgOk;
            }
        }

        public bool IsEmpty
        {
            get { return TransId<=0; }// == Guid.Empty; }
        }

        public static MessageAck Empty
        {
            get 
            {
                return new MessageAck(0, AckStatus.None, "", "");
            }
        }

        public override string ToString()
        {
            if(IsEmpty)
                return string.Format("Result={0},Description={1}", Status.ToString(), Description);

            return string.Format("TransId={0},Result={1},Description={2}", TransId, Status.ToString(), Description);
        }

        public string MessageReponse()
        {
            return string.Format(@"Netcell Ack: " + this.ToString());
        }

        public string ToMessageBox(string lang)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(@"<?xml version='1.0' encoding='utf-8' ?>");
            if (lang == "he")
            {
                sb.Append("<סיכום> \r\n");
                sb.AppendFormat("מספר שליחה: {0} \r\n", TransId);
                sb.AppendFormat("קוד: {0} \r\n", (int)Status);
                if (IsOk)
                {
                    sb.AppendLine("ההודעה נשלחה בהצלחה");
                }
                else
                {
                    sb.AppendFormat("סטטוס: {0} \r\n", Status.ToString());
                    sb.AppendFormat("תאור: {0} \r\n", Description);
                }
            }
            else
            {
                sb.Append("<RESULT> \r\n");
                sb.AppendFormat("TransId: {0} \r\n", TransId);
                sb.AppendFormat("Code: {0} \r\n", (int)Status);
                sb.AppendFormat("Status: {0} \r\n", Status.ToString());
                sb.AppendFormat("Description: {0} \r\n", Description);
            }
            return sb.ToString();
        }

        private string GetTargets()
        {
            if (_Targets == null)
                return null;
            int success = 0;
            StringBuilder sb = new StringBuilder();
            foreach (TargetAck t in Targets)
            {
                sb.AppendFormat("<To cli='{0}' opid='{1}'>", t.To, t.OpId);
                sb.AppendFormat("<status code='{0}'>{1}</status>", (int)t.Status, t.Status.ToString());
                sb.Append("</To>");
                if((int)t.Status < 1000)
                success++;
            }

            string head = string.Format("<Targets count='{0}' successful='{1}'>", Targets.Count, success);

            string end = "</Targets>";

            return string.Concat(head, sb.ToString(), end);
        }

        public string ToXml()
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(@"<?xml version='1.0' encoding='utf-8' ?>");
            sb.Append("<RESULT>");
            sb.AppendFormat("<TransId>{0}</TransId>", TransId);
            sb.AppendFormat("<Code>{0}</Code>", (int)Status);
            sb.AppendFormat("<Status>{0}</Status>", Status.ToString());
            sb.AppendFormat("<Description>{0}</Description>", Description);

            if (_Targets != null)
                sb.Append(GetTargets());

            sb.Append("</RESULT>");

            return sb.ToString();
        }

        public static string AckToXml(int transId, AckStatus status, string description)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(@"<?xml version='1.0' encoding='utf-8' ?>");
            sb.Append("<RESULT>");
            sb.AppendFormat("<TransId>{0}</TransId>", transId);
            sb.AppendFormat("<Code>{0}</Code>", (int)status);
            sb.AppendFormat("<Status>{0}</Status>", status.ToString());
            sb.AppendFormat("<Description>{0}</Description>", description);
            
            sb.Append("</RESULT>");

            return sb.ToString();
        }

        public static string AckToXml(AckService ack, AckStatus status, string description)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(@"<?xml version='1.0' encoding='utf-8' ?>");
            sb.Append("<RESULT>");
            sb.AppendFormat("<Service>{0}</Service>", ack.ToString());
            sb.AppendFormat("<Code>{0}</Code>", (int)status);
            sb.AppendFormat("<Status>{0}</Status>", status.ToString());
            sb.AppendFormat("<Description>{0}</Description>", description);
            sb.Append("</RESULT>");

            return sb.ToString();
        }


        public static MessageAck Desrialize(string xml)
        {

            Nistec.Xml.XmlParser parser = null;
            int TransId = 0;
            int Code = 0;
            string Description = "NA";

            try
            {
                parser = new Nistec.Xml.XmlParser(xml);

                System.Xml.XmlNode root = parser.SelectSingleNode("RESULT", true);
                TransId = parser.GetNodeInnerText(root, "TransId", 0);
                Code = parser.GetNodeInnerText(root, "Code", 0);
                //string Status = parser.GetNodeInnerText(root, "Status", 0);
                Description = parser.GetNodeInnerText(root, "Description", "");

                System.Xml.XmlNodeList xlist = parser.SelectNodes(root, "Targets//To", false);

                if (xlist != null)
                {
                    TargetListAck tack = TargetListAck.Desriealize(xlist);
                    return new MessageAck(TransId, (AckStatus)Code, Description, tack);
                }

                return new MessageAck(TransId, (AckStatus)Code, Description);
            }
            catch (Exception)
            {
                return new MessageAck(TransId, AckStatus.UnExpectedError, "UnExpectedError");
            }
        }


        public static AckStatus GetStatus(string xml)
        {
            Nistec.Xml.XmlParser parser = new Nistec.Xml.XmlParser(xml);

            System.Xml.XmlNode root = parser.SelectSingleNode("RESULT", true);
            int Code = parser.GetNodeInnerText(root, "Code", 0);
            return (AckStatus)Code;
        }

        public static bool GetStatusOk(string xml)
        {
            int status=(int) GetStatus(xml);
            return status > 0 && status <= 102;
        }
    }

    public struct ServiceAck
    {

        public readonly int TaskId;
        public readonly string Description;
        public readonly AckStatus Status;

      
        public ServiceAck(int taskId, AckStatus status, string description)
        {
            this.TaskId = taskId;
            this.Status = status;
            this.Description = description;
        }

        public static ServiceAck Desrialize(string xml)
        {
            Nistec.Xml.XmlParser parser = null;
            int TaskId = 0;
            int Code = 0;
            string Description = "NA";

            try
            {
                parser = new Nistec.Xml.XmlParser(xml);

                System.Xml.XmlNode root = parser.SelectSingleNode("RESULT", true);
                TaskId = parser.GetNodeInnerText(root, "TaskId", 0);
                Code = parser.GetNodeInnerText(root, "Code", 0);
                //string Status = parser.GetNodeInnerText(root, "Status", 0);
                Description = parser.GetNodeInnerText(root, "Description", "");

                return new ServiceAck(TaskId, (AckStatus)Code, Description);
            }
            catch (Exception)
            {
                return new ServiceAck(TaskId, AckStatus.UnExpectedError, "UnExpectedError");
            }
            //return (ServiceAck)Nistec.Xml.Serializer.Deserialize(xml, typeof(ServiceAck));
        }

        //public bool IsOk
        //{
        //    get { return this.Status == AckStatus.Ok || this.Status == AckStatus.Delivered || this.Status == AckStatus.Received; }
        //}
        public bool IsOk
        {
            get { return this.Status == AckStatus.Ok || this.Status == AckStatus.Delivered || this.Status == AckStatus.Received || this.Status == AckStatus.MsgDelivered || this.Status == AckStatus.MsgCompleted || this.Status == AckStatus.MsgOk; }
        }
        public bool IsEmpty
        {
            get { return Status == AckStatus.None; }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(@"<?xml version='1.0' encoding='utf-8' ?>");
            sb.Append("<RESULT>");
            sb.AppendFormat("<TaskId>{0}</TaskId>", TaskId);
            sb.AppendFormat("<Code>{0}</Code>", (int)Status);
            sb.AppendFormat("<Status>{0}</Status>", Status.ToString());
            sb.AppendFormat("<Description>{0}</Description>", Description);
            sb.Append("</RESULT>");

            return sb.ToString();
        }


    }

    public struct PasswordAck
    {

        public readonly string Service;
        public readonly string CellNumber;
        public readonly string Password;
        public readonly int Id;
        public readonly AckStatus Status;

        public PasswordAck(string cellNumber, string password, int id, AckStatus status)
        {
            this.Service = "Password Service";
            this.CellNumber = cellNumber;
            this.Password = password;
            this.Id = id;
            this.Status = status;
        }

        //public bool IsOk
        //{
        //    get { return this.Status == AckStatus.Ok || this.Status == AckStatus.Delivered || this.Status == AckStatus.Received; }
        //}
        public bool IsOk
        {
            get { return this.Status == AckStatus.Ok || this.Status == AckStatus.Delivered || this.Status == AckStatus.Received 
                || this.Status == AckStatus.MsgDelivered || this.Status == AckStatus.MsgCompleted || this.Status == AckStatus.MsgReceived || this.Status == AckStatus.MsgOk; }
        }
        public bool IsEmpty
        {
            get { return string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(CellNumber); }// == Guid.Empty; }
        }

    
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(@"<?xml version='1.0' encoding='utf-8' ?>");
            sb.Append("<RESULT>");
            sb.AppendFormat("<Service>{0}</Service>", "Password");
            sb.AppendFormat("<CellNumber>{0}</CellNumber>", CellNumber);
            sb.AppendFormat("<Password>{0}</Password>", Password);
            sb.AppendFormat("<Id>{0}</Id>", Id);
            sb.AppendFormat("<Status>{0}</Status>", Status);
            sb.Append("</RESULT>");

            return sb.ToString();
        }

    }
 
}
