using Netcell.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netcell.Remoting
{
    public class AppSession
    {

        public static int Session_Add
         (
         string SessionId,
         SessionApps AppId,
         string SessionValue,
         string SC,
         int State,
         int ttl)
        {
            try
            {
                using (var dal = new DalController())
                {
                    return dal.Session_Exec(0, SessionId, (int)AppId, SessionValue, SC, State, ttl);
                }
            }
            catch (Exception)
            { return -1; }
        }
        public static int Session_Update
           (
           string SessionId,
           SessionApps AppId,
           string SessionValue,
           string SC,
           int State)
        {
            try
            {
                using (var dal = new DalController())
                {
                    return dal.Session_Exec(1, SessionId, (int)AppId, SessionValue, SC, State, 0);
                }
            }
            catch (Exception)
            { return -1; }
        }
        public static int Session_Remove
           (
           string SessionId,
           SessionApps AppId)
        {
            try
            {
                using (var dal = new DalController())
                {
                    return dal.Session_Exec(2, SessionId, (int)AppId, null, null, 0, 0);
                }
            }
            catch (Exception)
            { return -1; }
        }
    }
}
