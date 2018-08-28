using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Pro.Data;
using Nistec.Data;
using Nistec;
using Pro.Lib;
using System.Data;

namespace Pro.Data.Entities
{

    [Entity(EntityName = "ManagementView", MappingName = "Management", ConnectionKey = "cnn_pro", EntityKey = new string[] { "MemberId,AccountId" })]
    public class ManagementContext : EntityContext<ManagementView>
    {
        #region ctor

        public ManagementContext()
        {
        }

        public ManagementContext(string MemberId,int AccountId)
            : base(MemberId, AccountId)
        {
        }

        public ManagementContext(int RecordId)
            : base()
        {
            SetByParam("RecordId", RecordId);
        }

        #endregion

        #region update

        public static int DoSave(ManagementView entity)
        {
            if(entity.RecordId>0)
                return DoSave(entity.MemberId, entity.AccountId, entity, UpdateCommandType.Update);
            return DoSave(entity.MemberId, entity.AccountId, entity, UpdateCommandType.Insert);

        }
        public static int DoSave(string MemberId, int AccountId, ManagementView entity, UpdateCommandType commandType)
        {
            if (commandType == UpdateCommandType.Delete)
                using (ManagementContext context = new ManagementContext(MemberId, AccountId))
                {
                    return context.SaveChanges(commandType);
                }

            EntityValidator.Validate(entity, "חבר", "he");

            if (commandType == UpdateCommandType.Insert)
                using (ManagementContext context = new ManagementContext())
                {
                    context.Set(entity);
                    return context.SaveChanges(commandType);
                }

            if (commandType == UpdateCommandType.Update)
                using (ManagementContext context = new ManagementContext(MemberId, AccountId))
                {
                    context.Set(entity);
                    return context.SaveChanges(commandType);
                }
            return 0;
        }

        #endregion

        #region static

        public static ManagementView Get(int RecordId)
        {
            using (ManagementContext context = new ManagementContext(RecordId))
            {
                return context.Entity;
            }
        }

        public static ManagementView Get(string MemberId, int AccountId)
        {
            using (ManagementContext context = new ManagementContext(MemberId, AccountId))
            {
                return context.Entity;
            }
        }

 

        //public static List<MemberItem> GetItems()
        //{
        //    using (MemberContext context = new MemberContext())
        //    {
        //        return context.EntityList();
        //    }
        //}

        #endregion
    }

    [EntityMapping("vw_Management")]
    public class ManagementListView : ManagementView
    {

        public static IEnumerable<ManagementListView> ViewList(int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemList<ManagementListView>(MappingName, "AccountId", AccountId);
        }

        public const string MappingName = "vw_Management";
        
        public string MemberName { get; set; }
        public string CityName { get; set; }
        public string BranchName { get; set; }
        public string RoleName { get; set; }
        
        
    }

    [EntityMapping("Management")]
    public class ManagementView : IEntityItem
    {

        public static IEnumerable<ManagementView> View()
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemList<ManagementView>(MappingName, null);
        }
         public static ManagementView Get(int MemberId)
        {
            if (MemberId == 0)
                return new ManagementView();
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<ManagementView>(MappingName, "MemberId", MemberId);
        }
        public const string MappingName = "Management";


        [EntityProperty(EntityPropertyType.Key)]
        public string MemberId { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }
        public int RoleId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public int City { get; set; }
        public int Branch { get; set; }
        public string CellPhone { get; set; }
        public string Phone { get; set; }
        //public string Email { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime LastUpdate { get; set; }

        [EntityProperty(EntityPropertyType.Identity)]
        public int RecordId	{ get; set; }



        public string ToHtml()
        {
            return EntityProperties.ToHtmlTable<ManagementView>(this, null, null, true);
        }

    
    }
}
