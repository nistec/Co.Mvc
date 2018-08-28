using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using Nistec.Data;
using Nistec.Data.Entities;
//using Nistec.Sys;
using Netcell.Data.Client;
using Nistec.Data.Factory;
using Nistec;
using Netcell.Data.Server;


namespace Netcell.Data.Db.Entities
{
  
    [Entity("User_Info", "Users", "cnn_Netcell", EntityMode.Generic, "UserId")]
    public class Users_Context : EntityContext<User_Info>
    {
        #region ctor
        public Users_Context(int UserId)
            : base(UserId)
        {

        }

        public Users_Context(User_Info item)
            : base(item)
        {

        }

        public Users_Context(DataFilter filter)
            : base()
        {
            Init(filter);
        }
        

        private Users_Context()
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

        #region methods
        
        
        public static User_Info Get(int userId)
        {
            using (Users_Context context = new Users_Context(userId))
            {
                return context.Entity;
            }
        }

        public static User_Info Get(string LogInName, string Pass)
        {

            using (Users_Context context = new Users_Context(
                DataFilter.Get("LogInName=@LogInName and Pass=@Pass", LogInName, Pass)))
            {
                return context.Entity;
            }

            //using (Users_Context context = new Users_Context())
            //{
            //    context.Set(@"SELECT top 1 * from [Users] where LogInName=@LogInName and Pass=@Pass",
            //        DataParameter.Get("LogInName", LogInName, "Pass", Pass),
            //        CommandType.Text
            //        );
            //    return context.Entity;
            //}
        }

        public static User_Info Get(string LogInName, string Pass, string Ip)
        {
            using (Users_Context context = new Users_Context())
            {
                context.Set(@"sp_Auth_WithIp", DataParameter.GetSql("LogInName", LogInName, "Pass", Pass, "Ip", Ip), CommandType.StoredProcedure);
                return context.Entity;
            }
        }

        public static int ValidateUserAccount(string LogInName, string Pass, int accountId, string Ip)
        {
            int userId = 0;

            using (DalRule dal = new DalRule())
            {
                dal.User_Auth_Lookup(LogInName, Pass, Ip, ref accountId, ref userId);
            }

            return userId;
        }

        public static int ValidateAccount(string LogInName, string Pass, string Ip)
        {
            int accountId = 0;
            int userId = 0;

            using (DalRule dal = new DalRule())
            {
                dal.User_Auth_Lookup(LogInName, Pass, Ip, ref accountId, ref userId);
            }

            return accountId;
        }

        public static int ValidateManager(string LogInName, string Pass, string Ip)
        {
            int accountId = 0;
            using (DalRule dal = new DalRule())
            {
                dal.Auth_Manager(LogInName, Pass, Ip, ref accountId);
            }

            return accountId;
        }

        public static List<User_Info> GetListItems(int accountId)
        {
            using (Users_Context context = new Users_Context())
            {
                return context.EntityList(DataFilter.Get("AccountId=@AccountId", accountId));
            }
        }


        #endregion
    }

    public class User_Info : IEntityItem
    {
        #region Properties

       [EntityProperty(EntityPropertyType.Identity, Caption = "מס.משתמש")]
        public int UserId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "חשבון")]
        public int AccountId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "שם כניסה")]
        public string LogInName
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "פרטים")]
        public string Details
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "שפה")]
        public string Lang
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "דואל")]
        public string MailAddress
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "טלפון")]
        public string Phone
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "נוצר ב")]
        public DateTime Creation
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מועד כניסה אחרון")]
        public DateTime LastLoggedIn
        {
            get;
            set;
        }
        
        [EntityProperty(EntityPropertyType.Default, Caption = "חסום")]
        public bool IsBlocked
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "אישור תקנון")]
        public bool ConfirmArticle
        {
            get;
            set;
        }
       [EntityProperty(EntityPropertyType.Default, Caption = "פעיל")]
        public bool IsActive
        {
            get;
            set;
        }
  
        [EntityProperty(EntityPropertyType.Default, Caption = "סוג משתמש")]
        public int UserType
        {
            get;
            set;
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "ארגומנטים")]
        public string PermsBits
        {
            get;
            set;
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "נסיון")]
        public int Evaluation
        {
            get;
            set;
        }

      //,[PassQuestion]
      //,[PassAnswer]
      //,[Perms]
      //,[ShowDashboard]

        #endregion
    }

}
