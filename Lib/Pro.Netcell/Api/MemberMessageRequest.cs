using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Nistec.Data;
using ProNetcell.Data.Entities;
using Nistec.Data.Entities;



namespace Pro.Netcell.Api
{

    public class MemberMessageRequest : RequestContract<MemberMessageContract>
    {
        public override MemberMessageContract Body { get; set; }
        public override string ToString()
        {
            return string.Format("ContactAdd - AccountId:{0},CellNumber:{1},Email:{2},BirthDate:{3},FirstName:{4},ExKey:{5},GroupName:{6}", Auth.AccountId, Body.CellPhone, Body.Email, Body.Birthday, Body.FirstName, Body.ExId, Body.Category);
        }
        public override void ValidateMessage(string clientIp)
        {
            base.ValidateMessage(clientIp);

            if (Body == null)
            {
                throw new ArgumentException("Message body parsing error");
            }

            Body.ValidateMessage();
        }
    }

    public class MemberMessageContract : IEntityItem//: MemberItem
    {
        //public string Category { get; set; }
        public int UpdateType { get; set; }
        public string City { get; set; }
        //[EntityProperty(EntityPropertyType.Optional)]
        public string Category { get; set; }
        public string Branch { get; set; }

        //===================================

        [EntityProperty(EntityPropertyType.Key)]
        public string Identifier { get; set; }

        [Validator(Required = true, Name = "תעודת זהות")]
        public string MemberId { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        [Validator(Required = true, Name = "חשבון")]
        public int AccountId { get; set; }
        [Validator(Required = true, Name = "שם פרטי")]
        public string LastName { get; set; }
        [Validator(Required = true)]
        public string FirstName { get; set; }
        public string ExId { get; set; }
        public string Address { get; set; }
        //public int City { get; set; }
        //public int Branch { get; set; }
        public string CellPhone { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string Note { get; set; }

        [Validator(Required = true, MinValue = "2000-01-01", Name = "תאריך הצטרפות")]
        public DateTime JoiningDate { get; set; }
        public string ZipCode { get; set; }

        [EntityProperty(EntityPropertyType.View)]
        public DateTime LastUpdate { get; set; }

        [EntityProperty(EntityPropertyType.Identity)]
        public int RecordId { get; set; }

        public string ExField1 { get; set; }
        public string ExField2 { get; set; }
        public string ExField3 { get; set; }

        public int ExEnum1 { get; set; }
        public int ExEnum2 { get; set; }
        public int ExEnum3 { get; set; }
        public bool? EnableNews { get; set; }

        public string DataSource { get; set; }
        public int DataSourceType { get; set; }
        public int MemberType { get; set; }
        public string CompanyName { get; set; }
        public int ExRef1 { get; set; }
        public int ExRef2 { get; set; }
        public int ExRef3 { get; set; }

        internal void ValidateMessage()
        {

        }
    }

    //public class MemberMessageContract
    //{
    //    public string Category { get; set; }
    //    public int UpdateType { get; set; }

    //    #region member
    //    public int CustomId { get; set; }
    //    public string MemberId { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string Address { get; set; }
    //    public string City { get; set; }
    //    public string Birthday { get; set; }
    //    public string Gender { get; set; }
    //    public string CellPhone { get; set; }
    //    public string Phone { get; set; }
    //    public string Email { get; set; }
    //    //public string JoiningDate { get; set; }
    //    public string Branch { get; set; }
    //    public string Note { get; set; }
    //    //public string LastUpdate { get; set; }
    //    public string ZipCode { get; set; }
       
    //    //public int ContactRule { get; set; }
    //    //public int RecordId { get; set; }
    //    //public bool IsDeleted { get; set; }
    //    //public bool IsSignup { get; set; }
    //    public string ExField1 { get; set; }
    //    public string ExField2 { get; set; }
    //    public string ExField3 { get; set; }
    //    public int ExEnum1 { get; set; }
    //    public int ExEnum2 { get; set; }
    //    public int ExEnum3 { get; set; }
    //    public string ExId { get; set; }
    //    //public int ExType { get; set; }
    //    //public string Identifier { get; set; }
    //    public bool EnableNews { get; set; }

    //    #endregion
    //    internal void ValidateMessage()
    //    {

    //    }
    //}
}