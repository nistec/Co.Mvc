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

    public class NewsContext : DbSystemContext<News>
    {
        public static NewsContext Get(int accountId, int userId)
        {
            return new NewsContext(accountId,userId);
        }
        public NewsContext(int accountId,int userId)
            : base(EntityCacheGroups.Task, accountId,userId)
        {
        }
        public IList<News> GetList(int NewsId)
        {
            return base.ExecOrViewList("NewsId", NewsId );
        }
    }
    
    [EntityMapping("News", "vw_News","הודעות")]
    public class News: IEntityItem
    {
      public int NewsId{get;set;}
      public string NewsSubject{get;set;}
      public string NewsText { get; set; }
      public string NewsType { get; set; }
      public DateTime? DateToDisplay{get;set;}
      public string Link { get; set; }
      public string Source { get; set; }
      public int SourceId{get;set;}
      public int AssignBy{get;set;}
      public int UserId{get;set;}
      public int AccountId{get;set;}
      public bool IsShare{get;set;}

    }
}
