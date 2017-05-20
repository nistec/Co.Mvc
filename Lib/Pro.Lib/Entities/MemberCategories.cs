using Nistec;
using Nistec.Data;
using Nistec.Data.Entities;
using Pro.Data.Entities.Props;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Pro.Data.Entities
{

    public class MemberCategoriesView : CategoryView
    {

        public static IEnumerable<MemberCategoriesView> View(int MemberRecord, int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.ExecuteList<MemberCategoriesView>("sp_Member_Categories", "Op", 3, "AccountId", AccountId, "MemberRecord", MemberRecord);
        }

        public static int AddCategory(int MemberRecord, string PropTypes, int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.ExecuteNonQuery("sp_Member_Categories", "Op", 0, "AccountId", AccountId, "MemberRecord", MemberRecord, "PropTypes", PropTypes);
        }

        public static int DeleteCategory(int MemberRecord, int PropId, int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.ExecuteNonQuery("sp_Member_Categories", "Op", 2, "AccountId", AccountId, "MemberRecord", MemberRecord, "PropTypes", PropId.ToString());

            //return db.ExecuteCommand("delete from Crm_Accounts_News where MemberId=@MemberId and PropId=@PropId", "MemberId", MemberId, "PropId", PropId);
        }

        


        [EntityProperty]
        public int MemberRecord { get; set; }

    }
}
