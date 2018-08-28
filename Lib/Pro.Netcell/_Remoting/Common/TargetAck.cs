using Nistec;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Netcell.Remoting
{


    [Serializable]
    public class TargetListAck : List<TargetAck>
    {
        public TargetAck Find(string to)
        {
           return base.Find(delegate(TargetAck t) { return t.To == to; });
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int count = 0;
            int succes = 0;
            foreach (TargetAck ta in this)
            {
                count++;
                if (ta.Status == MsgStatus.Delivered || ta.Status == MsgStatus.Completed)
                    succes++;
                sb.Append(ta.ToString());
            }

            string root= string.Format("<Targets count='{0}' successful='{1}'>",count,succes);

            return string.Concat(root, sb.ToString(), "</Targets>");
        }
        public static TargetListAck Desriealize(XmlNodeList xlist)
        {
            TargetListAck acks = new TargetListAck();

            try
            {
                foreach (XmlNode x in xlist)
                {
                    TargetAck ack = TargetAck.Desriealize(x);
                    if (ack != null)
                    {
                        acks.Add(ack);
                    }
                }

                return acks;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    [Serializable]
    public class TargetAck
    {
        public readonly string To;
        public readonly MsgStatus Status;
        public readonly int OpId;

     
        public static MsgStatus ToStatus(AckStatus status)
        {
            switch (status)
            {
                case AckStatus.None:
                    return MsgStatus.None;
                case AckStatus.Ok:
                case AckStatus.Delivered:
                case AckStatus.Received:
                case AckStatus.MsgReceived:
                case AckStatus.MsgDelivered:
                case AckStatus.MsgCompleted:
                case AckStatus.MsgOk:
                    return MsgStatus.Delivered;
                case AckStatus.BlockedByOperator:
                    return MsgStatus.Blocked;
                case AckStatus.RejectedByOperator:
                    return MsgStatus.Rejected;
                default:
                    return MsgStatus.Failed;
            }
        }

        public TargetAck()
        {
        }

        public TargetAck(string to, MsgStatus status, int opId)
        {
            CLI cli = new CLI(to);
            if (!cli.IsValid)
            {
                Status = MsgStatus.Canceled;
                To = to;
            }
            else
            {
                Status = status;
                To = cli.CellNumber;
            }
            //Status = status;
            OpId = opId;
        }

        public TargetAck(string to)
        {
            CLI cli = new CLI(to);
            if (!cli.IsValid)
            {
                Status = MsgStatus.Canceled;
                To = to;
            }
            else
            {
                Status = MsgStatus.Delivered;
                To = cli.CellNumber;
            }
            OpId = 0;
        }

        public override string ToString()
        {
            return string.Format("<To cli='{0}' opid='{1}'><status code='{2}'>{3}</status></To>", To, OpId, (int)Status, Status.ToString());
        }

        public static TargetAck Desriealize(XmlNode node)
        {
            //<To cli='{0}' opid='{1}'>", t.To, t.OpId);
            //<status code='{0}'>{1}</status>", (int)t.Status, t.Status.ToString());
            //</To>");

            string to=null;
            MsgStatus status = MsgStatus.None;
            int opId = 0;
            //string description = "NA";
            XmlAttribute attr=null;

            try
            {
                
                attr = node.Attributes["cli"];
                if (attr == null)
                    return null;
                to = attr.Value;

                attr = node.Attributes["opid"];
                if (attr != null)
                    opId = Types.ToInt(attr.Value, 0);

                XmlNode n = node.ChildNodes[0];

                attr = n.Attributes["code"];
                if (attr != null)
                    status = (MsgStatus)Types.ToInt(attr.Value, 0);

                //description = n.InnerText;

                return new TargetAck(to,status,opId);
            }
            catch (Exception)
            {
                return new TargetAck("*", MsgStatus.Failed, 0);
            }
        }
        
    }

    /*
    [Serializable]
    public class TargetItemList : List<TargetItem>
    {
        public TargetItem Find(string to)
        {
            return base.Find(delegate(TargetItem t) { return t.To == to; });
        }

        public void Add(string to, string personal, int sentId)
        {
            base.Add(new TargetItem(to, personal, sentId));
        }
    }

    [Serializable]
    public class TargetItem
    {
        public readonly string To;
        public readonly string Personal;
        public readonly int SentId;

        public TargetItem(string to, string personal, int sentId)
        {
            To = to;
            Personal = personal;
            SentId = sentId;
        }

    }

    
    [Serializable]
    public class Target
    {
        public readonly string To;
        public readonly string SendTo;
        public readonly int ContentId;
        public readonly string Message;
        public readonly string Url;
        public readonly string ContentType;
        public readonly bool IsValid;

        public int CarrierId;
        public string Personal;


        public static string[] SplitTargets(string targets)
        {
            if (string.IsNullOrEmpty(targets))
                return null;
            return Target.CreateBySplit(targets.TrimStart(';').TrimEnd(';'));
        }
        public static int GetTargetsCount(string targets)
        {
            string[] tr = SplitTargets(targets);
            if (tr == null)
                return 0;
            return tr.Length;
        }
        public Target(string to, string sendTo, string message, string personal, string url,string contentType,int contentId)
        {
            if (string.IsNullOrEmpty(to))
            { 
                throw new ArgumentNullException(to);
            }

            CLI cli = new CLI(to);
            IsValid = cli.IsValid;
            if (!IsValid)
            {
                return;
            }
            //this.OperatorId = cli.OperatorId;
            this.To = cli.CellNumber;
            this.SendTo = CLI.FixNumber(sendTo);
            this.Message = message;
            this.Url = url;
            this.ContentType = contentType;
            this.IsValid = true;
            this.ContentId = contentId;
            this.Personal = personal;
            if (!string.IsNullOrEmpty(personal))
            {
                try
                {
                    Message = RemoteUtil.FoematMessage(message, personal);
                }
                catch
                {
                    IsValid = false;
                }
            }

        }
      
        public Target(string to)
            :this(to,"","","","","TEXT",0)
        {
        }
        /// <summary>
        /// Ctor for MO
        /// </summary>
        /// <param name="to"></param>
        /// <param name="sendTo"></param>
        /// <param name="aggregatorId"></param>
        public Target(string to, int carrierId)
        {
            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentNullException(to);
            }

            CLI cli = new CLI(to);
            IsValid = cli.IsValid;
            if (!IsValid)
            {
                return;
            }
            this.CarrierId = carrierId;
            this.To = cli.CellNumber;
            this.IsValid = true;
        }

 
        public override string ToString()
        {
            return To;
        }

        /// <summary>
        /// Split string parameter by ';' or ',' and create list of Targets
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        public static Target[] CreateTargetBySplit(string concatenate)
        {
            string[] list = CreateBySplit(concatenate);
            Target[] targets=new Target[list.Length];
            int i = 0;
            foreach (string s in list)
            {
                targets[i] = new Target(s);
                i++;
            }
            return targets;
        }
        /// <summary>
        /// Split string parameter by ';' or ',' and create string array of targets
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        public static string[] CreateBySplit(string concatenate)
        {
            if (string.IsNullOrEmpty(concatenate))
            {
                throw new ArgumentNullException(concatenate);
            }
            string[] list = concatenate.Split(new char[] { ';', ',', ' ' });
            return list;
        }

    }

    */
}
