using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using System.Data.SqlClient;
using Nistec;
using Nistec.Data.SqlClient;
using Nistec.Data;
using Nistec.Data.Factory;

namespace Netcell.Data
{
    public class DalBulkInsert : DbBulkCopy
    {
         public DalBulkInsert()
            : base(Netcell.Data.DBRule.CnnNetcell)
        {
            //uploaded = false;
            // base.AutoCloseConnection = true;
        }

    }
}
