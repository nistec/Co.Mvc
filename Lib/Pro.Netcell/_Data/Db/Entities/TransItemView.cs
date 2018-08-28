using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using Nistec.Data;
using Nistec.Data.Entities;
//using Nistec.Sys;
using Nistec.Data.Factory;
using Nistec.Runtime;

namespace Netcell.Data.Db.Entities
{

    [Entity("Trans_Item", "vw_Trans_Items_b", "cnn_Netcell", EntityMode.Generic, "MessageId")]
    public class TransItemView_Context : EntityContext<TransItemView>
    {
        public TransItemView_Context(int messageId)
            : base(messageId)
        {
            
        }

        public TransItemView_Context(TransItemView item)
            : base(item)
        {

        }

        public string ToHtml(bool encrypt)
        {
            string h = this.EntityProperties.ToHtmlTable("", "");
            if (encrypt)
                return Encryption.ToBase64String(h, true);
            return h;
        }

        #region binding

        protected override void EntityBind()
        {
            //base.EntityDb.EntityCulture = Netcell.Data.DB.NetcellDB.GetCulture();
            //If EntityAttribute not define you can initilaize the entity here
            //base.InitEntity<AdventureWorks>("Contact", "Person.Contact", EntityKeys.Get("ContactID"));
        }

        #endregion

        #region methods

        public static DataTable GetList()
        {
            DataTable dt = null;
            using (IDbCmd cmd = NetcellDB.Instance.NewCmd())
            {
                dt = cmd.ExecuteCommand<DataTable>("select top 10 * from Person.Contact", true);
            }

            return dt;
        }

        #endregion
    }

    [Entity("Trans_Item", "vw_Trans_Items_Cell_b", "cnn_Netcell", EntityMode.Generic, "MessageId")]
    public class TransItemCell_Context : EntityContext<TransItemView>
    {
        public TransItemCell_Context(int messageId)
            : base(messageId)
        {

        }

        public string ToHtml(bool encrypt)
        {
            string h = this.EntityProperties.ToHtmlTable("", "");
            if (encrypt)
                return Encryption.ToBase64String(h, true);
            return h;
        }

        public static TransItemView Get(int MessageId)
        {
            using (TransItemCell_Context context = new TransItemCell_Context(MessageId))
            {
                return context.Entity;
            }
        }
    }

    [Entity("Trans_Item", "vw_Trans_Items_Mail_b", "cnn_Netcell", EntityMode.Generic, "MessageId")]
    public class TransItemMail_Context : EntityContext<TransItemView>
    {
        public TransItemMail_Context(int messageId)
            : base(messageId)
        {

        }

        public string ToHtml(bool encrypt)
        {
            string h = this.EntityProperties.ToHtmlTable("", "");
            if (encrypt)
                return Encryption.ToBase64String(h, true);
            return h;
        }

        public static TransItemView Get(int MessageId)
        {
            using (TransItemMail_Context context = new TransItemMail_Context(MessageId))
            {
                return context.Entity;
            }
        }
    }

    public class TransItemView : IEntityItem
    {

        #region Properties

        [EntityProperty(EntityPropertyType.Default, Caption = "�����")]
        public int Status
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "���� �����")]
        public string NotifyStatus
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��� �����")]
        public int AckStatus
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "�����")]
        public int MessageId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��-�����")]
        public int BatchId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����")]
        public string Target
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��-�����")]
        public int CampaignId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��������")]
        public int Platform
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "���")]
        public string Sender
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��-������")]
        public int ItemIndex
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����")]
        public int OperatorId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "�����")]
        public int OpId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����")]
        public decimal Price
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��-����")]
        public int Units
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����")]
        public int Size
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����� �����")]
        public DateTime SentTime
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "�����")]
        public int AccountId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��� ����")]
        public int MtId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "���� �����")]
        public DateTime SendTime
        {
            get;
            set;
        }

        #endregion

        public string ToHtml(bool encrypt)
        {
            TransItemView_Context context=new TransItemView_Context(this);
            string h=context.EntityProperties.ToHtmlTable("","");

            //string h = base.EntityProperties.ToHtmlTable("", "");
            if (encrypt)
                return Encryption.ToBase64String(h, true);
            return h;
        }
    }
 
}
