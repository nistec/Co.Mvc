using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Nistec.Data;
using Nistec;
using Nistec.Serialization;
using Nistec.Web.Security;

namespace Pro.Netcell.Api
{
    // [DataContract]
    public class DataAuth
    {
        public int AccountId { get; set; }

        public string UserName { get; set; }

        public string UserPass { get; set; }

        public int UserId { get; private set; }

        public void ValidateAuth(string clientIp)
        {
            //if (UserName == null || UserPass == null)
            //{
            //    throw new ArgumentException("Invalid Auth arguments");
            //}

            //if (!Authorizer.IsAlphaNumeric(UserName, UserPass))
            //{
            //    throw new ArgumentException("Illegal UserName or password");
            //}

            var user = AuthorizerService.AuthApi(UserName, UserPass, AccountId, ApiSettings.AppId, clientIp);

            if (user == null || user.State != (int)AuthState.Succeeded || user.UserId <= 0)
            {
                throw new ApiException((int)AuthState.UnAuthorized, "Access is denied");
            }
        }
    }

}