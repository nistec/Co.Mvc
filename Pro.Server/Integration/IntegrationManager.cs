using Nistec;
using Nistec.Data.Entities;
using Nistec.Generic;
using Pro.Data;
using Pro.Lib.Upload;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pro.Server.Integration
{
    public class IntegrationManager
    {
        IntegrationTask Task;
        Dictionary<string, IntegrationFieldsMap> FieldsMap;
        Dictionary<string, int> FileStgColumns;
        DataTable FileStg;
        DataTable UploadStg;
        UploadSumarize Sumarize;
        string UploadKey;
        int ExType;

        public IntegrationManager(string uploadKey)
        {

            UploadKey = uploadKey;


        }

        public void Execute()
        {
            Task = IntegrationTask.Get(UploadKey);
            using (var db = DbContext.Create<DbPro>())
            {
                UploadStg = db.QueryDataTable("select * from Integration_Stg where 1=0");
            }
            LoadFileStg();
            LoadFieldsMap();
            BuildUploadStg();
        }


        void LoadFileStg()
        {
            using (var db = DbContext.Create<DbPro>())
            {
                FileStg = db.QueryDataTable("Integration_File_Stg", "UploadKey", UploadKey);
            }
            FileStgColumns = new Dictionary<string, int>();
            DataRow row = FileStg.Rows[0];

            for (int i = 0; i < FileStg.Columns.Count; i++)
            {
                var entry = FileStg.Columns[i];
                string columnName = Types.NZ(row[entry.ColumnName], (string)null);
                if (columnName != null)
                {
                    FileStgColumns[columnName] = i;
                }
            }
        }


        void LoadFieldsMap()
        {
            using (var db = DbContext.Create<DbPro>())
            {
                var list = db.ExecuteList<IntegrationFieldsMap>("select * from [vw_Integration_Fields_Map] where SourceType=@SourceType", "SourceType", Task.SourceType);

                FieldsMap = new Dictionary<string, IntegrationFieldsMap>();
                foreach (var entry in list)
                {
                    FieldsMap[entry.SourceName] = entry;
                }
            }
        }

         void BuildUploadStg()
         {
             //ResolveFileStg();

             Sumarize = new UploadSumarize();
             int counter = 0;
             int columns = FileStg.Columns.Count;

             //foreach (DataRow dr in FileStg.Rows)
             
             //first row contains header
             for (int i = 1; i < FileStg.Rows.Count;i++ )
             {
                     DataRow dr = FileStg.Rows[i];
                     GenericRecord record = new GenericRecord();
                     foreach (var entry in FileStgColumns)
                     {
                         var val = dr[entry.Value];

                         IntegrationFieldsMap fieldmap;
                         if (FieldsMap.TryGetValue(entry.Key, out fieldmap))
                         {
                             if (fieldmap.ValidateFilter(val))
                                 break;
                             if (fieldmap.IsRuleField)
                                 continue;

                             val = fieldmap.DoSourceFormat(val);
                             record.SetValue(fieldmap.FieldName, val);
                         }

                     }
                     record.SetValue("UploadKey", UploadKey);
                     record.SetValue("AccountId", Task.AccountId);
                     record.SetValue("ExType", ExType);
                     string IdentifierKey = null;

                     switch (ExType)
                     {
                         case 0://MemberId
                             IdentifierKey = Types.NZ(record["MemberId"], (string)null); break;
                         case 1://CellNumber
                             IdentifierKey = Types.NZ(record["CellPhone"], (string)null); break;
                         case 2://Email
                             IdentifierKey = Types.NZ(record["Email"], (string)null); break;
                         case 3://ExId
                             IdentifierKey = Types.NZ(record["ExId"], (string)null); break;
                     }

                     if (string.IsNullOrEmpty(IdentifierKey))
                     {
                         Sumarize.WrongItem++;
                         continue;
                         //throw new Exception("Invalid identifier Type:" + ExType.ToString());
                     }
                     string Identifier = string.Format("{0}_{1}_{2}", Task.AccountId, ExType, IdentifierKey);
                     record.SetValue("Identifier", Identifier);
                     record.SetValue("RecordId", counter);
                     //ContactRule rule = UploadReader.GetContactRule(cellNumber, email);
                     record.AddTo(UploadStg, false);
                     Sumarize.Ok++;
                     counter++;
                 }
         }
    }
}
