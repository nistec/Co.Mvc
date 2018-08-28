using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netcell.Lib
{

    public enum ContactSignType
    {
        Registry = 0,
        Upload = 1,
        Api = 2,
        Web=3
    };

    public enum ContactRule
    {
        None = 0,
        Cell = 1,
        Mail = 2,
        Both = 3
    }
    public enum ContactKeyType
    {
        None = 0,
        Cell = 1,
        Mail = 2,
        Target = 3,
        Key = 4,
        Uuid = 5
    };

    public enum ContactUpdateType
    {
        InsertOnly = 0,
        UpdateFull = 1,
        UpdateLight = 2,
        UpdateKeyDate = 3,
        Sync = 4,
        RegisterNewOnly = 10,
        RegisterUpdateFull = 11,
        RegisterUpdateLight = 12
    }
    public enum ContactsUploadMethod
    {
        Sync=0,
        //AsyncDbCmd,
        QueueCommand=1,
        Auto=2,
    }

 
    //public enum ContactsIdType
    //{
    //    CellNumber,
    //    Email,
    //    ID,
    //    Idx,
    //}
}
