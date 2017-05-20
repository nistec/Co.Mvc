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


    public class UploadMembersView : MemberItem
    {

        public static IEnumerable<UploadMembersView> ViewUploaded(int accountId, string uploadKey)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemList<UploadMembersView>("Upload_Members", "AccountId", accountId, "UploadKey", uploadKey);
        }


        //[EntityProperty(EntityPropertyType.Key)]
        //public string MemberId { get; set; }
        //[EntityProperty(EntityPropertyType.Key)]
        //public int AccountId { get; set; }
        //public string LastName { get; set; }
        //public string FirstName { get; set; }
        //public string FatherName { get; set; }
        //public string Address { get; set; }
        //public int City { get; set; }
        //public int PlaceOfBirth { get; set; }
        //public int BirthDateYear { get; set; }
        //public int ChargeType { get; set; }
        //public int Branch { get; set; }
        //public string CellPhone { get; set; }
        //public string Phone { get; set; }
        //public string Email { get; set; }
        //public int Status { get; set; }
        ////public int Category { get; set; }
        //public int Region { get; set; }
        //public string Gender { get; set; }
        //public string Birthday { get; set; }
        //public string Note { get; set; }
        //public string Categories { get; set; }
        //public DateTime JoiningDate { get; set; }
        //[EntityProperty(EntityPropertyType.View)]
        //public DateTime LastUpdate { get; set; }
        //[EntityProperty(EntityPropertyType.Identity)]
        //public int RecordId	{ get; set; }

        public string UploadKey { get; set; }
        public int UploadState { get; set; }
        public int ContactRule { get; set; }
    }
}
