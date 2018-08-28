using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Data;
using System.Text.RegularExpressions;

namespace Netcell.Data.Db.Entities
{

    [Entity("Billing_Package", "vw_Accounts_Billing_Package", "cnn_Netcell", EntityMode.Generic, "AccountId", EntitySourceType = EntitySourceType.View)]
    public class Billing_Package_View : EntityContext<Account_Billing_Package>
    {

        public Billing_Package_View(int AccountId)
            : base(AccountId)
        {
           
        }

        //[EntityProperty(EntityPropertyType.Key, Caption = "חשבון")]
        //public int AccountId
        //{
        //    get { return base.GetValue<int>(); }
        //}



    }


    public class Account_Billing_Package : Billing_Package
    {
        [EntityProperty(EntityPropertyType.Key, Caption = "חשבון")]
        public int AccountId { get; set; }
    }



    [Entity("Billing_Package", "Billing_Package", "cnn_Netcell", EntityMode.Generic, "PackageId", EntitySourceType = EntitySourceType.Table)]
    public class Billing_Package_Context : EntityContext<Billing_Package>
    {
        public Billing_Package_Context(int PackageId)
            : base(PackageId)
        {

        }
    }

    public class Billing_Package : IEntityItem
    {

        #region Properties


        [EntityProperty(EntityPropertyType.Identity, Caption = "מס-חבילה")]
        public int PackageId{get;set;}

        [EntityProperty(EntityPropertyType.Default, Caption = "שם החבילה")]
        public string PackageName{get;set;}

        [EntityProperty(EntityPropertyType.Default, Caption = "מחיר")]
        public decimal FixedPrice{get;set;}

        [EntityProperty(EntityPropertyType.Default, Caption = "תאור")]
        public int Details{get;set;}

        [EntityProperty(EntityPropertyType.Default, Caption = "תקופה")]
        public int Period{get;set;}
        
        [EntityProperty(EntityPropertyType.Default, Caption = "סטטוס")]
        public int State{get;set;}
        
        [EntityProperty(EntityPropertyType.Default, Caption = "אנשי קשר")]
        public int MaxContacts{get;set;}
        
        [EntityProperty(EntityPropertyType.Default, Caption = "רשימות תפוצה")]
        public int MaxMailingList{get;set;}
        
        [EntityProperty(EntityPropertyType.Default, Caption = "אתרים")]
        public int MaxSites{get;set;}
        
        [EntityProperty(EntityPropertyType.Default, Caption = "אתרים סלולארים")]
        public int MaxMobileSites{get;set;}
        
        [EntityProperty(EntityPropertyType.Default, Caption = "מערכות רישום")]
        public int MaxRegistryForms{get;set;}
        
        [EntityProperty(EntityPropertyType.Default, Caption = "דיוורים שמורים")]
        public int MaxCampaignsTemplates{get;set;}
        
        [EntityProperty(EntityPropertyType.Default, Caption = "תבניות עיצוב סלולאר")]
        public int MaxMobileDesign{get;set;}
        
        [EntityProperty(EntityPropertyType.Default, Caption = "תבניות עיצוב מייל")]
        public int MaxMailDesigns{get;set;}
        
        [EntityProperty(EntityPropertyType.Default, Caption = "מסרונים שמורים")]
        public int MaxSmsTemplates{get;set;}
        
        [EntityProperty(EntityPropertyType.Default, Caption = "תאריכונים")]
        public int MaxCampaignsWatch{get;set;}
        
        [EntityProperty(EntityPropertyType.Default, Caption = "תזמונים")]
        public int MaxScheduleItems{get;set;}
        
        [EntityProperty(EntityPropertyType.Default, Caption = "קטלוגים")]
        public int MaxCatalogs{get;set;}
        

        #endregion
    }

    /*
    public class Billing_Package_Base : ActiveEntity
    {

       
        #region Ctor

        public Billing_Package_Base()
            : base()
        {

        }
        public Billing_Package_Base(int Id)
            : base(Id)
        {

        }

        #endregion

        #region Properties


        [EntityProperty(EntityPropertyType.Identity, Caption = "מס-חבילה")]
        public int PackageId
        {
            get { return base.GetValue<int>(); }
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "שם החבילה")]
        public string PackageName
        {
            get { return base.GetValue<string>(); }
            set { SetValue(value); }
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "מחיר")]
        public decimal FixedPrice
        {
            get { return base.GetValue<decimal>(); }
            set { SetValue(value); }
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "תאור")]
        public int Details
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "תקופה")]
        public int Period
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "סטטוס")]
        public int State
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }

       
        [EntityProperty(EntityPropertyType.Default, Caption = "אנשי קשר")]
        public int MaxContacts
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "רשימות תפוצה")]
        public int MaxMailingList
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "אתרים")]
        public int MaxSites
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "אתרים סלולארים")]
        public int MaxMobileSites
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מערכות רישום")]
        public int MaxRegistryForms
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "דיוורים שמורים")]
        public int MaxCampaignsTemplates
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "תבניות עיצוב סלולאר")]
        public int MaxMobileDesign
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
         [EntityProperty(EntityPropertyType.Default, Caption = "תבניות עיצוב מייל")]
        public int MaxMailDesigns
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
       [EntityProperty(EntityPropertyType.Default, Caption = "מסרונים שמורים")]
        public int MaxSmsTemplates
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
       [EntityProperty(EntityPropertyType.Default, Caption = "תאריכונים")]
        public int MaxCampaignsWatch
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "תזמונים")]
        public int MaxScheduleItems
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "קטלוגים")]
        public int MaxCatalogs
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
        
        #endregion

    }
    */
}
