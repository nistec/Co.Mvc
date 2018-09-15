using Nistec.Data;
using Nistec.Data.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pro.Data
{
    public class DalTrace : Nistec.Data.SqlClient.DbCommand
    {

        #region ctor

        public DalTrace()
            : base(DbConfig.CnnTrace)
        {

        }

        #endregion

        [DBCommand("SELECT RowId,ActTime,Method,Status FROM Trace_Act")]
        public IEnumerable<T> Trace_Act<T>()
        {
            return base.Execute<T, List<T>>();
        }

        [DBCommand("SELECT RowId,ActTime,Method,Status FROM Trace_Act where RowId=@RowId")]
        public T GetTrace_Act<T>([DbField()]long RowId)
        {
            return base.Execute<T>();
        }
        //[DBCommand("Update Trace_Act ")]
        //public T UpdateTrace_Act<T>([DbField()]long RowId)
        //{
        //    return base.Execute<T>();
        //}
    }

}
