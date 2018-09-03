using Nistec.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pro.Lib.Upload.Members
{
    [EntityMapping("Members")]
    public class MemberItem : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Key)]
        public string Identifier { get; set; }

        //[Validator(RequiredVar="@ExType=0", Name="תעודת זהות")]
        public string MemberId { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        [Validator(Required = true, Name = "חשבון")]
        public int AccountId { get; set; }
        [Validator(Required = true, Name = "שם פרטי")]
        public string LastName { get; set; }
        [Validator(Required = true)]
        public string FirstName { get; set; }
        //[Validator(RequiredVar = "@ExType=3")]
        public string ExId { get; set; }
        public string Address { get; set; }
        public int City { get; set; }
        public int Branch { get; set; }
        //[Validator(RequiredVar = "@ExType=1", Name = "טלפון נייד")]
        public string CellPhone { get; set; }
        public string Phone { get; set; }
        //[Validator(RequiredVar = "@ExType=2", Name = "דואל")]
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

        [EntityProperty(EntityPropertyType.Optional)]
        public string Categories { get; set; }

        public string DisplayName
        {
            get
            {
                if (MemberType == 1)
                    return CompanyName;
                return FirstName + " " + LastName;
            }
        }

        public string MemberName { get { return FirstName + " " + LastName; } }
        public string ToHtml()
        {
            return EntityProperties.ToHtmlTable<MemberItem>(this, null, null, true);
        }
        public Dictionary<string, object> ToDictionary()
        {
            return EntityDataExtension.EntityToDictionary(this, true, false); //..<MemberItem>(this, null, null, true);
        }

    }
}
