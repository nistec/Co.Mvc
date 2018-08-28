using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Netcell.Remoting
{
 
    [Serializable]
    public class AppException : ApplicationException
    {
        protected AckStatus _AckStatus;
        protected int _AccountId;
        protected string _Method;

        //public static void Trace(AckStatus ack, int accountId, string msg)
        //{
        //    string method = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;
        //    new AppException(ack, accountId, msg, method);
        //}
        //public static void Trace(AckStatus ack, string msg)
        //{
        //    string method = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;
        //    new AppException(ack,0, msg,method);
        //}
        //public static void Trace(AckStatus ack, Exception ex)
        //{
        //    string method = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;
        //    new AppException(ack, 0, ex.Message, method);
        //}
        public AppException(string msg)
            : base(msg)
        {
        }

        public AppException(AckStatus ack, int accountId, string msg, string method)
            : base(msg)
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
        public AppException(AckStatus ack, string msg): base(msg)
        {
            _Method = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;
            _AckStatus = ack;
            OnException(msg);
        }
        /// <summary>
        /// MessageException
        /// </summary>
        /// <param name="ack"></param>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public AppException(AckStatus ack, string msg, params object[] args)
            : base(string.Format(msg, args))
        {
            _Method = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;
            _AckStatus = ack;
            OnException(msg);
        }
         /// <summary>
        /// MessageException
        /// </summary>
        /// <param name="ack"></param>
        /// <param name="accountId"></param>
        /// <param name="msg"></param>
        public AppException(AckStatus ack, int accountId, string msg)
            : base(msg)//base(ack, msg)
        {
            _Method = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;
            _AckStatus = ack;
            _AccountId = accountId;
            OnException(msg);
        }
        /// <summary>
        /// MessageException
        /// </summary>
        /// <param name="ack"></param>
        /// <param name="msg"></param>
        public AppException(AckStatus ack, Exception ex)
            : base(ex.Message)
        {
            _Method = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;
            _AckStatus = ack;
            OnException(ex.Message);
        }

        /// <summary>
        /// MessageException
        /// </summary>
        /// <param name="ack"></param>
        /// <param name="msg"></param>
        public AppException(AckStatus ack, string msg, Exception ex)
            : base(msg,ex)
        {
            _Method = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;
            _AckStatus = ack;
            OnException(msg);
        }


        /// <summary>
        /// Method 
        /// </summary>
        public string Method
        {
            get { return _Method; }
        }

        /// <summary>
        /// Acknowledge Status
        /// </summary>
        public AckStatus Status
        {
            get { return _AckStatus; }
        }
        /// <summary>
        /// AccountId 
        /// </summary>
        public int AccountId
        {
            get { return _AccountId; }
        }

        protected virtual void OnException(string message)
        {
            //DalTrace.Instance.Exceptions_Insert(message, 0, Method, (int)Status, AccountId);

            //Nistec.Tasker.Remote.TaskerClient.RemoteClient.AddTask( Nistec.Tasker.TaskerKey.SqlCommand,"",""
            //Log.ErrorFormat("AppException message:{0}, Method:{1}, Status:{2}, AccountId:{3}", message, Method, Status, AccountId);
            try
            {
               // RemoteApi.Instance.ExecuteTrace_Exception(message, 0, Method, (int)Status, AccountId);
            }
            catch
            {

            }
        }
     
    }
}
