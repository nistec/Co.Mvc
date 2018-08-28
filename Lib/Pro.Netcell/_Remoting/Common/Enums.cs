using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netcell.Remoting
{
    //bitMask
    public enum UsingTypes
    {
        None = 0,
        Ephone = 1,
        Api = 2
        //Co=4
    }

    public enum ExChecks
    {
        None = 0,
        SmsBlocked = 1,
        MailBlocked = 2
    }

    public enum AuthState
    {
        Ok=1,
        None = 0,
        UnAuthorized = -1,
        IpNotAllowed = -2,
        MethodNotAlloowed=-3,
    }
}
