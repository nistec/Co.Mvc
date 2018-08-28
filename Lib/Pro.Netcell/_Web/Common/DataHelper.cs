using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Nistec;

namespace Netcell.Web
{

    //public static class DataRowExtended
    //{
    //    public static T Get<T>(this DataRow row, string field)
    //    {
    //        return GenericTypes.Convert<T>(row[field]);
    //    }

    //    public static T Get<T>(this DataRow row, string field, T valueIfNull)
    //    {
    //        return GenericTypes.Convert<T>(row[field], valueIfNull);
    //    }
    //}

    public class DataHelper
    {

        public static DataView DataTop(DataView dv, int top) 
        {
            if (dv.Count <= top || top <=0)
                return dv;
 
            DataTable dt = dv.Table;
            DataTable cloneDataTable = dt.Clone();
            for (int i = 0; i < top; i++) 
            { 
                cloneDataTable.ImportRow(dt.Rows[i]); 
            } 
            return new DataView(cloneDataTable); 
        } 

    }
}
