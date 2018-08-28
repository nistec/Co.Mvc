using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Netcell.Data;
using MControl.Sys;
using Netcell.Data.Server;

namespace Netcell.Remoting
{
    public class UserAuth : MControl.Data.ActiveRecord
    {
        public static int ValidateAccount(string LogInName, string Pass)
        {
            try
            {
                UserAuth ua = new UserAuth(LogInName, Pass);
                return ua.AccountId;
            }
            catch
            {
                return -1;
            }
        }

        public static int ValidateAuth(string LogInName, string Pass, int AccountId,string Ip)
        {
            try
            {
                int userId = 0;

                using (DalRule dal = new DalRule())
                {
                    dal.User_Auth_Lookup(LogInName, Pass, Ip, ref AccountId, ref userId);
                }

                return userId;

             }
            catch(Exception ex)
            {
                string err = ex.Message;
                return -1;
            }
        }

        public static UserAuth Auth(string LogInName, string Pass)
        {
            try
            {
                return new UserAuth(LogInName, Pass);
            }
            catch
            {
                return UserAuth.Empty;
            }
        }

        public UserAuth(string LogInName, string Pass)
        {
            try
            {
                if (LogInName == null || Pass == null)
                {
                    throw new MsgException(AckStatus.IllegalAuthentication, "null user name or password");
                }
                if (!RemoteUtil.IsAlphaNumeric(LogInName, Pass))
                {
                    throw new MsgException(AckStatus.IllegalAuthentication, "Illegal UserName or password, expect AlphaNumeric");
                }
                if (!RemoteUtil.IsValidString(LogInName) || !RemoteUtil.IsValidString(Pass))
                {
                    throw new MsgException(AckStatus.IllegalAuthentication, "Illeagal user name or password");
                }
                DataRow dr = null;
                using (DalRule dal = new DalRule())
                {
                    dr = dal.User_Auth(LogInName, Pass);
                }
                if (dr == null)
                {
                    throw new MsgException(AckStatus.AuthorizationException, "Un Authorized");
                }
                base.Init(dr);
                if (IsEmpty)
                {
                    throw new MsgException(AckStatus.AuthorizationException, "Un Authorized");
                }

                LogInName = GetStringValue("LogInName");
                Pass = GetStringValue("Pass");
                UserId = GetIntValue("UserId");
                AccountId = GetIntValue("AccountId");
                UserType = GetIntValue("UserType");
                ExState = 0;
                if (AccountId < 0)
                {
                    throw new MsgException(AckStatus.AuthorizationException, "Un Authorized");
                }

            }
            catch (MsgException mex)
            {
                throw mex;
            }
            catch (Exception ex)
            {
                throw new MsgException(AckStatus.SecurityLoginFaild, "Could not load  UserAuth" + ex.Message);
            }
        }

        public UserAuth(string LogInName, string Pass, string Ip)
        {
            try
            {
                if (LogInName == null || Pass == null)
                {
                    throw new MsgException(AckStatus.IllegalAuthentication, "null user name or password");
                }
                if (!RemoteUtil.IsAlphaNumeric(LogInName, Pass))
                {
                    throw new MsgException(AckStatus.IllegalAuthentication, "Illegal UserName or password, expect AlphaNumeric");
                }
                if (!RemoteUtil.IsValidString(LogInName) || !RemoteUtil.IsValidString(Pass))
                {
                    throw new MsgException(AckStatus.IllegalAuthentication, "Illeagal user name or password");
                }
                DataRow dr = null;
                using (DalRule dal = new DalRule())
                {
                    dr = dal.User_Auth(LogInName, Pass,Ip);
                }
                if (dr == null)
                {
                    throw new MsgException(AckStatus.AuthorizationException, "Un Authorized");
                }
                base.Init(dr);
                if (IsEmpty)
                {
                    throw new MsgException(AckStatus.AuthorizationException, "Un Authorized");
                }

                LogInName = GetStringValue("LogInName");
                Pass = GetStringValue("Pass");
                UserId = GetIntValue("UserId");
                AccountId = GetIntValue("AccountId");
                UserType = GetIntValue("UserType");
                ExState = 0;
                if (AccountId < 0)
                {
                    throw new MsgException(AckStatus.AuthorizationException, "Un Authorized");
                }

            }
            catch (MsgException mex)
            {
                throw mex;
            }
            catch (Exception ex)
            {
                throw new MsgException(AckStatus.SecurityLoginFaild, "Could not load  UserAuth" + ex.Message);
            }
        }

        public UserAuth(string LogInName, string Pass, int AccountId, string Ip, UsingTypes usingType, ExChecks exCheck)
        {
            int authState = 0;
            try
            {
                if (LogInName == null || Pass == null)
                {
                    throw new MsgException(AckStatus.IllegalAuthentication, "null user name or password");
                }
                if (!RemoteUtil.IsAlphaNumeric(LogInName, Pass))
                {
                    throw new MsgException(AckStatus.IllegalAuthentication, "Illegal UserName or password, expect AlphaNumeric");
                }
                if (!RemoteUtil.IsValidString(LogInName) || !RemoteUtil.IsValidString(Pass))
                {
                    throw new MsgException(AckStatus.IllegalAuthentication, "Illeagal user name or password");
                }

                DataRow dr = null;
                using (DalRule dal = new DalRule())
                {
                    dr = dal.User_Auth_Service(ref authState,LogInName, Pass, AccountId, (int)usingType,Ip,(int)exCheck);
                }
                AuthState AuthState = (AuthState)authState;
                if (AuthState !=  AuthState.Ok)
                {
                    throw new MsgException(AckStatus.AuthorizationException, "Un Authorized - " + AuthState.ToString());
                }
                if (dr == null)
                {
                    throw new MsgException(AckStatus.AuthorizationException, "Un Authorized");
                }
                base.Init(dr);
                if (IsEmpty)
                {
                    throw new MsgException(AckStatus.AuthorizationException, "Un Authorized");
                }

                LogInName = GetStringValue("LogInName");
                Pass = GetStringValue("Pass");
                UserId = GetIntValue("UserId");
                AccountId = GetIntValue("AccountId");
                UserType = GetIntValue("UserType");
                ExState = GetIntValue("ExState");
                if (AccountId < 0)
                {
                    throw new MsgException(AckStatus.AuthorizationException, "Un Authorized");
                }

            }
            catch (MsgException mex)
            {
                throw mex;
            }
            catch (Exception ex)
            {
                throw new MsgException(AckStatus.SecurityLoginFaild, "Could not load  UserAuth" + ex.Message);
            }
        }
        private UserAuth()
        {

        }
        public static UserAuth Empty
        {
            get { return new UserAuth(); }
        }
        public bool IsValid
        {
            get { return AccountId > 0 && UserId > 0; }
        }
        public string DecodedPass
        {
            get { return Encryption.DecryptPass(Pass); }
        }

        #region properties

        public readonly string LogInName;

        public readonly string Pass;

        public readonly int UserId;

        public readonly int AccountId;
        public readonly int UserType;
        public readonly int ExState;

        #endregion
    }

 
}
