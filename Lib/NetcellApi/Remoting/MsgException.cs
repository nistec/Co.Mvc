using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Nistec.Logging;
using Netcell.Data.Db;

namespace Netcell.Remoting
{

    [Serializable]
    public class MsgException : NetException
    {
        //AckStatus _AckStatus;
        //int _AccountId;
        //string _Method;

        //public static string GetMethodFullName(System.Diagnostics.StackFrame frame)
        //{
        //    return frame.GetMethod().ReflectedType.FullName + "." + frame.GetMethod().Name;
        //}

        //public static void Trace(AckStatus ack, int accountId, string msg)
        //{
        //    string method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));//.Name;//.Module.FullyQualifiedName;
        //    new MsgException(ack, accountId, msg, method);
        //}
        //public static void Trace(AckStatus ack, int accountId, Exception ex)
        //{
        //    string method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
        //    new MsgException(ack, accountId, ex.Message, method);
        //}
        //public static void Trace(AckStatus ack, string msg)
        //{
        //    string method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
        //    new MsgException(ack, 0, msg, method);
        //}
        //public static void Trace(AckStatus ack, Exception ex)
        //{
        //    string method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
        //    new MsgException(ack, 0, ex.Message, method);
        //}

        //public static void Trace(AckStatus ack, string msg, params object[] args)
        //{
        //    string method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
        //    new MsgException(ack, 0, string.Format(msg, args), method);
        //}

        public MsgException(AckStatus ack, int accountId, string msg, string method)
            : base(ack, accountId, msg, method)
        {
        }
        /// <summary>
        /// MessageException
        /// </summary>
        /// <param name="ack"></param>
        /// <param name="msg"></param>
        public MsgException(AckStatus ack, string msg)
            : base(ack, msg)
        {
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
        }
        /// <summary>
        /// MessageException
        /// </summary>
        /// <param name="ack"></param>
        /// <param name="msg"></param>
        public MsgException(AckStatus ack, Exception ex)
            : base(ack, ex)
        {
        }

        protected override void OnException(string message)
        {
            //Log.ErrorFormat("MsgException: Method:{0}, AckStatus={1} Message={2}", Method, Status, message);
            base.OnException(message);

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
                          Trace_Insert(1,Method, Status, AccountId, message);
                        break;
                    default:
                        if (((int)Status) > 5000)
                        {
                            //fatal
                            //ActiveSystemAlert.AsyncAlertAction(3, Status, AccountId, message);
                             Trace_Insert(3,Method, Status, AccountId, message);
                        }
                        else
                        {
                            //mail
                            //ActiveSystemAlert.AsyncAlertAction(1, Status, AccountId, message);
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
    public class SecurityException : NetException 
    {
       
        public SecurityException(AckStatus ack, string msg)
            : base(ack, 0, msg)
        {
              OnException(msg);
        }
      
        protected override void OnException(string message)
        {
            Netlog.ErrorFormat("SecurityException: AckStatus={0} Message={1}", Status, message);

            if (((int)Status) < 500)
                return;

            switch (Status)
            {
                case AckStatus.SecurityBlockedAddress:
                case AckStatus.SecurityLoginOver:
                     //ActiveSystemAlert.AsyncAlertAction(1, Status, AccountId, message);
                     //DalTrace.Instance.Exceptions_Insert(message, 0, Method, (int)Status, AccountId);
                      MsgException.Trace_Insert(1,Method, Status, AccountId, message);
                    break;
            }

        }

    }
}
