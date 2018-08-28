using System;
using System.Collections.Generic;
using System.Text;
using Nistec.Data.Factory;

namespace Netcell.Data.Common
{

  
    public class QueryUtil
    {
        public enum QuerySection
        {
            Select,
            From,
            Where,
            GroupBy
        }

        public QueryUtil()
        {
            Distinct = false;
        }

        public QueryUtil(bool distinct)
        {
            Distinct = distinct;
        }

        bool Distinct;
        List<string> select = new List<string>();
        List<string> from = new List<string>();
        List<string> group = new List<string>();
        List<string> where = new List<string>();

        public void AddSelect(string s)
        {
            select.Add(string.Format(s));
        }

        public void AddSelectGroup(string s)
        {
            select.Add(string.Format(s));
            group.Add(string.Format(s));
        }
        public void AddFrom(string s)
        {
            from.Add(string.Format(s));
        }
        public void AddWhere(string s)
        {
            where.Add(string.Format(s));
        }
        public void AddWhere(string s, params object[] args)
        {
            where.Add(string.Format(s, args));
        }

        public string BuildQuery()//bool SplitByMonth)
        {

            StringBuilder sb = new StringBuilder();

            //if (SplitByMonth)
            //{
            //    sbSelect.Append("p.AccountId,");
            //    sbSelect.Append("case g.Status when 0 then 'רשום' when 1 then 'חסום' when 2 then 'בנסיון' end as Status,");
            //    sbSelect.Append(DalUtil.GetMonthSplit("g.Registerdate", "1", "m"));
            //    sbGroup.Append("p.AccountId,g.Status");
            //}
            //else
            //{
            //    sbSelect.Append("p.AccountId, Count(*) AS Total,");
            //    sbSelect.Append("case g.Status when 0 then 'רשום' when 1 then 'חסום' when 2 then 'בנסיון' end as Status");
            //    sbGroup.Append("p.AccountId,g.Status");
            //}

            if (select.Count <= 0 || from.Count<=0)
            {

                throw new Exception ("Invalid predicat select or from in query");
            }

            sb.Append("SELECT ");
            if(Distinct)
                sb.Append("DISTINCT ");

            foreach (string s in select)
            {
                sb.Append(s + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.AppendLine(" ");

            sb.Append("FROM ");
            foreach (string s in from)
            {
                sb.Append(s + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.AppendLine(" ");

            if (where.Count > 0)
            {
                sb.Append("WHERE");
                foreach (string s in where)
                {
                    sb.AppendFormat(" ({0}) and", s);
                }
                sb.Remove(sb.Length - 3, 3);
                sb.AppendLine(" ");
            }
            if (group.Count > 0)
            {
                sb.Append("GROUP BY ");
                foreach (string s in group)
                {
                    sb.Append(s + ",");
                }
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();

        }

    }
  
}
