#define LOG4// LOGNET | LOG4

using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using log4net;
using System.Text;


namespace Netcell
{

#if LOG4
    public static class Log
    {


        static Log()
        {
            Start();
        }

        public static void Stop()
        {
            logger.Info("Logger stoped...");
            //if (logger.IsInfoEnabled) logger.Info("Logger InfoEnabled Stop");
            // It's not possible to use shutdown hooks in the .NET Compact Framework,
            // so you have manually shutdown log4net to free all resoures.
            LogManager.Shutdown();

        }

        public static void Start()
        {
            // Uncomment the next line to enable log4net internal debugging
            // log4net.helpers.LogLog.InternalDebugging = true;

            // This will instruct log4net to look for a configuration file
            // called ConsoleApp.exe.config in the application base
            // directory (i.e. the directory containing ConsoleApp.exe)
            log4net.Config.XmlConfigurator.Configure();

            // Create a logger
            logger = LogManager.GetLogger(typeof(Log));
            //logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //logger = log4net.LogManager.GetLogger(System.Reflection.Assembly.GetCallingAssembly(), "Log");

            // Log an info level message
            //if (logger.IsInfoEnabled) logger.Info("Logger InfoEnabled");

            // Invoke static LogEvents method on LoggingExample class
            //LoggingExample.LogEvents();

            //Console.Write("Press Enter to exit...");
            //Console.ReadLine();
            logger.Info("Logger started...");

        }

        static log4net.ILog logger;// = log4net.LogManager.GetLogger(System.Reflection.Assembly.GetCallingAssembly(), "Log");


        public static log4net.ILog Logger
        {
            get { return logger; }
        }

        public static void Debug(string message)
        {
            //if (logger.IsDebugEnabled)
             logger.Debug(message);
        }
        public static void Info(string message)
        {
            //if (logger.IsInfoEnabled) 
            logger.Info(message);
        }
        public static void Warn(string message)
        {
            //if (logger.IsWarnEnabled) 
            logger.Warn(message);
        }
        public static void Error(string message)
        {
            //if (logger.IsErrorEnabled) 
            logger.Error(message);
        }
        public static void Fatal(string message)
        {
            //if (logger.IsFatalEnabled) 
            logger.Fatal(message);
        }

        public static void Error(string message, Exception ex)
        {
            // if (logger.IsErrorEnabled) 
            logger.Error(message, ex);
        }
        public static void Fatal(string message, Exception ex)
        {
            // if (logger.IsFatalEnabled) 
            logger.Fatal(message, ex);
        }


        public static void DebugFormat(string message, params object[] args)
        {
            //if (logger.IsDebugEnabled)
                logger.DebugFormat(message, args);
        }
        public static void InfoFormat(string message, params object[] args)
        {
            //if (logger.IsInfoEnabled) 
            logger.InfoFormat(message, args);
        }
        public static void WarnFormat(string message, params object[] args)
        {
            //if (logger.IsWarnEnabled) 
            logger.WarnFormat(message, args);
        }
        public static void ErrorFormat(string message, params object[] args)
        {
            //if (logger.IsErrorEnabled) 
            logger.ErrorFormat(message, args);
        }
        public static void FatalFormat(string message, params object[] args)
        {
            //if (logger.IsFatalEnabled) 
            logger.FatalFormat(message, args);
        }


        public static void Exception(string message, Exception e)
        {
            Exception(message,e,false);
        }

        public static void Exception(string message, Exception e, bool addStackTrace)
        {
            //if (!logger.IsErrorEnabled) return;

            StringBuilder sb = new StringBuilder();
            sb.Append(message);
            Exception innerEx = null;
            if (e != null)
            {
                sb.Append(e.Message);
                innerEx = e.InnerException;
                while (innerEx != null)
                {
                    sb.Append("innerEx: ");
                    sb.Append(innerEx.Message);
                    innerEx = innerEx.InnerException;
                }
                if (addStackTrace)
                {
                    sb.AppendLine();
                    sb.AppendFormat("StackTrace: {0} ", e.StackTrace);
                }
            }
            logger.Error(sb.ToString());
        }


    }
#else

    public static class Log
    {


        static Log()
        {
            Start();
        }

        public static void Stop()
        {
            logger.Info("Logger stoped...");
            //if (logger.IsInfoEnabled) logger.Info("Logger InfoEnabled Stop");
            // It's not possible to use shutdown hooks in the .NET Compact Framework,
            // so you have manually shutdown log4net to free all resoures.
            LogManager.Shutdown();

        }

        public static void Start()
        {
            // Uncomment the next line to enable log4net internal debugging
            // log4net.helpers.LogLog.InternalDebugging = true;

            // This will instruct log4net to look for a configuration file
            // called ConsoleApp.exe.config in the application base
            // directory (i.e. the directory containing ConsoleApp.exe)
            log4net.Config.XmlConfigurator.Configure();

            // Create a logger
            logger = LogManager.GetLogger(typeof(Log));
            //logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //logger = log4net.LogManager.GetLogger(System.Reflection.Assembly.GetCallingAssembly(), "Log");

            // Log an info level message
            //if (logger.IsInfoEnabled) logger.Info("Logger InfoEnabled");

            // Invoke static LogEvents method on LoggingExample class
            //LoggingExample.LogEvents();

            //Console.Write("Press Enter to exit...");
            //Console.ReadLine();
            logger.Info("Logger started...");

        }

        static log4net.ILog logger;// = log4net.LogManager.GetLogger(System.Reflection.Assembly.GetCallingAssembly(), "Log");


        public static log4net.ILog Logger
        {
            get { return logger; }
        }

        public static void Debug(string message)
        {
            //if (logger.IsDebugEnabled)
             logger.Debug(message);
        }
        public static void Info(string message)
        {
            //if (logger.IsInfoEnabled) 
            logger.Info(message);
        }
        public static void Warn(string message)
        {
            //if (logger.IsWarnEnabled) 
            logger.Warn(message);
        }
        public static void Error(string message)
        {
            //if (logger.IsErrorEnabled) 
            logger.Error(message);
        }
        public static void Fatal(string message)
        {
            //if (logger.IsFatalEnabled) 
            logger.Fatal(message);
        }

        public static void Error(string message, Exception ex)
        {
            // if (logger.IsErrorEnabled) 
            logger.Error(message, ex);
        }
        public static void Fatal(string message, Exception ex)
        {
            // if (logger.IsFatalEnabled) 
            logger.Fatal(message, ex);
        }


        public static void DebugFormat(string message, params object[] args)
        {
            //if (logger.IsDebugEnabled)
                logger.DebugFormat(message, args);
        }
        public static void InfoFormat(string message, params object[] args)
        {
            //if (logger.IsInfoEnabled) 
            logger.InfoFormat(message, args);
        }
        public static void WarnFormat(string message, params object[] args)
        {
            //if (logger.IsWarnEnabled) 
            logger.WarnFormat(message, args);
        }
        public static void ErrorFormat(string message, params object[] args)
        {
            //if (logger.IsErrorEnabled) 
            logger.ErrorFormat(message, args);
        }
        public static void FatalFormat(string message, params object[] args)
        {
            //if (logger.IsFatalEnabled) 
            logger.FatalFormat(message, args);
        }


        public static void Exception(string message, Exception e)
        {
            Exception(message,e,false);
        }

        public static void Exception(string message, Exception e, bool addStackTrace)
        {
            //if (!logger.IsErrorEnabled) return;

            StringBuilder sb = new StringBuilder();
            sb.Append(message);
            Exception innerEx = null;
            if (e != null)
            {
                sb.Append(e.Message);
                innerEx = e.InnerException;
                while (innerEx != null)
                {
                    sb.Append("innerEx: ");
                    sb.Append(innerEx.Message);
                    innerEx = innerEx.InnerException;
                }
                if (addStackTrace)
                {
                    sb.AppendLine();
                    sb.AppendFormat("StackTrace: {0} ", e.StackTrace);
                }
            }
            logger.Error(sb.ToString());
        }


    }

#endif


    //public enum LoggerPriority
    //{
    //    Debug,
    //    Info,
    //    Warning,
    //    Error
    //}
    ///// <summary>
    ///// Generates A TraceWriter For Application Configuration Settings
    ///// </summary>
    ////[System.Runtime.InteropServices.ComVisible(false)]
    //class LogWriter
    //{
    //    #region Fields
    //    /// <summary>
    //    /// The Internal Logger
    //    /// </summary>
    //    private static readonly log4net.ILog __Log4Net;
    //    #endregion

    //    #region Constructor
    //    /// <summary>
    //    /// Initialized Underline Logger
    //    /// </summary>
    //    static LogWriter()
    //    {
    //        //Type typeCaller = __GetCallingMethodType();
    //        //if (typeCaller == null)
    //        //{
    //        //    typeCaller = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
    //        //}

    //        //// Create a logger for use in this class
    //        //__Log4Net = log4net.LogManager.GetLogger(typeCaller.Assembly, typeCaller);


    //        //System.Reflection.MethodBase methodBase = (System.Reflection.MethodBase)(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod());
    //        //Type typeCaller =  methodBase.DeclaringType;
    //        Type typeCaller = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;

    //        // Create a logger for use in this class
    //        __Log4Net = log4net.LogManager.GetLogger(typeCaller.Name);

    //        }

    //    public static void WriteLog(/*string nameSpace,*/ string Module, string transactionId ,string message, LoggerPriority Levels)
    //    {
    //        switch(Levels)
    //        {
    //            case LoggerPriority.Error:
    //                WriteError("Module: " + Module + " OrderId: " + transactionId + " " + message);
    //                break;
    //            case LoggerPriority.Info:
    //                WriteInfo("OrderId:" + transactionId + " "  + message);
    //                break;
    //            case LoggerPriority.Warning:
    //                WriteWarning("OrderId:" + transactionId + " "  + message);
    //                break;
    //            case LoggerPriority.Debug:
    //                WriteDebug("OrderId:" + transactionId + " " + message);
    //                break;
    //            default:				
    //                WriteInfo("OrderId:" + transactionId + " "  + message);
    //                break;

    //        }

    //    }

    //    #endregion

    //    #region Methods

    //    private static string GetMethodBase()
    //    {
    //        System.Reflection.MethodBase methodBase = (System.Reflection.MethodBase)(new System.Diagnostics.StackTrace().GetFrame(2).GetMethod());
    //        return methodBase.DeclaringType.Name + "." + methodBase.Name;
    //    }
    //    private static string GetDeclaringMethod()
    //    {
    //        System.Reflection.MethodBase methodBase = (System.Reflection.MethodBase)(new System.Diagnostics.StackTrace().GetFrame(2).GetMethod());
    //        return methodBase.DeclaringType.Namespace + "." + methodBase.DeclaringType.Name + "." + methodBase.Name;
    //    }


    //    /// <summary>
    //    /// Writes Error Information To Trace
    //    /// </summary>
    //    /// <param name="module"></param>
    //    /// <param name="orderId"></param>
    //    /// <param name="message"></param>
    //    public static void WriteError(string module, string orderId, string message)
    //    {
    //        __Log4Net.Error(string.Format("{0} OrderId: {1} Msg: {2}", module, orderId, message));
    //    }

    //    /// <summary>
    //    /// Writes Error Information To Trace
    //    /// </summary>
    //    /// <param name="module"></param>
    //    /// <param name="orderId"></param>
    //    /// <param name="type"></param>
    //    /// <param name="message"></param>
    //    public static void WriteError(string module, string orderId, string type, string message)
    //    {
    //        __Log4Net.Error(string.Format("{0}{1} Type: {2} Msg: {3}", module, string.IsNullOrEmpty(orderId) ? "":" OrderId: " + orderId,type, message));
    //    }

    //    /// <summary>
    //    /// Writes Error Information To Trace
    //    /// </summary>
    //    /// <param name="message"></param>
    //    public static void WriteError(string message)
    //    {
    //        __Log4Net.Error(string.Format("{0} Msg: {1}", GetMethodBase(), message));

    //        //__Log4Net.Error(message);
    //    }
    //    /// <summary>
    //    /// Writes Error Information To Trace
    //    /// </summary>
    //    /// <param name="orderId"></param>
    //    /// <param name="message"></param>
    //    public static void WriteError(string orderId,string message)
    //    {
    //        __Log4Net.Error(string.Format("{0} {1} Msg: {2}", GetMethodBase(),orderId, message));
    //    }

    //    /// <summary>
    //    /// Writes Error Information To Trace
    //    /// </summary>
    //    /// <param name="format"></param>
    //    /// <param name="args"></param>
    //    public static void WriteError(string format, params object[] args)
    //    {
    //        WriteError(string.Format(format, args));
    //    }
    //    /// <summary>
    //    /// Writes Error Information To Trace
    //    /// </summary>
    //    /// <param name="module"></param>
    //    /// <param name="orderId"></param>
    //    /// <param name="message"></param>
    //    public static void WriteWarning(string module, string orderId, string message)
    //    {
    //        __Log4Net.Warn(string.Format("Module: {0} OrderId: {1} Msg: {2}", module, orderId, message));
    //    }

    //    /// <summary>
    //    /// Writes Warning Information To Trace
    //    /// </summary>
    //    /// <param name="message"></param>
    //    public static void WriteWarning(string message)
    //    {
    //        __Log4Net.Warn(string.Format("{0} Msg: {1}", GetMethodBase(), message));
    //        //__Log4Net.Warn(message);
    //    }
    //    /// <summary>
    //    /// Writes Warning Information To Trace
    //    /// </summary>
    //    /// <param name="orderId"></param>
    //    /// <param name="message"></param>
    //    public static void WriteWarning(string orderId, string message)
    //    {
    //        __Log4Net.Warn(string.Format("{0} {1} Msg: {2}", GetMethodBase(), orderId, message));
    //    }

    //    /// <summary>
    //    /// Writes Warning Information To Trace
    //    /// </summary>
    //    /// <param name="format"></param>
    //    /// <param name="args"></param>
    //    public static void WriteWarning(string format, params object[] args)
    //    {
    //        WriteWarning(string.Format(format, args));
    //    }
    //    /// <summary>
    //    /// Writes Warning Information To Trace
    //    /// </summary>
    //    /// <param name="module"></param>
    //    /// <param name="orderId"></param>
    //    /// <param name="message"></param>
    //    public static void WriteInfo(string module, string orderId, string message)
    //    {
    //        __Log4Net.Info(string.Format("{0} OrderId: {1} Msg: {1}", module, orderId, message));
    //    }

    //    /// <summary>
    //    /// Writes Info Information To Trace
    //    /// </summary>
    //    /// <param name="message"></param>
    //    public static void WriteInfo(string message)
    //    {
    //        __Log4Net.Info(string.Format("{0} Msg: {1}", GetMethodBase(), message));
    //        //__Log4Net.Info(message);
    //    }

    //    /// <summary>
    //    /// Writes Info Information To Trace
    //    /// </summary>
    //    /// <param name="orderId"></param>
    //    /// <param name="message"></param>
    //    public static void WriteInfo(string orderId, string message)
    //    {
    //        __Log4Net.Info(string.Format("{0} {1} Msg: {2}", GetMethodBase(), orderId, message));
    //    }

    //    /// <summary>
    //    /// Writes Info Information To Trace
    //    /// </summary>
    //    /// <param name="format"></param>
    //    /// <param name="args"></param>
    //    public static void WriteInfo(string format, params object[] args)
    //    {
    //        WriteInfo(string.Format(format, args));
    //    }

    //    public static void WriteDebug(string module, string orderId, string message)
    //    {
    //        __Log4Net.Debug(string.Format("{0} OrderId: {1} Msg: {2}", module, orderId, message));
    //    }

    //    /// <summary>
    //    /// Writes Debug Debugrmation To Trace
    //    /// </summary>
    //    /// <param name="message"></param>
    //    public static void WriteDebug(string message)
    //    {
    //        __Log4Net.Debug(string.Format("{0} Msg: {1}", GetMethodBase(), message));
    //        //__Log4Net.Debug(message);
    //    }
    //    /// <summary>
    //    /// Writes Debug Debugrmation To Trace
    //    /// </summary>
    //    /// <param name="orderId"></param>
    //    /// <param name="message"></param>
    //    public static void WriteDebug(string orderId, string message)
    //    {
    //        __Log4Net.Debug(string.Format("{0} {1} Msg: {2}", GetMethodBase(), orderId, message));
    //    }

    //    /// <summary>
    //    /// Writes Debug Debugrmation To Trace
    //    /// </summary>
    //    /// <param name="format"></param>
    //    /// <param name="args"></param>
    //    public static void WriteDebug(string format, params object[] args)
    //    {
    //        WriteDebug(string.Format(format, args));
    //    }
    //    #endregion

    //    #region Helpers
    //    /// <summary>
    //    /// Get Calling Method (From Another Assembly) Declaring Type
    //    /// </summary>
    //    /// <returns></returns>
    //    static Type __GetCallingMethodType()
    //    {
    //        StackTrace st = new StackTrace(false);
    //        for (int i=1; i<st.FrameCount; i++)
    //        {
    //            StackFrame sf = st.GetFrame(i);
    //            MethodBase mb = sf.GetMethod();

    //            if (mb.DeclaringType == typeof(LogWriter)) 
    //                continue;

    //            if (mb.DeclaringType.Assembly == typeof(LogWriter).Assembly) 
    //                continue;

    //            return mb.DeclaringType;
    //        }

    //        return null;
    //    }
    //    #endregion
    //}
}
