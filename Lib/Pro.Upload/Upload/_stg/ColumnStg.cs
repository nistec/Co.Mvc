using Nistec.Data.Entities;
using Nistec.Data;
using Pro.Lib.Upload.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Nistec;
using Nistec.Generic;

namespace Pro.Lib.Upload
{
    public class ColumnStg : IEntityItem
    {
        //public int AccountId { get; set; }
        public string CustomColumnName { get; set; }
        public int ColumnOrdinal { get; set; }
        public string FieldName { get; set; }
        public string DisplayName { get; set; }
        public string SrcFormat { get; set; }
        public bool FilterType { get; set; }
        public string Operator { get; set; }
        public string Condition { get; set; }
        public bool Enable { get; set; }

      
    }

    public class ColumnStgMap
    {
        public int Count { get; private set; }
        public string Message { get; private set; }
        public string UploadKey { get; private set; }
        public List<ColumnStg> Columns { get; private set; }
        public int AccountId { get; private set; }

        public ColumnStgMap(int accountId, string error)
        {
            AccountId = accountId;
            Message = error;
            Count = 0;
        }
        public ColumnStgMap(int accountId, DataColumnCollection columns, int count)
        {
            Count = count;
            UploadKey = accountId.ToString() + "_" + UUID.NewId();
            Message = "ok";
            AccountId = accountId;
            Columns = new List<ColumnStg>();
            ColumnMap map = new ColumnMap(AccountId);
            int ordinal = 0;
            foreach (DataColumn col in columns)
            {
                //ColumnStg cs = map.MatchToDataColumn(col.ColumnName, ordinal);
                ColumnStg cs = map.SemanticToDataColumn(col.ColumnName, ordinal);
                ordinal++;
                Columns.Add(cs);
            }

            /*
            var columnsstg = [
               { value: "FirstName", label: "שם פרטי" },
               { value: "LastName", label: "שם משפחה" }
            ];
            */


        }
    }
}
