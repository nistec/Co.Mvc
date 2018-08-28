using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Netcell
{

    [Serializable]
    public class NetException : ApplicationException
    {
        protected AckStatus Status { get; private set; }
        protected int AccountId { get; private set; }
        protected string Method { get; private set; }

        public static string GetMethodFullName(System.Diagnostics.StackFrame frame)
        {
            return frame.GetMethod().ReflectedType.FullName + "." + frame.GetMethod().Name;
        }

        public static void Trace(AckStatus ack, int accountId, string msg)
        {
            string method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));//.Name;//.Module.FullyQualifiedName;
            new NetException(ack, accountId, msg, method);
        }
        public static void Trace(AckStatus ack, int accountId, Exception ex)
        {
            string method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            new NetException(ack, accountId, ex.Message, method);
        }
        public static void Trace(AckStatus ack, string msg)
        {
            string method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            new NetException(ack, 0, msg, method);
        }
        public static void Trace(AckStatus ack, Exception ex)
        {
            string method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            new NetException(ack, 0, ex.Message, method);
        }

        public static void Trace(AckStatus ack, string msg, params object[] args)
        {
            string method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            new NetException(ack, 0, string.Format(msg, args), method);
        }

        public NetException(AckStatus ack, int accountId, string msg, string method)
            : base(msg)
        {
            Method = method;
            Status = ack;
            AccountId = accountId;
            OnException(msg);
        }
        /// <summary>
        /// MessageException
        /// </summary>
        /// <param name="ack"></param>
        /// <param name="msg"></param>
        public NetException(AckStatus ack, string msg)
            : base(msg)
        {
            Method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            Status = ack;
            OnException(msg);
        }
        /// <summary>
        /// MessageException
        /// </summary>
        /// <param name="ack"></param>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public NetException(AckStatus ack, string msg, params object[] args)
            : base(msg)
        {
            Method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            Status = ack;
            OnException(string.Format(msg,args));
        }
        /// <summary>
        /// MessageException
        /// </summary>
        /// <param name="ack"></param>
        /// <param name="accountId"></param>
        /// <param name="msg"></param>
        public NetException(AckStatus ack, int accountId, string msg)
            : base(msg)
        {
            Method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            Status = ack;
            AccountId = accountId;
            OnException(msg);
        }
        /// <summary>
        /// MessageException
        /// </summary>
        /// <param name="ack"></param>
        /// <param name="msg"></param>
        public NetException(AckStatus ack, Exception ex)
            : base(ex.Message)
        {
            Method = GetMethodFullName(new System.Diagnostics.StackTrace().GetFrame(1));
            Status = ack;
            OnException(ex.Message);
        }

 
        protected virtual void OnException(string message)
        {
            //Log.ErrorFormat("NetException {0} message:{1}, Status:{2}, AccountId:{3}", Method, message, Status, AccountId);
        }
    }
}
