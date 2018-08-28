using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Netcell.Data.DbTrace;

namespace Netcell.Remoting
{

    [Serializable]
    public class MsgException : AppException //ApplicationException
    {
        //AckStatus _AckStatus;
        //int _AccountId;
        //string _Method;

        public static string GetMethodFullName(System.Diagnostics.StackFrame frame)
        {
            return frame.GetMethod().ReflectedType.FullName + "." + frame.GetMethod().Name;
        }

        public static void Trace(AckStatus ack, int accountId, string msg)
        {
            string method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));//.Name;//.Module.FullyQualifiedName;
            new MsgException(ack, accountId, msg, method);
        }
        public static void Trace(AckStatus ack, int accountId, Exception ex)
        {
            string method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            new MsgException(ack, accountId, ex.Message, method);
        }
        public static void Trace(AckStatus ack, string msg)
        {
            string method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            new MsgException(ack, 0, msg, method);
        }
        public static void Trace(AckStatus ack, Exception ex)
        {
            string method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            new MsgException(ack, 0, ex.Message, method);
        }

        public static void Trace(AckStatus ack, string msg, params object[] args)
        {
            string method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            new MsgException(ack, 0, string.Format(msg, args), method);
        }

        public MsgException(AckStatus ack, int accountId, string msg, string method)
            : base(ack, accountId, msg, method)
        {
            _Method = method;
            _AckStatus = ack;
            _AccountId = accountId;
            OnException(msg);
        }
        /// <summary>
        /// MessageException
        /// </summary>
        /// <param name="ack"></param>
        /// <param name="msg"></param>
        public MsgException(AckStatus ack, string msg)
            : base(ack, msg)
        {
            _Method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            _AckStatus = ack;
            OnException(msg);
        }
        /// <summary>
        /// MessageException
        /// </summary>
        /// <param name="ack"></param>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public MsgException(AckStatus ack, string msg, params object[] args)
            : base(ack, msg, args)
        {
            _Method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            _AckStatus = ack;
            OnException(string.Format(msg,args));
        }
        /// <summary>
        /// MessageException
        /// </summary>
        /// <param name="ack"></param>
        /// <param name="accountId"></param>
        /// <param name="msg"></param>
        public MsgException(AckStatus ack, int accountId, string msg)
            : base(ack, accountId, msg)
        {
            _Method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            _AckStatus = ack;
            _AccountId = accountId;
            OnException(msg);
        }
        /// <summary>
        /// MessageException
        /// </summary>
        /// <param name="ack"></param>
        /// <param name="msg"></param>
        public MsgException(AckStatus ack, Exception ex)
            : base(ack, ex)
        {
            _Method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            _AckStatus = ack;
            OnException(ex.Message);
        }

        ///// <summary>
        ///// MessageException
        ///// </summary>
        ///// <param name="ack"></param>
        ///// <param name="msg"></param>
        //public MsgException(AckStatus ack, string msg, Exception ex)
        //    : base(ack, msg, ex)
        //{
        //    _Method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
        //    _AckStatus = ack;
        //    OnException(msg);
        //}

        ///// <summary>
        ///// Method 
        ///// </summary>
        //public string Method
        //{
        //    get { return _Method; }
        //}
        ///// <summary>
        ///// Acknowledge Status
        ///// </summary>
        //public AckStatus Status
        //{
        //    get { return _AckStatus; }
        //}
        ///// <summary>
        ///// AccountId 
        ///// </summary>
        //public int AccountId
        //{
        //    get { return _AccountId; }
        //}

        protected override void OnException(string message)
        {
            Log.ErrorFormat("MsgException: Method:{0}, AckStatus={1} Message={2}", Method, Status, message);

            if (((int)Status) < 500)
                return;
            try
            {
                switch (Status)
                {
                    //case AckStatus.Received:
                    //case AckStatus.Delivered:
                    //case AckStatus.None:
                    //case AckStatus.Ok:
                    //    break;
                    case AckStatus.ApplicationException:
                    case AckStatus.CacheException:
                    case AckStatus.NotEnoughCredit:
                    case AckStatus.InvalidContent:
                    case AckStatus.BillingException:
                    case AckStatus.NetworkError:
                    case AckStatus.FatalCarrierException:
                    case AckStatus.CarrierNotResponse:
                        //sms
                        //ActiveSystemAlert.AsyncAlertAction(1, Status, AccountId, message);
                        //DalTrace.Instance.Exceptions_Insert(message, 0, Method, (int)Status, AccountId);
                        if (ViewConfig.EnableRemote)
                        {
                            RemoteApi.Instance.ExecuteSystemAlert(1, (int)Status, AccountId, message, 0, Method);
                        }
                        else
                            Trace_Insert(1,Method, Status, AccountId, message);
                        break;
                    default:
                        if (((int)Status) > 5000)
                        {
                            //fatal
                            //ActiveSystemAlert.AsyncAlertAction(3, Status, AccountId, message);
                            if (ViewConfig.EnableRemote)
                            {
                                RemoteApi.Instance.ExecuteSystemAlert(3, (int)Status, AccountId, message, 0, Method);
                            }
                            else
                                Trace_Insert(3,Method, Status, AccountId, message);
                        }
                        else
                        {
                            //mail
                            //ActiveSystemAlert.AsyncAlertAction(1, Status, AccountId, message);
                            if (ViewConfig.EnableRemote)
                            {
                                RemoteApi.Instance.ExecuteSystemAlert(1, (int)Status, AccountId, message, 0, Method);
                            }
                            else
                                Trace_Insert(1,Method, Status, AccountId, message);
                        }
                        //DalTrace.Instance.Exceptions_Insert(message, 0, Method, (int)Status, AccountId);
                        break;
                }
            }
            catch
            {
                Trace_Insert(1, Method,Status, AccountId, message);
            }

        }

        public static void Trace_Insert(int actionType, string method, AckStatus status, int accountId, string message)
        {
            try
            {
                using (DalTrace dal = new DalTrace())
                {
                    dal.Exceptions_Insert(message, 0, method, (int)status, accountId);
                }
            }
            catch
            {

            }
        }

    }

    [Serializable]
    public class SecurityException : AppException 
    {
       
        public SecurityException(AckStatus ack, string msg)
            : base(ack, 0, msg)
        {
              OnException(msg);
        }
      
        protected override void OnException(string message)
        {
            Log.ErrorFormat("SecurityException: AckStatus={0} Message={1}", Status, message);

            if (((int)Status) < 500)
                return;

            switch (Status)
            {
                case AckStatus.SecurityBlockedAddress:
                case AckStatus.SecurityLoginOver:
                     //ActiveSystemAlert.AsyncAlertAction(1, Status, AccountId, message);
                     //DalTrace.Instance.Exceptions_Insert(message, 0, Method, (int)Status, AccountId);
                    if (ViewConfig.EnableRemote)
                    {
                        RemoteApi.Instance.ExecuteSystemAlert(1, (int)Status, AccountId, message, 0, Method);
                    }
                    else
                        MsgException.Trace_Insert(1,Method, Status, AccountId, message);
                    break;
            }

        }

    }
}
