using Nistec.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProAd.Data.Entities
{
    #region Accounts_Label

    [EntityMapping("Accounts_Label", ProcUpsert = "sp_Accounts_Label_AddOrUpdate", ProcDelete = "sp_Accounts_Label_Del")]
    public class Accounts_Label : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Identity)]
        [Validator(Required = true, RequiredOperation = RequiredOperation.Update, Name = "מס")]
        public long LabelId
        {
            get;
            set;
        }

        //[EntityProperty(EntityPropertyType.Key)]
        [Validator(Required = true, Name = "חשבון")]
        public int AccountId
        {
            get;
            set;
        }
        //[EntityProperty(EntityPropertyType.Key)]
        [Validator(Required = true, Name = "שם השדה")]
        public string Label
        {
            get;
            set;
        }

        public string Val
        {
            get;
            set;
        }
        public DateTime Modified
        {
            get;
            set;
        }
    }


    #endregion

}
