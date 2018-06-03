using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using Nistec.Data.SqlClient;
using Nistec.Data;
using Nistec.Data.Factory;
using Nistec.Data.Entities;
using Nistec;
using Nistec.Generic;
using Nistec.Serialization;

namespace Netcell.Data.DbServices.Entities
{

    public enum TemplateService
    {
        MailNotifyMessage = 1,
        CellNotifyMessage = 2,
        CampaignCellNotifyBegin = 3,
        CampaignCellNotifyEnd = 4,
        CampaignMailNotifyBegin = 5,
        CampaignMailNotifyEnd = 6,
        UserLogin = 7,
        CreditAlert=8
    }

   [Entity("TemplateEntity", "Templates", "cnn_Services", EntityMode.Generic, "TemplateId")]
    public class TemplateEntity_Context : EntityContext<TemplateEntity>
    {
        #region ctor
       public TemplateEntity_Context(int TemplateId)
           : base(TemplateId)
        {

        }
        public TemplateEntity_Context(DataFilter filter)
            : base()
        {
            Init(filter);
        }
        public TemplateEntity_Context(TemplateEntity item)
            : base(item)
        {

        }
        protected TemplateEntity_Context()
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

        public static TemplateEntity Get(TemplateService TemplateId)
        {
            return Get((int)TemplateId);
        }

        public static TemplateEntity Get(int TemplateId)
        {
            using (TemplateEntity_Context context = new TemplateEntity_Context(TemplateId))
            {
                return context.Entity;
            }
        }

        public static TemplateEntity Get(DataRow dr)
        {
            using (TemplateEntity_Context context = new TemplateEntity_Context())
            {
                context.EntityRecord = new GenericRecord(dr);
                return context.Entity;
            }
        }

        public static IList<TemplateEntity> GetListItems()
        {
            using (TemplateEntity_Context context = new TemplateEntity_Context())
            {
                return context.EntityList();
            }
        }

        public static DataTable GetList(int ItemId)
        {
            using (var cmd = Netcell_Services.Instance)
            {
                return cmd.ExecuteCommand<DataTable>("select TemplateId,TemplateName from Templates", CommandType.Text, null);
            }
        }

        public static TemplateEntity Deserialize(string base64)
        {
            return NetSerializer.DeserializeFromBase64<TemplateEntity>(base64);
        }

        #endregion

        #region messages
       /*
        public static string CreateCampaignMessage(TemplateService template, int accountId, int campaignId, string url)
        {
            TemplateEntity en = Get((int)template);
            string html = en.Body;
 
            //string sub = Subject;
            html = html.Replace("#campaignid#", campaignId.ToString());
            html = html.Replace("#linkshow#", url);
            //subject = sub.Replace("#campaignid#", campaignId.ToString());

            return html;
        }

        public static string CreateUserLoginMessage(string userName, string login, string pass)
        {
            TemplateEntity en = Get((int)TemplateService.UserLogin);
            string html = en.Body;
            html = html.Replace("#username#", userName);
            html = html.Replace("#loginname#", login);
            html = html.Replace("#pass#", pass);
            return html;
        }

        public static string CreateCreditAlertMessage(int accountId, decimal credit)
        {
            TemplateEntity en = Get((int)TemplateService.CreditAlert);
            string html = en.Body;
            html = html.Replace("#accountid#", accountId.ToString());
            html = html.Replace("#credit#", credit.ToString());
            return html;
        }
       */
        #endregion
    }

    [Serializable]
    public class TemplateEntity : IEntityItem
    {
  
        #region Ex Properties

        public bool IsEmpty
        {
            get { return Types.IsEmpty(TemplateId); }
        }

        #endregion

        #region properties

     
        [EntityProperty(EntityPropertyType.Key)]
         public int TemplateId
         {
             get;
             set;
         }

        [EntityProperty(EntityPropertyType.Default)]
        public string TemplateName
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string Subject
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string Body
        {
            get;
            set;
        }
         
        #endregion

        #region context

        public string CreateCampaignMessage(int accountId, int campaignId, string url)
        {
             string html = Body;

            //string sub = Subject;
            html = html.Replace("#campaignid#", campaignId.ToString());
            html = html.Replace("#linkshow#", url);
            //subject = sub.Replace("#campaignid#", campaignId.ToString());

            return html;
        }

        public string CreateUserLoginMessage(string userName, string login, string pass)
        {
            string html = Body;
            html = html.Replace("#username#", userName);
            html = html.Replace("#loginname#", login);
            html = html.Replace("#pass#", pass);
            return html;
        }

        public string CreateCreditAlertMessage(int accountId, decimal credit)
        {
            string html = Body;
            html = html.Replace("#accountid#", accountId.ToString());
            html = html.Replace("#credit#", credit.ToString());
            return html;
        }


        #endregion

        #region methods

        public string Serialize()
        {
            return NetSerializer.SerializeToBase64(this);
        }

        public string Print()
        {
            return string.Format("TemplateEntity Item TemplateId:{0}, TemplateName:{1}, Subject:{2}", TemplateId, TemplateName, Subject);
        }
        
        #endregion
    }

 
}
