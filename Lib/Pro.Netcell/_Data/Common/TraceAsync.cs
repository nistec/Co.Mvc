using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MControl;
using System.Data.SqlClient;
using System.Data;
using Netcell.Data.Common;
using MControl.Data.Factory;
using Netcell.Data.Db;
using Netcell.Data.DbTrace;

namespace Netcell.Data
{
    public class TraceAsync
    {
        //public static TraceAsync Instance
        //{
        //    get
        //    {
        //        return new TraceAsync();
        //    }
        //}

        #region Invoke async

        public static void Execute(string cmd, params object[] args)
        {
            RenderTraceAsync(new InvokePublishDelegate(AsyncTraceWorker), cmd, args);
        }

        delegate void InvokePublishDelegate(string cmd, object[] args);

        static void AsyncTraceWorker(string cmd, object[] args)
        {
            if (args == null)
            {
                return;
            }

            try
            {
                switch (cmd)
                {
                    case "publish":
                        Publish_Comments(args);
                        //using (DalController dal = new DalController())
                        //{
                        //    dal.Publish_Comments_Insert(args);
                        //}
                        break;
                    case "publish-dest":
                        Publish_DestKey(args);
                        break;
                    case "exception":
                        Exceptions_Insert(args);
                        break;
                }

            }
            catch (Exception ex)
            {
                string s = ex.Message;
                //MsgException.Trace(AckStatus.NetworkError, AccountId, "InvokeDynamicAsync Exception:" + ex.Message);
            }
        }


        /// <summary>
        /// Delegate to wrap another delegate and its arguments
        /// </summary>
        delegate void DelegateWrapper(Delegate d, object[] args);

        /// <summary>
        /// An instance of DelegateWrapper which calls InvokeWrappedDelegate,
        /// which in turn calls the DynamicInvoke method of the wrapped
        /// delegate.
        /// </summary>
        static DelegateWrapper wrapperInstance = new DelegateWrapper(InvokeWrappedDelegate);

        /// <summary>
        /// Callback used to call <code>EndInvoke</code> on the asynchronously
        /// invoked DelegateWrapper.
        /// </summary>
        static AsyncCallback callback = new AsyncCallback(EndWrapperInvoke);

        /// <summary>
        /// Executes the specified delegate with the specified arguments
        /// asynchronously on a thread pool thread.
        /// </summary>
        static void RenderTraceAsync(Delegate d, params object[] args)
        {
            // Invoke the wrapper asynchronously, which will then
            // execute the wrapped delegate synchronously (in the
            // thread pool thread)
            wrapperInstance.BeginInvoke(d, args, callback, null);
        }

        /// <summary>
        /// Invokes the wrapped delegate synchronously
        /// </summary>
        static void InvokeWrappedDelegate(Delegate d, object[] args)
        {
            d.DynamicInvoke(args);
        }

        /// <summary>
        /// Calls EndInvoke on the wrapper and Close on the resulting WaitHandle
        /// to prevent resource leaks.
        /// </summary>
        static void EndWrapperInvoke(IAsyncResult ar)
        {
            wrapperInstance.EndInvoke(ar);
            ar.AsyncWaitHandle.Close();
        }

        #endregion

        public static int Publish_Comments(object[] values)
        {
            if (values == null || values.Length < 7)
            {
                return -1;
            }

            SqlParameter[] parameters = DalUtil.CreateParameters(new string[] { "PublishKey", "ItemId", "AckStatus", "PublishState", "PublishComment", "SessionId", "Server" }, values);
            using (IDbCmd cmd = DbFactory.Create<NetcellDB>())
            {
                return cmd.ExecuteNonQuery("sp_Publish_Comment", parameters, CommandType.StoredProcedure);
            }
        }

        public static int Publish_DestKey(object[] values)
        {
            if (values == null || values.Length < 7)
            {
                return -1;
            }
            if (values[0] == null)
            {
                return -1;
            }
            string PublishKey = null;
            string SessionId = null;

            if (!Counters.ParseAltKey(values[0].ToString(), ref SessionId, ref PublishKey))
            {
                return -1;
            }
            values[0] = PublishKey;
            values[1] = SessionId;
            SqlParameter[] parameters = DalUtil.CreateParameters(new string[] { "PublishKey", "SessionId", "ItemId", "AckStatus", "PublishState", "PublishComment", "Server" }, values);
            using (IDbCmd cmd = DbFactory.Create<NetcellDB>())
            {
                return cmd.ExecuteNonQuery("sp_Publish_Comment", parameters, CommandType.StoredProcedure);
            }
        }

        
        public static int Exceptions_Insert(object[] values)
        {
            if (values == null || values.Length < 5)
            {
                return -1;
            }
            SqlParameter[] parameters = DalUtil.CreateParameters(new string[] { "Description", "Priority", "Method", "Status", "AccountId" }, values);
            using (IDbCmd cmd = DbFactory.Create<Netcell_Trace>())
            {
                return cmd.ExecuteNonQuery("insert into Exceptions(Description, Priority, Method, Status, AccountId) values(@Description, @Priority, @Method, @Status, @AccountId)", parameters);
            }
        }

    }
}
