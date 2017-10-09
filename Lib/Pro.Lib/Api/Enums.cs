using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pro.Lib.Api
{
    public enum MemberUpdateType
    {
        InsertOnly = 0,
        Sync = 1,
        UpdateFull = 2
    }
    public enum DataSourceTypes
    {
        CoSystem = 0, Register = 1, FileSync = 2, ApiSync = 3
    }
    public enum EnableNewsState
    {
        NotSet = -1, Disable = 0, Enable = 1
    }
    public enum PlatformType
    {
        NA = 0, Cell = 1, Mail = 2
    }
    public enum MemberKeyType
    {
        None=-1, MemberId = 0, Cell = 1, Mail = 2, Target =3 ,Key = 4
    }
    
}