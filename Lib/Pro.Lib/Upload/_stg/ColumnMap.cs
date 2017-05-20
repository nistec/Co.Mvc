using Pro.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Nistec.Data;
using System.Text.RegularExpressions;
using Nistec.Data.Entities;

namespace Pro.Lib.Upload
{
    /*
         [AccountId]
        ,[Semantic]
        ,[FieldName]
        ,[DisplayName]
        ,[EntityType]
        ,[SrcFormat]
        ,[Condition]
        ,[Enable]
        ,[IsOrig]
            */
    public class SemanticField : IEntityItem
    {
        public int AccountId { get; set; }
        public string Semantic { get; set; }
        public string FieldName { get; set; }
        public string DisplayName { get; set; }
        public string EntityType { get; set; }
        public string SrcFormat { get; set; }
        public bool FilterType { get; set; }
        public string Operator { get; set; }
        public string Condition { get; set; }
        public bool Enable { get; set; }

        public ColumnStg ToColumnStg(string name, int ordinal)
        {

            return new ColumnStg()
            {

                ColumnOrdinal = ordinal,
                CustomColumnName = name,
                FieldName = FieldName,
                DisplayName = DisplayName,
                FilterType = FilterType,
                Operator = Operator,
                Condition = Condition,
                SrcFormat = SrcFormat,
                Enable = Enable

            };
        }
    }

    public class ColumnMap
    {

        Dictionary<string, string> Map;
        Dictionary<string, SemanticField> SemanticFields;
        Dictionary<string, string> MapFields;
        Dictionary<string, string> MatchCols;
        int AccountId;

        public ColumnMap(int accountId)
        {
            AccountId = accountId;
            Map = new Dictionary<string, string>();
            SemanticFields = new Dictionary<string, SemanticField>();
            MapFields = new Dictionary<string, string>();
            MatchCols = new Dictionary<string, string>();

            CreateMapFieldsSemantic(accountId);
        }
               
        public ColumnStg SemanticToDataColumn(string name, int ordinal)
        {
            ColumnStg cs = null;
            string dbcol;
            string colname = NormelizeTextKey(name);
            if (Map.TryGetValue(colname, out dbcol))
            {
                if (!MatchCols.ContainsKey(dbcol))
                {
                    SemanticField sf;
                    if (SemanticFields.TryGetValue(dbcol, out sf))
                    {
                        cs = sf.ToColumnStg(name, ordinal);
                    }
                    else
                    {
                        cs = new ColumnStg()
                        {
                             ColumnOrdinal=ordinal,
                             CustomColumnName=name
                        };
                    }
                     MatchCols[dbcol] = colname;
                }
            }
            else
            {
                cs = new ColumnStg()
                {
                    ColumnOrdinal = ordinal,
                    CustomColumnName = name
                };
            }
            return cs;
        }

        void CreateMapFieldsSemantic(int accountId)
        {
            /*
           [AccountId]
          ,[Semantic]
          ,[FieldName]
          ,[DisplayName]
          ,[EntityType]
          ,[SrcFormat]
          ,[Condition]
          ,[Enable]
          ,[IsOrig]
              */
            using (var db = DbContext.Create<DbPro>())
            {
                IList<SemanticField> list = db.ExecuteList<SemanticField>("sp_Fields_Map", "EntityType", "members", "AccountId", accountId);
                foreach (var cs in list)
                {
                    if (cs.Semantic == null || cs.Semantic == "" || cs.FieldName == null || cs.FieldName == "" || cs.FieldName.ToUpper() == "NA")
                        continue;
                    string k = NormelizeTextKey(cs.Semantic);
                    Map[k] = k;
                    //SemanticFields[cs.FieldName] = cs;
                    if (cs.AccountId != 0)
                    {
                        SemanticFields[k] = cs;
                    }
                    else if (SemanticFields.ContainsKey(k) == false)
                    {
                        SemanticFields[k] = cs;
                    }
                }
            }
        }

        public ColumnStg MatchToDataColumn(string name, int ordinal)
        {
            ColumnStg cs = null;
            string dbcol;
            string colname = NormelizeTextKey(name);
            if (Map.TryGetValue(colname, out dbcol))
            {
                if (!MatchCols.ContainsKey(dbcol))
                {
                    cs = new ColumnStg()
                    {

                        ColumnOrdinal = ordinal,
                        CustomColumnName = name,
                        FieldName = dbcol,
                        DisplayName = MapFields[dbcol],

                    };

                    //dbColumn = dbcol;
                    //fieldName = MapFields[dbColumn];
                    MatchCols[dbcol] = colname;
                }
            }
            return cs;
        }

        void CreateMapFields(int accountId)
        {

            using (var db = DbContext.Create<DbPro>())
            {
                var dt = db.QueryDataTable("Members_Map", "AccountId", accountId);

                foreach (DataRow dr in dt.Rows)
                {
                    string keys = dr.Get<string>("Field");
                    string fieldName = dr.Get<string>("FieldName");
                    string val = dr.Get<string>("FieldMap");
                    if (keys == null || keys == "" || val == null || val == "" || val.ToUpper() == "NA")
                        continue;
                    string k = NormelizeTextKey(val);
                    Map[k] = val;
                    MapFields[val] = fieldName;

                    string[] args = keys.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string key in args)
                    {
                        //string k = Regex.Replace(key, "[^a-zA-Z0-9 -]", "");
                        k = NormelizeTextKey(key);
                        Map[k] = val;
                    }
                }
            }
        }

        public static string NormelizeTextKey(string dirtyString)
        {
            return new string(dirtyString.Where(Char.IsLetterOrDigit).ToArray()).ToLower();
        }
    
    }
}
