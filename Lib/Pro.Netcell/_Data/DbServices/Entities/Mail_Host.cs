using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Nistec.Data;
using Netcell.Data;
using Nistec.Threading;
using System.Threading;
using Nistec.Data.Entities;
using Nistec;
using Nistec.Data.Factory;
using Netcell.Data.Db.Entities;
using Nistec.Generic;

namespace Netcell.Data.DbServices.Entities
{

    //[Entity("Mail_Host", "Mail_Host", "cnn_Services", EntityMode.Generic, "AccountId,HostId,Priority")]
    [Entity("Mailer_Host", "Mailer_Host", "cnn_Services", EntityMode.Generic, "HostId")]
    public class Mail_Host_Context : EntityContext<Mail_Host>
    {
        #region ctor
       //public Mail_Host_Context(int AccountId, string HostId, int Priority)
       //    : base(AccountId, HostId, Priority)
       // {

       // }
       //public Mail_Host_Context(int AccountId, string HostId)
       //    : base(AccountId, HostId, 0)
       //{

       //}
       public Mail_Host_Context(string HostId)
           : base(0, HostId, 0)
       {

       }
       
       public Mail_Host_Context(DataFilter filter)
            : base()
        {
            Init(filter);
        }
        public Mail_Host_Context(Mail_Host item)
            : base(item)
        {

        }
        protected Mail_Host_Context()
            : base()
        {
        }
        #endregion

        #region binding

        protected override void EntityBind()
        {
            //base.EntityDb.EntityCulture = Netcell.Data.DB.NetcellDB.GetCulture();
            //If EntityAttribute not define you can initilaize the entity here
            //base.InitEntity<AdventureWorks>("Contact", "Person.Contact", EntityKeys.Get("ContactID"));
        }

        #endregion

        #region methods static

        //public static Mail_Host Get(int AccountId, string HostId)
        //{
        //    using (Mail_Host_Context context = new Mail_Host_Context(AccountId,HostId))
        //    {
        //        return context.Entity;
        //    }
        //}

        public static Mail_Host Get(string HostId)
        {
            using (Mail_Host_Context context = new Mail_Host_Context(HostId))
            {
                return context.Entity;
            }
        }

        public static Mail_Host Get(int AccountId)
        {
            DataRow dr = null;
            using (IDbCmd cmd = Netcell_Services.Instance.NewCmd())
            {
                dr = cmd.ExecuteCommand<DataRow>("SELECT top 1 * from [vw_Mailer_Host] where (AccountId=@AccountId or AccountId=0) and IsActive=1 order by AccountId desc", DataParameter.GetSql("AccountId", AccountId), CommandType.Text);
             }
            if (dr == null)
            {
                return null;
            }
            return Get(dr);
        }

        //public static Mail_Host Get(int AccountId, string HostType)
        //{
        //    DataRow dr = null;
        //    using (IDbCmd cmd = Netcell_Services.Instance.NewCmd())
        //    {
        //        dr = cmd.ExecuteCommand<DataRow>("SELECT top 1 * from [Mail_Host] where (AccountId=@AccountId or AccountId=0)and (HostType=@HostType) and IsActive=1 order by AccountId desc", DataParameter.Get("AccountId", AccountId, "HostType", HostType), CommandType.Text);
        //    }
        //    if (dr == null)
        //    {
        //        return null;
        //    }
        //    return Get(dr);
        //}

        public static Mail_Host Get(DataRow dr)
        {
            using (Mail_Host_Context context = new Mail_Host_Context())
            {
                context.EntityRecord = new GenericRecord(dr);
                return context.Entity;
            }
        }

        //public static DataTable GetListTable(int Server)
        //{
        //    using (IDbCmd cmd = Netcell_Services.Instance.NewCmd())
        //    {
        //        return cmd.ExecuteCommand<DataTable>("SELECT * from [Mail_Host] where (IsActive=@IsActive  and Server=@Server", DataParameter.Get("IsActive", 1, "Server", Server));
        //    }
        //}

        //public static List<Mail_Host> GetListItems(int server)
        //{
        //    using (Mail_Host_Context context = new Mail_Host_Context())
        //    {
        //        return context.EntityList(DataFilter.Get("IsActive=@IsActive  and Server=@Server", 1, server));
        //    }
        //}

        public static List<Mail_Host> GetListItems()
        {
            using (Mail_Host_Context context = new Mail_Host_Context())
            {
                return context.EntityList(DataFilter.Get("IsActive=@IsActive", 1));
            }
        }

        public static Mail_Host Deserialize(string base64)
        {
            return Nistec.Serialization.BinarySerializer.DeserializeFromBase64<Mail_Host>(base64);
        }

        #endregion
    }

   [Serializable]
   public class Mail_Host : IEntityItem, IMailHost
   {

       #region static

       public static IMailHost Create(string hostId)
       {
           var mcnn = Mail_Host_Context.Get(hostId);
           return mcnn as IMailHost;
       }
       //public static IMailHost CreateConnection(int accountId, string hostType)
       //{
       //    var mcnn=  Mail_Host_Context.Get(accountId, hostType);
       //    return mcnn as IMailHost;
       //}
       public static IMailHost CreateConnection(int accountId)
       {
           var mcnn = Mail_Host_Context.Get(accountId);
           return mcnn as IMailHost;
       }
       public static IMailHost CreateDefaultConnection(int accountId)
       {
           return CreateConnection(accountId);//, HostTypes.Default);
       }
      

       #endregion

       #region Ex Properties

       public bool IsEmpty
       {
           get { return Types.IsEmpty(HostId); }
       }

       #endregion

       #region properties

       //[EntityProperty(EntityPropertyType.Key, false)]
       //public int AccountId { get; set; }

       [EntityProperty(EntityPropertyType.Key, false)]
       public string HostId { get; set; }

       [EntityProperty(EntityPropertyType.Key, false)]
       public int Priority { get; set; }

       [EntityProperty(EntityPropertyType.Default, false)]
       public string HostType { get; set; }

       //[EntityProperty(EntityPropertyType.Default, false)]
       //public int Port { get; set; }

       [EntityProperty(EntityPropertyType.Default, false)]
       public string Host { get; set; }

       //[EntityProperty(EntityPropertyType.Default, false)]
       //public string UserName { get; set; }

       //[EntityProperty(EntityPropertyType.Default, false)]
       //public string Password { get; set; }

       [EntityProperty(EntityPropertyType.Default, false)]
       public string Domain { get; set; }

       //[EntityProperty(EntityPropertyType.Default, false)]
       //public string QueuePath { get; set; }

       [EntityProperty(EntityPropertyType.Default, false)]
       public bool IsActive { get; set; }
       
       [EntityProperty(EntityPropertyType.Default, false)]
       public bool EnableChunk { get; set; }

       //[EntityProperty(EntityPropertyType.Default, false)]
       //public bool EnableQuick { get; set; }


       [EntityProperty(EntityPropertyType.Default, false)]
       public int ChunkMode { get; set; }


       //[EntityProperty(EntityPropertyType.Default, false)]
       //public int Server { get; set; }

       [EntityProperty(EntityPropertyType.Default, false)]
       public int OperatorId { get; set; }
       
       #endregion

       #region context
       #endregion

       #region methods

       public string Serialize()
       {
           return Nistec.Serialization.BinarySerializer.SerializeToBase64(this);
       }

       public string Print()
       {
           return string.Format("Mail_Host Item HostId:{0}", HostId);
       }

       public bool ShouldEnableChunk(int count)
       {
           if (!EnableChunk)
               return false;
           return count > 1;//= ViewConfig.MinChunkItems;
       }

       public string GetMailSender(string from)
       {
           string domain = Domain;
           string name = from.Split(new char[] { '@' })[0];
           return string.Format("{0}@{1}", name, domain);
       }


       
       [EntityProperty(EntityPropertyType.NA)]
       public string ChunkQueueName
       {
           get { return HostId /*+ "_Chunk"*/; }
       }

       public void Dispose()
       {

       }

       #endregion
   }

   //public static class HostTypes
   //{
   //    public const string Default = "Default";
   //    //public const string Quick = "Quick";
   //}

 
   public interface IMailHost
   {
       //int AccountId { get; }

       string HostId { get; }

       int Priority { get; }

       string HostType { get; }

       string Host { get; }

       //int Port { get; }

       //string UserName { get; }

       //string Password { get; }

       string Domain { get; }

       //string QueuePath { get; }

       //bool IsEmpty { get; }

 
       bool EnableChunk { get; }

       int ChunkMode { get; }

       //int Server { get; }

       int OperatorId { get; }

       bool ShouldEnableChunk(int count);

       string GetMailSender(string from);

       void Dispose();

   }


 
}
