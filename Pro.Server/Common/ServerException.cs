using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Nistec.Logging;

namespace Pro.Server
{

    public class ServerException : ApplicationException
    {
        string _Method;
        int _AccountId;
        bool _Alert;

        public static string GetMethodFullName(System.Diagnostics.StackFrame frame)
        {
            return frame.GetMethod().ReflectedType.FullName + "." + frame.GetMethod().Name;
        }

        public ServerException(string message, string method, int accountId)
            : base(message)
        {
            _Method = method;
            _AccountId = accountId;
            OnException(message);
        }
        public ServerException(string message, string method)
            : base(message)
        {
            _Method = method;
            _AccountId = 0;
            OnException(message);
        }
        public ServerException(string message)
            : base(message)
        {
            _Method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            _AccountId = 0;
            OnException(message);
        }

        //public ServerException(string message, params object[] args)
        //    : base(string.Format(message, args))
        //{
        //    _Method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
        //    OnException(string.Format(message, args));
        //}

      
        public ServerException(string message, string method, Exception ex)
            : base(message, ex)
        {
            _Method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            OnException(ex.Message + " Exception: " + ex.Message);
        }
        public ServerException(string message, Exception ex)
            : base(message, ex)
        {
            _Method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            OnException(ex.Message + " Exception: " + ex.Message);
        }
        public ServerException(Exception ex)
            : base(ex.Message, ex.InnerException)
        {
            _Method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            OnException(ex.Message);
        }

        protected void OnException(string message)
        {
            if (_Method != null && _AccountId >0)
            {
                Netlog.ErrorFormat("ServerException: Method:{0}, Account={1} Message={2}", _Method, _AccountId, message);
            }
            else if (_Method != null)
            {
                Netlog.ErrorFormat("ServerException: Method:{0}, Message={1}", _Method, message);
            }
            else
            {
                Netlog.ErrorFormat("ServerException: Message={0}", message);
            }

            if (_Alert)
            {
                //TODO insert alert
                try
                {
                    //using (DalTrace dal = new DalTrace())
                    //{
                    //    dal.Exceptions_Insert(message, 0, method, (int)status, accountId);
                    //}
                }
                catch
                {

                }
            }

        }
    }
}
