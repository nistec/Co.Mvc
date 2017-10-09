using Nistec;
using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Web.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSystem.Data.Entities
{

    public class CalendarContext : EntityContext<DbSystem, CalendarItem> //where T : IEntityItem
    {

        public static void Refresh(int AccountId)
        {
            DbContextCache.Remove<CalendarItem>(Settings.ProjectName, EntityCacheGroups.Task, AccountId, 0);
        }
        public static CalendarContext Get(int userId)
        {
            return new CalendarContext(userId);
        }
        public CalendarContext(int userId)
        {
            if (userId > 0)
                CacheKey = DbContextCache.GetKey<CalendarItem>(Settings.ProjectName, EntityCacheGroups.Task, 0, userId);
        }
        //var list = calendar.GetList("TimeFrom", dateFrom, "TimeTo", dateTo);

        public IEnumerable<CalendarItem> GetListItems(int accountId, int userId, DateTime dateFrom, DateTime dateTo)
        {

           var list= DbContextCache.ExecuteList<DbSystem, CalendarItem>(CacheKey, new object[]{"AccountId", accountId, "UserId", userId, "TimeFrom", new DateTime(2015,1,1), "TimeTo", new DateTime(2029,12,31)});

           var filterlist = list.Where(v => v.TimeFrom >= dateFrom && v.TimeTo <= dateTo).ToArray();

           return filterlist;

            //using (var db = DbContext.Create<DbSystem>())
            //{
            //    return db.ExecuteList<CalendarItem>("sp_Calendar","AccountId", accountId, "UserId", userId, "TimeFrom", dateFrom, "TimeTo", dateTo);
            //}


            //using(var db= DbContext.Create<DbSystem>())
            //{
            //  return  db.Query<CalendarItem>("select * from [vw_Calendar] where AccountId=@AccountId and UserId=@UserId and TimeFrom >=@TimeFrom and TimeTo<=@TimeTo", "AccountId", accountId, "UserId", userId, "TimeFrom", dateFrom, "TimeTo", dateTo);
            //}
            //return DbContextCache.EntityList<DbSystem, CalendarItem>(CacheKey, null);
        }

        public IList<CalendarItem> GetList()
        {
            return DbContextCache.EntityList<DbSystem, CalendarItem>(CacheKey, null);
        }
        public IList<CalendarItem> GetList(int CalendarId)
        {
            return DbContextCache.EntityList<DbSystem, CalendarItem>(CacheKey, new object[] { "CalendarId", CalendarId });
        }
        protected override void OnChanged(UpdateCommandType commandType)
        {
            DbContextCache.Remove(CacheKey);
        }
        public FormResult GetFormResult(EntityCommandResult res, string reason)
        {
            return FormResult.Get(res, EntityName, reason);
        }
    }


    [EntityMapping("Calendar", "vw_Calendar", "יומן", ProcListView="sp_Calendar")]
    public class CalendarItem : IEntityItem
    {
        //public int GetId()
        //{
        //    //return Types.ToInt(CalendarKey.Replace("id", ""));
        //    //return Types.ToInt(CalendarKey.Split('-')[1]);
        //    return Types.ToInt(CalendarKey.Split('.')[1]);
        //}

        public int GetUserId(int defaultValue)
        {
            //return Types.ToInt(CalendarKey.Replace("id", ""));
            //return Types.ToInt(CalendarKey.Split('-')[1]);
            if (string.IsNullOrEmpty(DisplayName))
                return defaultValue;
            return Types.ToInt(DisplayName.Split(':')[1].Trim());
        }

        [EntityProperty(EntityPropertyType.Identity)]
        public int CalendarId { get; set; }
        [EntityProperty(EntityPropertyType.Optional)]
        public int RowId { get; set; }

        public string Subject { get; set; }
        public string Location { get; set; }

        [EntityProperty(EntityPropertyType.View)]
        public DateTime ModifiedDate { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public bool AllDay { get; set; }
        public string TimeZone { get; set; }
        public string Repeat { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public bool IsHide { get; set; }
        public bool IsReadOnly { get; set; }
        public string ResourceId { get; set; }

        [EntityProperty(EntityPropertyType.Optional)]
        public string DisplayName { get; set; }
    }
   
}
