using Nistec;
using Nistec.Data;
using Nistec.Data.Entities;
using ProNetcell.Data.Entities.Props;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ProNetcell.Data.Entities
{

    public class ContactCategoriesView : CategoryView
    {

        public static IEnumerable<ContactCategoriesView> View(int ContactRecord, int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.ExecuteList<ContactCategoriesView>("sp_Contact_Categories", "Op", 3, "AccountId", AccountId, "ContactRecord", ContactRecord);
        }

        public static int AddCategory(int ContactRecord, string PropTypes, int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.ExecuteNonQuery("sp_Contact_Categories", "Op", 0, "AccountId", AccountId, "ContactRecord", ContactRecord, "PropTypes", PropTypes);
        }

        public static int DeleteCategory(int ContactRecord, int PropId, int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.ExecuteNonQuery("sp_Contact_Categories", "Op", 2, "AccountId", AccountId, "ContactRecord", ContactRecord, "PropTypes", PropId.ToString());

            //return db.ExecuteCommand("delete from Crm_Accounts_News where ContactId=@ContactId and PropId=@PropId", "ContactId", ContactId, "PropId", PropId);
        }

        


        [EntityProperty]
        public int ContactRecord { get; set; }

    }
}
