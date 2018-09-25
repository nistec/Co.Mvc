using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Web.Controls;
using Pro;
using Pro.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSystem.Data
{
    public class DbSystemContext<T> : EntityContext<DbSystem, T> where T : IEntityItem
    {
        public static void Refresh(string entityCacheGroups, int userId)
        {
            DbContextCache.Remove<T>(Settings.ProjectName, entityCacheGroups, 0, userId);
        }
        public static DbSystemContext<T> Get(string entityCacheGroups, int accountId, int userId)
        {
            return new DbSystemContext<T>(entityCacheGroups, accountId, userId);
        }
        public DbSystemContext(string entityCacheGroups, int accountId, int userId)
        {
            CacheKey = DbContextCache.GetKey<T>(Settings.ProjectName, entityCacheGroups, accountId, userId);
        }
        //public override IList<T> ExecOrViewList(params object[] keyValueParameters)
        //{
        //    //int ttl = 3;
        //    return DbContextCache.EntityList<DbSystem, T>(CacheKey, keyValueParameters);
        //}
        protected override void OnChanged(ProcedureType commandType)
        {
            DbContextCache.Remove(CacheKey);
        }

        public FormResult GetFormResult(EntityCommandResult res, string reason)
        {
            return FormResult.Get(res, this.EntityName, reason);
        }

    }
}
