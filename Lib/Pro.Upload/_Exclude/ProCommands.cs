using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Data;
using System.Globalization;
using Nistec.Data.Factory;
using Nistec.Data.Entities.Cache;
using Nistec.Generic;
using System.Data;
using System.Web;

namespace Pro.Data
{

    public class ProCommands
    {
        public static string ViewLog()
        {
            using (var db = DbContext.Create<DbLogs>())
                return db.QueryJson("Select top 1000 * from Sb_Log order by logid desc");
        }
    }

}
