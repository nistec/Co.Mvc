using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using Nistec.Data.Entities;
using Nistec.Serialization;

namespace Netcell
{
    [Serializable]
    public class TargetEntity: IEntityItem
    {

        public string To { get; set; }
        public string Args { get; set; }

        public TargetEntity()
        {

        }

        public TargetEntity(string[] args)
        {
            To = args[0];
            Args = (args.Length > 1) ? args[1] : "";
        }

        public TargetEntity(string to, string args)
        {
            To = to;
            Args = args;
        }

        public string Serialize()
        {
            return NetSerializer.SerializeToJson(this);
        }

        public static TargetEntity Deserialize(string target)
        {
            return NetSerializer.DeserializeFromJson<TargetEntity>(target);
        }

        public static string SerializeList(List<TargetEntity> list)
        {
            return NetSerializer.SerializeToJsonArray<TargetEntity>(list);
        }

        public static List<TargetEntity> DeserializeList(string targets)
        {
            return NetSerializer.DeserializeFromJsonArray<TargetEntity>(targets);
        }

        public static TargetEntity ParseTarget(string target)
        {
            try
            {
                target = target.Replace("{", "").Replace("}", "");

                string[] items = target.Split(',');
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (string s in items)
                {
                    string[] arg = target.Split(':');
                    dic[arg[0]] = arg[1];
                }
                TargetEntity entity = new TargetEntity()
                {
                    To = dic["To"],
                    Args = dic["Args"]
                };
                return entity;
            }
            catch (Exception ex)
            {
                throw new TypeLoadException(ex.Message);
            }
        }

        public static List<TargetEntity> FormatTargets(string targets)
        {
            if (targets == null)
            {
                throw new ArgumentNullException("ReminderEntity.CreateTargets");
            }
            string[] args = targets.Split(';');
            return FormatTargets(args);
       }

        public static List<TargetEntity> FormatTargets(string[] targets)
        {
            if (targets == null)
            {
                throw new ArgumentNullException("ReminderEntity.CreateTargets");
            }
            if (targets.Length > 2000)
            {
                throw new ArgumentException("ReminderEntity.CreateTargets: targets max send exceeded");
            }
            List<TargetEntity> list = new List<TargetEntity>();

            foreach (string s in targets)
            {
                list.Add(FormatTarget(s));
            }

            return list;
        }

        public static TargetEntity FormatTarget(string target)
        {
            string[] args = target.Split(':');
            return new TargetEntity(args);
        }

       

        public static DataTable TargetEntitySchema()
        {
            DataTable dt = new DataTable("Target");
            //dt.Columns.Add("ReminderId", typeof(Int32));
            dt.Columns.Add("To");
            dt.Columns.Add("Args");
            return dt.Clone();
        }

        public static DataTable TargetsToDataTable(TargetEntity[] targets)
        {
            DataTable dt = TargetEntitySchema();
            foreach (TargetEntity t in targets)
            {
                dt.Rows.Add(t.To, t.Args);
            }
            return dt;
        }

    }
 
}
