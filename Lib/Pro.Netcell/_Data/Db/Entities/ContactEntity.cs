using System;
using System.Data;
using System.Collections;
using System.ComponentModel;
using Nistec;
using Netcell.Data.Client;
using Nistec.Data;
using Nistec.Data.Entities;
using Netcell.Data.Common;
using Netcell.Data.Db;
using Nistec.Data.Factory;
using System.Collections.Generic;
using Nistec.Runtime;
//using Nistec.Sys;

namespace Netcell.Data.Db.Entities
{

    [Entity("ContactsItem", "Contacts_Items", "cnn_Netcell", EntityMode.Generic, "ContactId")]
    public class Contact_Context : EntityContext<ContactEntity>
    {
        #region ctor
        public Contact_Context(int ContactId)
            : base(ContactId)
        {

        }

        public Contact_Context(ContactEntity item)
            : base(item)
        {

        }

        public Contact_Context(DataFilter filter)
            : base()
        {
            Init(filter);
        }


        private Contact_Context()
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


        public static ContactEntity Get(int ContactId)
        {
            using (Contact_Context context = new Contact_Context(ContactId))
            {
                return context.Entity;
            }
        }

        public static ContactEntity Get(string Identifier)
        {

            using (Contact_Context context = new Contact_Context(
                DataFilter.Get("Identifier=@Identifier", Identifier)))
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

    
        public static List<ContactEntity> GetListItems(int accountId)
        {
            using (Contact_Context context = new Contact_Context())
            {
                return context.EntityList(DataFilter.Get("AccountId=@AccountId", accountId));
            }
        }

        public string ToHtml(bool encrypt)
        {
            string h = this.EntityProperties.ToHtmlTable("", "");
            if (encrypt)
                return Encryption.ToBase64String(h, true);
            return h;
        }

        #endregion
    }


    /// <summary>
    /// Summary description for ActiveContact.
    /// </summary>
    public class ContactEntity : IEntityItem
    {
        public const string DefaultSegments = "00000000000000000000";

        #region Ex Properties

        public bool IsEmpty
        {
            get { return Types.IsEmpty(ContactId); }
        }

        #endregion

        #region Properties


        [EntityProperty(EntityPropertyType.Identity, Caption = "���� ������")]
        public int ContactId
        {
            get;
            set;
        }

        string _CellNumber;

        [EntityProperty(EntityPropertyType.Default, false, Caption = "����� ����")]
        public string CellNumber
        {
            get { return _CellNumber; }

            set
            {
                if (ContactRule == 2 && value == "*")
                    _CellNumber = value;
                else
                    _CellNumber = DalUtil.GetValidValue(value, "*", DalUtil.MobilePattern);
            }
        }
        //[EntityProperty(EntityPropertyType.Default, 50, false)]
        //public string ContactName
        //{
        //    get;
        //    set;
        //}

        string _Email;
        [EntityProperty(EntityPropertyType.Default, Caption = "���� ��������")]
        public string Email
        {
            get { return _Email; }
            set
            {
                if (ContactRule == 1 && value == "*")
                    _Email = value;
                else
                    _Email = DalUtil.GetValidValue(value, "*", Nistec.Regx.RegexPattern.Email);
            }
        }

        string _BirthDate;
        [EntityProperty(EntityPropertyType.Default, 10, Caption = "����� ����")]
        public string BirthDate
        {
            get
            {
                return _BirthDate;
            }
            set
            {
                _BirthDate = DalUtil.GetValidStringDate(value, "");
            }
        }

        [EntityProperty(EntityPropertyType.Default, 50, false, Caption = "�� ����")]
        public string FirstName
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, 50, Caption = "�� �����")]
        public string LastName
        {
            get;
            set;
        }
        string _PhoneNumber;
        [EntityProperty(EntityPropertyType.Default, Caption = "����� 1")]
        public string Phone1
        {
            get { return _PhoneNumber; }
            set { _PhoneNumber = DalUtil.GetValidValue(value, "", DalUtil.PhonePattern); }
        }
        [EntityProperty(EntityPropertyType.Default, 50, Caption = "�����")]
        public string Address
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, 50, Caption = "���")]
        public string City
        {
            get;
            set;
        }
        string _Sex;
        [EntityProperty(EntityPropertyType.Default, 1, Caption = "����")]
        public string Sex
        {
            get { return _Sex; }
            set { _Sex = DalUtil.GetValidValue(value, "U"); }
        }
        [EntityProperty(EntityPropertyType.Default, 1, Caption = "���� ������")]
        public DateTime CreationDate
        {
            get;
            set;
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "����� �����")]
        public DateTime LastUpdate
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����� �����")]
        public DateTime? RegisterDate
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "���� �����")]
        public string Registration
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
        [EntityProperty(EntityPropertyType.Default, Caption = "�����")]
        public int Country
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��� �����")]
        public int Sign
        {
            get;
            set;
        }
        /// <summary>
        /// 0=blocked 1= allow all 2 = mail only
        /// </summary>
        [EntityProperty(EntityPropertyType.Default, Caption = "��� ����")]
        public bool IsActive
        {
            get;
            set;
        }

        [EntityProperty(EntityPropertyType.Default, 50, Caption = "����")]
        public string Company
        {
            get;
            set;
        }
        //[EntityProperty(EntityPropertyType.Default, 50)]
        //public string Branch
        //{
        //    get;
        //    set;
        //}
        //string _WeddingDate;
        //[EntityProperty(EntityPropertyType.Default)]
        //public string WeddingDate
        //{
        //    get
        //    {
        //        return _WeddingDate;
        //        //return Types.FormatDate(GetValue<string>("WeddingDate"), "dd/MM/yyyy", ""); 
        //    }
        //    set
        //    {
        //        _WeddingDate = DalUtil.GetValidStringDate(value, "");
        //    }
        //}

        [EntityProperty(EntityPropertyType.Default, Caption = "��� ������")]
        public int ContactRule
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, 50, Caption = "�����")]
        public string Details
        {
            get;
            set;
        }

        [EntityProperty(EntityPropertyType.Default, 20)]
        public string Segments
        {
            get;
            set;
        }
        //string _OtherDate;
        //[EntityProperty(EntityPropertyType.Default)]
        //public string OtherDate
        //{
        //    get
        //    {
        //        return _OtherDate;
        //        //return Types.FormatDate(GetValue<string>("PartnerBirthDate"), "dd/MM/yyyy", ""); 
        //    }
        //    set
        //    {
        //        _OtherDate = DalUtil.GetValidStringDate(value, "");
        //    }
        //}
        [EntityProperty(EntityPropertyType.Default, Caption = "���� �������")]
        public EnableNewsState EnableNews
        {
            get;
            set;
        }


        #endregion

        #region Ex
        [EntityProperty(EntityPropertyType.Default, 10, Caption = "����� 1")]
        public string ExDate1 { get; set; }
        [EntityProperty(EntityPropertyType.Default, 10, Caption = "����� 2")]
        public string ExDate2 { get; set; }
        [EntityProperty(EntityPropertyType.Default, 10, Caption = "����� 3")]
        public string ExDate3 { get; set; }
        [EntityProperty(EntityPropertyType.Default, 10, Caption = "����� 4")]
        public string ExDate4 { get; set; }
        [EntityProperty(EntityPropertyType.Default, 10, Caption = "����� 5")]
        public string ExDate5 { get; set; }
        [EntityProperty(EntityPropertyType.Default, 10, Caption = "����� 1")]
        public string ExText1 { get; set; }
        [EntityProperty(EntityPropertyType.Default, 10, Caption = "����� 2")]
        public string ExText2 { get; set; }
        [EntityProperty(EntityPropertyType.Default, 10, Caption = "����� 3")]
        public string ExText3 { get; set; }
        [EntityProperty(EntityPropertyType.Default, 10, Caption = "����� 4")]
        public string ExText4 { get; set; }
        [EntityProperty(EntityPropertyType.Default, 10, Caption = "����� 5")]
        public string ExText5 { get; set; }
        [EntityProperty(EntityPropertyType.Default, 10, Caption = "���� ������")]
        public string ExKey { get; set; }
        public int ExType { get; set; }
        [EntityProperty(EntityPropertyType.Default, 10, Caption = "���")]
        public string ExLang { get; set; }
        public string Identifier { get; set; }
        #endregion

        #region methods

        public string Print()
        {
            return string.Format("Contact Item :{0}, FirstName:{1}, CellNumber:{2}, Email:{3}", ContactId, FirstName, CellNumber, Email);
        }

        #endregion

    }
}
