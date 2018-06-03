using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Data;
using System.ComponentModel;
using System.IO;
using Nistec.Xml;
using Nistec;
using Nistec.Serialization;

namespace MControl.Messaging
{

    #region DB structer
    /*
3	ID	int	4	0
0	ItemID	varchar	50	0
0	QueueName	varchar	50	0
0	Subject	varchar	50	0
0	Body	image	16	0
0	MessageId	int	4	0
0	ArrivedTime	datetime	8	0
0	Status	tinyint	1	0
0	Sender	nvarchar	50	0
0	Destination	nvarchar	50	0
0	SentTime	datetime	8	0
0	SenderId	int	4	0
0	OperationId	int	4	0
0	Notify	varchar	250	1
*/
    #endregion

    #region interface 
    /// <summary>
    /// IQueueItem
    /// </summary>
    public interface IQueueItem : IDisposable//,System.Runtime.Remoting.Messaging.IMessage
    {
       
        /// <summary>
        /// Get ItemId
        /// </summary>
        Guid ItemId { get; }
        /// <summary>
        /// Get MessageId
        /// </summary>
        int MessageId { get; set; }
        /// <summary>
        /// Get ArrivedTime
        /// </summary>
        DateTime ArrivedTime { get; }
        /// <summary>
        /// Get SentTime
        /// </summary>
        DateTime SentTime { get; }
        /// <summary>
        /// Get Priority
        /// </summary>
        Priority Priority { get; }

        /// <summary>
        /// Get or Set Body
        /// </summary>
        object Body { get; set; }
        /// <summary>
        /// Get or Set Subject
        /// </summary>
        string Subject { get; set; }
        /// <summary>
        /// Get or Set Label
        /// </summary>
        string Label { get; set; }
        /// <summary>
        /// Get or Set Sender
        /// </summary>
        string Sender { get; set; }
        /// <summary>
        /// Get or Set Destination
        /// </summary>
        string Destination { get; set; }
        /// <summary>
        /// Get or Set Status
        /// </summary>
        ItemState Status { get; set; }
        /// <summary>
        /// Get or Set SenderId
        /// </summary>
        int SenderId { get; set; }
        /// <summary>
        /// Get or Set OperationId
        /// </summary>
        int OperationId { get; set; }
        /// <summary>
        /// Get Has Attach
        /// </summary>
        bool HasAttach { get; }
        /// <summary>
        /// Get AttachItems List
        /// </summary>
        IList<QueueAttachItem> AttachItems { get; }
        /// <summary>
        /// Get Retry
        /// </summary>
        int Retry { get; }
        /// <summary>
        /// Get or Set AppSpecific
        /// </summary>
        int AppSpecific { get; set; }
        /// <summary>
        /// Get or Set TransactionId
        /// </summary>
        string TransactionId { get; set; }
        /// <summary>
        /// Get or Set Server
        /// </summary>
        int Server { get; set; }
        /// <summary>
        /// Get or Set TimeOut in seconds
        /// </summary>
        int TimeOut { get; set; }

        /// <summary>
        /// Get or Set Identifer
        /// </summary>
        int Identifer { get; set; }
        ///// <summary>
        ///// Get or Set AttachStream
        ///// </summary>
        //string AttachStream { get; set; }
        /// <summary>
        /// Get or Set Notify
        /// </summary>
        string Notify { get; set; }
        /// <summary>
        /// Get or Set Price
        /// </summary>
        decimal Price { get; set; }
        /// <summary>
        /// Get or Set Segments
        /// </summary>
        int Segments { get; set; }
        /// <summary>
        /// Get or Set ClientContext
        /// </summary>
        string ClientContext { get; set; }
        /// <summary>
        /// Get or Set Format
        /// </summary>
        string Format { get; set; }
        /// <summary>
        /// Get or Set Host/Queue name
        /// </summary>
        string Host { get; set; }

        /// <summary>
        /// Serialize QueueItem to string
        /// </summary>
        string ToString();
        /// <summary>
        /// Serialize QueueItem to xml
        /// </summary>
        string Serialize();
         /// <summary>
        /// Serialize Queue Item To ByteArray
        /// </summary>
        /// <returns></returns>
        byte[] ToByteArray();
        /// <summary>
        /// Stores Queue Item it's child entities to specified stream.
        /// </summary>
        /// <param name="storeStream">Stream where to store mime entity.</param>
        void ToStream(System.IO.Stream storeStream);
        /// <summary>
        /// AddAttachment
        /// </summary>
        /// <param name="item"></param>
        void AddAttachment(QueueAttachItem item);
        /// <summary>
        /// AddAttachment
        /// </summary>
        /// <param name="items"></param>
        void AddAttachment(QueueAttachItem[] items);
        /// <summary>
        /// AddAttachment
        /// </summary>
        /// <param name="attachStream"></param>
        /// <param name="attachPath"></param>
        void AddAttachment(string attachStream, string attachPath);
        /// <summary>
        /// Get Copy of Queue item
        /// </summary>
        /// <returns></returns>
        QueueItem Copy();

        /// <summary>
        /// Get indicate wether the item is timeout 
        /// </summary>
        bool IsTimeOut { get; }
        /// <summary>
        /// ToDictionary
        /// </summary>
        /// <returns></returns>
        IDictionary ToDictionary();
        /// <summary>
        /// Save file
        /// </summary>
        /// <param name="path"></param>
        void Save(string path);
        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="path"></param>
        void Delete(string path);
        /// <summary>
        /// Get Duration
        /// </summary>
        /// <returns></returns>
        double Duration();
    }

    #endregion

    public struct QueueItemPtr
    {
        public QueueItem Item;
    }

    [Serializable]
    [ComVisible(true)]
    public sealed class QueueItem:IDisposable,IQueueItem
    {
        #region memebers

        public static readonly TimeSpan DefaultTimeOut = TimeSpan.FromMinutes(60);//4294967295);
        public static readonly int DefaultTimeOutInSecond = 3600;
        private List<QueueAttachItem> attachItems;

        #endregion

        #region locking

        private int m_Lock;

        internal void LOCK()
        {
            Interlocked.Exchange(ref m_Lock, 1);
        }
        internal void UNLOCK()
        {
            Interlocked.Exchange(ref m_Lock, 0);
        }
        internal int ISLOCK()
        {
            return m_Lock;
        }
        #endregion

        #region property

        /// <summary>
        /// Get ItemId
        /// </summary>
        public Guid ItemId { get; set; }
        /// <summary>
        /// Get MessageId
        /// </summary>
        public int MessageId { get; set; }
        /// <summary>
        /// Get Priority
        /// </summary>
        public Priority Priority { get; set; }
        /// <summary>
        /// Get Retry
        /// </summary>
        public int Retry { get; set; }
 
        /// <summary>
        /// Get ArrivedTime
        /// </summary>
        public DateTime ArrivedTime { get; set; }
        /// <summary>
        /// Get SentTime
        /// </summary>
        public DateTime SentTime { get; set; }
 
        /// <summary>
        /// Get or Set Body
        /// </summary>
        public object Body { get; set; }
        /// <summary>
        /// Get or Set Subject
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Get or Set Sender
        /// </summary>
        public string Sender { get; set; }
        /// <summary>
        /// Get or Set Destination
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// Get or Set Status
        /// </summary>
        public ItemState Status { get; set; }
        /// <summary>
        /// Get or Set SenderId
        /// </summary>
        public int SenderId { get; set; }
        /// <summary>
        /// Get or Set OperationId
        /// </summary>
        public int OperationId { get; set; }
        /// <summary>
        /// Get  HasAttach
        /// </summary>
        public bool HasAttach { get; set; }
        /// <summary>
        /// Get or Set Notify
        /// </summary>
        public string Notify { get; set; }
        /// <summary>
        /// Get or Set Price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Get or Set Identifer
        /// </summary>
        public int Identifer { get; set; }
         /// <summary>
        /// Get or Set Label
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// Get or Set TransactionId
        /// </summary>
        public string TransactionId { get; set; }
        /// <summary>
        /// Get or Set AppSpecific
        /// </summary>
        public int AppSpecific { get; set; }
        /// <summary>
        /// Get or Set Segments
        /// </summary>
        public int Segments { get; set; }
        /// <summary>
        /// Get or Set ClientContext
        /// </summary>
        public string ClientContext { get; set; }
        /// <summary>
        /// Get or Set Server
        /// </summary>
        public int Server { get; set; }
        /// <summary>
        /// Get or Set timeout in seconds
        /// </summary>
        public int TimeOut { get; set; }
        /// <summary>
        /// Get or Set Format
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// Get or Set Host/Queue name
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Get indicate wether the item is timeout 
        /// </summary>
        public bool IsTimeOut
        {
            get { return TimeSpan.FromSeconds(TimeOut) < DateTime.Now.Subtract(ArrivedTime); }
        }

        #endregion

        #region attachments

        /// <summary>
        /// Get AttachItems Read only List
        /// </summary>
        [ReadOnly (true)]
        public IList<QueueAttachItem>  AttachItems
        {
            get 
            {
                if (attachItems == null)
                {
                    attachItems = new List<QueueAttachItem>();
                }
                return attachItems.AsReadOnly(); 
            }
        }

 
        /// <summary>
        /// AddAttachment
        /// </summary>
        /// <param name="attachStream"></param>
        /// <param name="attachPath"></param>
        public void AddAttachment(string attachStream,string attachPath)
        {
            if (attachItems == null)
            {
                attachItems = new List<QueueAttachItem>();
            }
 
            attachItems.Add(new QueueAttachItem(this.ItemId,this.MessageId,attachStream,attachPath));
            HasAttach = attachItems.Count > 0;
        }

        /// <summary>
        /// AddAttachment
        /// </summary>
        /// <param name="item"></param>
        public void AddAttachment(QueueAttachItem item)
        {
            if (attachItems == null)
            {
                attachItems = new List<QueueAttachItem>();
            }
            attachItems.Add(item);
            HasAttach = attachItems.Count > 0;
        }
        /// <summary>
        /// AddAttachment
        /// </summary>
        /// <param name="items"></param>
        public void AddAttachment(QueueAttachItem[] items)
        {
            if (attachItems == null)
            {
                attachItems = new List<QueueAttachItem>();
            }
            attachItems.AddRange(items);
            HasAttach = attachItems.Count > 0;
        }

        /// <summary>
        /// AddAttachment
        /// </summary>
        /// <param name="drAttach"></param>
        internal void AddAttachment(DataRow[] drAttach)
        {
            if (drAttach != null)
            {
                if (attachItems != null)
                {
                    attachItems.Clear();
                    attachItems = null;
                }
                attachItems = new List<QueueAttachItem>();
                foreach (DataRow d in drAttach)
                {
                    attachItems.Add(new QueueAttachItem(d));
                }
                HasAttach = attachItems.Count > 0;
            }
        }
        #endregion

        #region methods

        /// <summary>
        ///  Get Copy of Queue item
        /// </summary>
        /// <returns></returns>
        public QueueItem Copy()
        {
            //string xml=this.Serialize();
            //return QueueItem.Deserialize(xml);

            QueueItem item = new QueueItem()
            {
                AppSpecific = this.AppSpecific,
                ArrivedTime = this.ArrivedTime,
                attachItems = this.attachItems,
                Body = this.Body,
                ClientContext = this.ClientContext,
                Destination = this.Destination,
                HasAttach = this.HasAttach,
                Identifer = this.Identifer,
                m_Lock = this.m_Lock,
                ItemId = this.ItemId,
                Label = this.Label,
                MessageId = this.MessageId,
                OperationId = this.OperationId,
                Price = this.Price,
                Priority = this.Priority,
                Notify = this.Notify,
                Retry = this.Retry,
                Segments = this.Segments,
                Sender = this.Sender,
                SenderId = this.SenderId,
                SentTime = this.SentTime,
                Server = this.Server,
                Status = this.Status,
                Subject = this.Subject,
                TimeOut = this.TimeOut,
                TransactionId = this.TransactionId,
                Format = this.Format,
                Host=this.Host
            };
            return item;

        }
        /// <summary>
        /// Serialize QueueItem to xml string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return XmlUtil.Serialize(this);// ObjectToXml.Instance.Serialize(this);
        }

        internal void DoRetry()
        {
            Retry++;
            ItemId = Guid.NewGuid();
        }

        #endregion

        #region ctor

        /// <summary>
        /// QueueItem ctor
        /// </summary>
        /// <param name="priority"></param>
        public QueueItem(/*int messageId,*/ Priority priority)
            : this(priority, DefaultTimeOut)
        {
        }

        /// <summary>
        /// QueueItem ctor with time out
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="timeout"></param>
        public QueueItem(/*Guid itemId, int messageId,*/ Priority priority, TimeSpan timeout)
        {
            this.ItemId = Guid.NewGuid();
            this.Priority = priority;

            this.ArrivedTime = DateTime.Now;
            this.SentTime = DateTime.Now;
            this.TimeOut =(int) timeout.TotalSeconds;// DateTime.Now.AddSeconds((timeout <= 0) ? DefaultTimeOut : timeout);
        }

        /// <summary>
        /// QueueItem ctor with time out
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="priority"></param>
        public QueueItem(Guid itemId, Priority priority)
        {
            this.ItemId = itemId;
            this.Priority = priority;

            this.ArrivedTime = DateTime.Now;
            this.SentTime = DateTime.Now;
            this.TimeOut = (int)DefaultTimeOut.TotalSeconds;// DateTime.Now.AddSeconds((timeout <= 0) ? DefaultTimeOut : timeout);
        }

        /// <summary>
        /// QueueItem ctor with time out
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="priority"></param>
        /// <param name="timeout"></param>
        public QueueItem(Guid itemId, Priority priority, TimeSpan timeout)
        {
            this.ItemId = itemId;
            this.Priority = priority;

            this.ArrivedTime = DateTime.Now;
            this.SentTime = DateTime.Now;
            this.TimeOut = (int)timeout.TotalSeconds;// DateTime.Now.AddSeconds((timeout <= 0) ? DefaultTimeOut : timeout);
        }

        /// <summary>
        /// QueueItem
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="messageId"></param>
        public QueueItem()
            : this(Priority.Normal, DefaultTimeOut)//Guid itemId, int messageId)
        {
       }

  

 
        /// <summary>
        /// QueueItem ctor
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="serializeBody">should serialize body to base 64 string</param>
        public QueueItem(DataRow dr, bool serializeBody)
        {
            if (dr == null)
                return ;
            this.ItemId = Guid.NewGuid();//= new Guid(dr["ItemID"].ToString());
            this.Subject = dr["Subject"].ToString();
            this.MessageId = Types.ToInt(dr["MessageId"], 0);
            this.Priority =(Priority) Types.ToInt(dr["Priority"], 0);
            int itimeOut = Types.ToInt(dr["TimeOut"], DefaultTimeOutInSecond);
            this.TimeOut = (itimeOut <= 0) ? DefaultTimeOutInSecond : itimeOut;
            //this.timeOut = TimeSpan.FromSeconds((double)itimeOut);//DateTime.Now.AddSeconds(DefaultTimeOut));

            object drBody=dr["Body"];
            if (serializeBody && drBody != null)
            {
                try
                {
                    this.Body = NetSerializer.DeserializeFromBase64(dr["Body"].ToString());
                }
                catch
                {
                    this.Body = drBody;
                }
            }
            else
            {
                this.Body = drBody;
            }
            this.Status = (ItemState)Types.ToInt(dr["Status"], 0);
            this.Sender = Types.NZ(dr["Sender"],"");
            this.Destination = dr["Destination"].ToString();
            this.SenderId = Types.ToInt(dr["SenderId"], 0);
            this.OperationId = Types.ToInt(dr["OperationId"], 0);
            this.Identifer = Types.ToInt(dr["Identifer"], 0);
            this.HasAttach = Types.ToBool(dr["HasAttach"], false);
            this.Price = Types.ToDecimal(dr["Price"], 0);
            this.Retry = Types.ToInt(dr["Retry"], 0);
            this.AppSpecific = Types.ToInt(dr["AppSpecific"], 0);
            this.Segments = Types.ToInt(dr["Segments"], 0);
            this.Server = Types.ToInt(dr["Server"], 0);
            this.ClientContext = Types.NZ(dr["ClientContext"], "");
            this.Label = Types.NZ(dr["Label"], "");
            this.Notify = Types.NZ(dr["Notify"], "");
            this.TransactionId = Types.NZ(dr["TransactionId"], "");
            this.Format = Types.NZ(dr["Format"], "");
            this.Host = Types.NZ(dr["Host"], "");
            
            object o = null;
            //o = dr["ArrivedTime"];
            //if (o != null && o != DBNull.Value)
            //    this.arrivedTime = Types.ToDateTime(o.ToString(), DateTime.Now);
            
            this.ArrivedTime = DateTime.Now;

            o = dr["SentTime"];
            if (o != null && o != DBNull.Value)
                this.SentTime = Types.ToDateTime(o.ToString(), DateTime.Now);


            //o = dr["AttachStream"];
            //if (o != null && o != DBNull.Value)
            //{
            //    this.attachStream =  (System.IO.Stream)o;
            //}

 
        }


        /// <summary>
        /// QueueItem ctor
        /// </summary>
        /// <param name="dr">IDictionary</param>
        public QueueItem(IDictionary dr)
        {
            if (dr == null)
                return;
            this.ItemId = (Guid)dr["ItemID"];
            this.Subject = (string)dr["Subject"];
            this.MessageId = (int)dr["MessageId"];
            this.Priority = (Priority)(int)dr["Priority"];
            this.TimeOut = (int)dr["TimeOut"];
            this.Body = dr["Body"];
            this.Status = (ItemState)(int)dr["Status"];
            this.Sender = (string)dr["Sender"];
            this.Destination = (string)dr["Destination"];
            this.SenderId = (int)dr["SenderId"];
            this.OperationId = (int)dr["OperationId"];
            this.Identifer = (int)dr["Identifer"];
            this.HasAttach = (bool)dr["HasAttach"];
            this.Price = (decimal)dr["Price"];
            this.Retry = (int)dr["Retry"];
            this.AppSpecific = (int)dr["AppSpecific"];
            this.Segments = (int)dr["Segments"];
            this.Server = (int)dr["Server"];
            this.ClientContext = (string)dr["ClientContext"];
            this.Label = (string)dr["Label"];
            this.Notify = (string)dr["Notify"];
            this.TransactionId = (string)dr["TransactionId"];
            this.ArrivedTime = (DateTime)dr["ArrivedTime"];
            this.SentTime = (DateTime)dr["SentTime"];
            this.Format = Types.NZ(dr["Format"], "");
            this.Host = Types.NZ(dr["Host"], "");

        }

        //~QueueItem()
        //{
        //    Dispose(false);
        //}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool disposed = false;
        void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (attachItems != null)
                {
                    foreach (QueueAttachItem itm in attachItems)
                    {
                        itm.Dispose();
                    }
                    //attachItems.Clear();
                    attachItems = null;
                }
                Body = null;
                Subject = null;
                Sender = null;
                Destination = null;
                Notify = null;
                TransactionId = null;
                Label = null;
                Format = null;
                Host = null;
                this.disposed = true;
            }

        }

        //public void Dispose()
        //{
        //    if (attachItems != null)
        //    {
        //        foreach (QueueAttachItem itm in attachItems)
        //        {
        //            itm.Dispose();
        //        }
        //        //attachItems.Clear();
        //        attachItems = null;
        //    }
        //    Body = null;
        //    Subject = null;
        //    Sender = null;
        //    Destination = null;
        //    Notify = null;
        //    TransactionId = null;
        //    Label = null;
        //    Format = null;
        //    GC.SuppressFinalize(this);
        //}

        #endregion

        #region util

        internal void SetSentTime()
        {
            if (SentTime <= ArrivedTime)
            {
                this.SentTime = DateTime.Now;
            }
        }

        public double Duration()
        {
          return  SentTime.Subtract(ArrivedTime).TotalSeconds;
        }

        /// <summary>
        /// Deserialize queue item from base64 string
        /// </summary>
        public static QueueItem Deserialize(string serItem)
        {
            object o = NetSerializer.DeserializeFromBase64(serItem);//.DeserializeFromXml(xml, typeof(QueueItem));
            if (o == null)
                return null;
            if (!(o.GetType().IsAssignableFrom(typeof(QueueItem))))
            {
                throw new Exception("Type not valid");
            }
            return (QueueItem)o;
        }

        /// <summary>
        /// Deserialize queue item from byte array
        /// </summary>
        public static QueueItem Deserialize(byte[] bytes)
        {
            object o = NetSerializer.DeserializeFromBytes(bytes);//.DeserializeFromXml(xml, typeof(QueueItem));
            if (o == null)
                return null;
            if (!(o.GetType().IsAssignableFrom(typeof(QueueItem))))
            {
                throw new Exception("Type not valid");
            }
            return (QueueItem)o;
        }

        /// <summary>
        /// Deserialize queue item from stream
        /// </summary>
        public static QueueItem Deserialize(Stream stream)
        {
            object o = NetSerializer.BinDeserialize(stream);
            if (o == null)
                return null;
            if (!(o.GetType().IsAssignableFrom(typeof(QueueItem))))
            {
                throw new Exception("Type not valid");
            }
            return (QueueItem)o;
        }

        public static QueueItem ReadFile(string filename)
        {
            if (!File.Exists(filename))
            {
                return null;
            }
            return  Deserialize(File.ReadAllBytes(filename));

        }

        /// <summary>
        /// Serialize Queue Item
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            return NetSerializer.SerializeToBase64(this);//.SerializeToXml(this,this.GetType());
        }

        /// <summary>
        /// Serialize Queue Item To ByteArray
        /// </summary>
        /// <returns></returns>
        public byte[] ToByteArray()
        {
            return NetSerializer.SerializeToBytes(this);
        }

        /// <summary>
        /// Stores Queue Item it's child entities to specified stream.
        /// </summary>
        /// <param name="storeStream">Stream where to store mime entity.</param>
        public void ToStream(System.IO.Stream storeStream)
        {
            byte[] data = ToByteArray();
            storeStream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Get Queue Item as Stream
        /// </summary>
        /// <returns></returns>
        public Stream GetStream()
        {
            return new MemoryStream(ToByteArray());
        }

        public string ToFileName(string path)
        {
            return SysUtil.PathFix(path + "\\" + ItemId.ToString() + ".mcq");
        }

        public void Save(string path)
        {
            string filename = ToFileName(path);
            File.WriteAllBytes(filename, ToByteArray());

            //using (FileStream fs = File.Create(filename))
            //{
            //    fs.BeginWrite
            //    ToStream(fs);
            //    SysUtil.StreamCopy(message, fs);

            //    // Create message info file for the specified relay message.
            //    RelayMessageInfo messageInfo = new RelayMessageInfo(sender, to, date, false, targetHost);

            //    File.WriteAllBytes(filename, ToByteArray());
            //}
        }

        public void Delete(string path)
        {
            SysUtil.DeleteFile(ToFileName(path));
        }

        public DataRow ToDataRow()
        {
            //System.Runtime.Serialization.ISerializable;
            //System.Xml.Serialization.IXmlSerializable
            DataRow dr = SQLCMD.GetQueueItemRowSchema();
            FillDataRow(dr);
            return dr;
        }

        internal void FillDataRow(DataRow dr)
        {

            dr["ItemID"] = ItemId;
            dr["Status"] = this.Status;
            dr["MessageId"] = this.MessageId;
            dr["Priority"] = this.Priority;
            dr["Retry"] = this.Retry;
            dr["ArrivedTime"] = this.ArrivedTime;
            dr["SentTime"] = this.SentTime;
            dr["Body"] = Types.NZ(this.Body, "");
            dr["Subject"] = Types.NZ(this.Subject, "");
            dr["Sender"] = Types.NZ(this.Sender, "");
            dr["Destination"] = Types.NZ(this.Destination, "");
            dr["SenderId"] = this.SenderId;
            dr["OperationId"] = this.OperationId;
            dr["HasAttach"] = this.HasAttach;
            dr["Notify"] = Types.NZ(this.Notify, "");
            dr["Price"] = this.Price;
            dr["Identifer"] = this.Identifer;
            dr["Label"] = Types.NZ(this.Label, "");
            dr["TransactionId"] = Types.NZ(this.TransactionId, "");
            dr["AppSpecific"] = this.AppSpecific;
            dr["Segments"] = this.Segments;
            dr["ClientContext"] = Types.NZ(this.ClientContext, "");
            dr["Server"] = this.Server;
            dr["TimeOut"] = this.TimeOut;
            dr["Format"] = this.Format;
            dr["Host"] = this.Host;
        }


        //public IDictionary Properties
        //{
        //    get { return this.ToDictionary(); }
        //}

        public IDictionary ToDictionary()
        {
            IDictionary dr = new Hashtable();
            dr["ItemID"] = ItemId;
            dr["Status"] = (int)this.Status;
            dr["MessageId"] = this.MessageId;
            dr["Priority"] = (int)this.Priority;
            dr["Retry"] = this.Retry;
            dr["ArrivedTime"] = this.ArrivedTime;
            dr["SentTime"] = this.SentTime;
            dr["Body"] = this.Body;
            dr["Subject"] = this.Subject;
            dr["Sender"] = this.Sender;
            dr["Destination"] = this.Destination;
            dr["SenderId"] = this.SenderId;
            dr["OperationId"] = this.OperationId;
            dr["HasAttach"] = this.HasAttach;
            dr["Notify"] = this.Notify;
            dr["Price"] = this.Price;
            dr["Identifer"] = this.Identifer;
            dr["Label"] = this.Label;
            dr["TransactionId"] = this.TransactionId;
            dr["AppSpecific"] = this.AppSpecific;
            dr["Segments"] = this.Segments;
            dr["ClientContext"] = this.ClientContext;
            dr["Server"] = this.Server;
            dr["TimeOut"] = this.TimeOut;
            dr["Format"] = this.Format;
            dr["Host"] = this.Host;
            return dr;
        }
        #endregion

    }

    [Serializable]
    public class QueueAttachItem : IDisposable
    {
        #region memebers

        private Guid attachId;
        private int msgId;
        private string attachStream;
        private string attachPath;

        #endregion

        #region property

        /// <summary>
        /// Get ItemId
        /// </summary>
        public Guid AttachId { get { return attachId; } }
        /// <summary>
        /// Get MessageId
        /// </summary>
        public int MsgId { get { return msgId; } }
        /// <summary>
        /// Get or Set AttachStream
        /// </summary>
        public string AttachStream
        {
            get { return attachStream; }
            set
            {
                attachStream = value;
            }
        }
        /// <summary>
        /// Get or Set AttachPath
        /// </summary>
        public string AttachPath
        {
            get { return attachPath; }
            set
            {
                attachPath = value;
            }
        }

        #endregion

        /// <summary>
        /// Serialize QueueItem to xml string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return XmlUtil.Serialize(this);// ObjectToXml.Instance.Serialize(this);
        }

        /// <summary>
        /// Serialize QueueItem to xml string
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            return NetSerializer.SerializeToBase64(this);// XmlUtil.Serialize(this);// ObjectToXml.Instance.Serialize(this);
        }
        /// <summary>
        /// Serialize Queue Item To ByteArray
        /// </summary>
        /// <returns></returns>
        public byte[] ToByteArray()
        {
            return NetSerializer.SerializeToBytes(this);
        }
        /// <summary>
        /// Deserialize queue item from base64 string
        /// </summary>
        public static QueueAttachItem Deserialize(string serItem)
        {
            object o = NetSerializer.DeserializeFromBase64(serItem);
            if (o == null)
                return null;
            if (!(o.GetType().IsAssignableFrom(typeof(QueueItem))))
            {
                throw new Exception("Type not valid");
            }
            return (QueueAttachItem)o;
        }

        /// <summary>
        /// Deserialize queue item from byte array
        /// </summary>
        public static QueueAttachItem Deserialize(byte[] bytes)
        {
            object o = NetSerializer.DeserializeFromBytes(bytes);
            if (o == null)
                return null;
            if (!(o.GetType().IsAssignableFrom(typeof(QueueAttachItem))))
            {
                throw new Exception("Type not valid");
            }
            return (QueueAttachItem)o;
        }


        #region ctor

        /// <summary>
        /// QueueAttachItem ctor
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="messageId"></param>
        public QueueAttachItem(Guid itemId, int messageId)
        {
            this.attachId = itemId;
            this.msgId = messageId;
        }

         public QueueAttachItem(Guid itemId, int messageId,string stream,string link)
        {
            this.attachId = itemId;
            this.msgId = messageId;
            this.attachStream = stream;
            if (!string.IsNullOrEmpty(link))
            {
                this.attachPath = link;// new Uri(link);
            }
        }

         /// <summary>
        /// QueueItem ctor
        /// </summary>
        /// <param name="dr"></param>
        public QueueAttachItem(DataRow dr)
        {
            if (dr == null)
                return ;
            this.attachId = new Guid(dr["AttachId"].ToString());
            this.msgId = Types.ToInt(dr["MsgId"], 0);
            object o = null;
            o=dr["AttachStream"];
            if (o != null && o != DBNull.Value)
            {
                this.attachStream = o.ToString();
            }
            o = dr["AttachPath"];
            if (o != null && o != DBNull.Value)
            {
                this.attachPath = o.ToString();//new Uri(o.ToString());
            }
        }

        public void Dispose()
        {
            attachStream = null;
            GC.SuppressFinalize(this);
        }

        //private bool disposed = false;


        //private void Dispose(bool disposing)
        //{
        //    if (!this.disposed)
        //    {
        //        if (disposing)
        //        {
        //            attachStream = null;
        //        }

        //        // Call the appropriate methods to clean up 
        //        // unmanaged resources here.
        //        // If disposing is false, 
        //        // only the following code is executed.
        //        //CloseHandle(handle);
        //        //handle = IntPtr.Zero;
        //    }
        //    disposed = true;
        //}

        // Use interop to call the method necessary  
        // to clean up the unmanaged resource.
        //[System.Runtime.InteropServices.DllImport("Kernel32")]
        //private extern static Boolean CloseHandle(IntPtr handle);

        #endregion

        #region util

        //public DataRow ToDataRow()
        //{
        //    DataRow dr = SQLCMD.GetQueueAttachItemRowSchema();
        //    FillDataRow(dr);
        //    return dr;
        //}

        internal void FillDataRow(DataRow dr)
        {
            dr["AttachId"] = attachId;
            dr["MsgId"] = this.msgId;
            dr["AttachStream"] = this.attachStream;
            dr["AttachPath"] = this.attachPath;
  
        }



        #endregion

    }

    /// <summary>
    /// This class holds Relay_Queue queued item.
    /// </summary>
    public class Relay_QueueItem
    {
        private string m_Queue = null;
        private string m_From = "";
        private string m_To = "";
        private string m_MessageID = "";
        private Stream m_MessageStream = null;
        private object m_Tag = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="queue">Item owner queue.</param>
        /// <param name="from">Sender address.</param>
        /// <param name="to">Target recipient address.</param>
        /// <param name="messageID">Message ID.</param>
        /// <param name="message">Raw mime message. Message reading starts from current position.</param>
        /// <param name="tag">User data.</param>
        internal Relay_QueueItem(string queue, string from, string to, string messageID, Stream message, object tag)
        {
            m_Queue = queue;
            m_From = from;
            m_To = to;
            m_MessageID = messageID;
            m_MessageStream = message;
            m_Tag = tag;
        }


        #region Properties Implementation

        /// <summary>
        /// Gets this relay item owner queue.
        /// </summary>
        public string Queue
        {
            get { return m_Queue; }
        }

        /// <summary>
        /// Gets from address.
        /// </summary>
        public string From
        {
            get { return m_From; }
        }

        /// <summary>
        /// Gets target recipient.
        /// </summary>
        public string To
        {
            get { return m_To; }
        }

        /// <summary>
        /// Gets message ID which is being relayed now.
        /// </summary>
        public string MessageID
        {
            get { return m_MessageID; }
        }

        /// <summary>
        /// Gets raw mime message which must be relayed.
        /// </summary>
        public Stream MessageStream
        {
            get { return m_MessageStream; }
        }

        /// <summary>
        /// Gets or sets user data.
        /// </summary>
        public object Tag
        {
            get { return m_Tag; }

            set { m_Tag = value; }
        }

        #endregion

    }
}
