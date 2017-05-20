using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using log4net;
using System.Text;


namespace Pro.Server
{
 

    public static class Log4
    {

        static Log4()
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
            //logger = LogManager.GetLogger(typeof(Logger));
            //logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            logger = log4net.LogManager.GetLogger(System.Reflection.Assembly.GetCallingAssembly(), "Log");

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
            if (logger.IsDebugEnabled)
                logger.Debug(message);
        }
        public static void Info(string message)
        {
            if (logger.IsInfoEnabled) logger.Info(message);
        }
        public static void Warn(string message)
        {
            if (logger.IsWarnEnabled) logger.Warn(message);
        }
        public static void Error(string message)
        {
            if (logger.IsErrorEnabled) logger.Error(message);
        }
        public static void Fatal(string message)
        {
            if (logger.IsFatalEnabled) logger.Fatal(message);
        }

        public static void Error(string message, Exception ex)
        {
            if (logger.IsErrorEnabled) logger.Error(message, ex);
        }
        public static void Fatal(string message, Exception ex)
        {
            if (logger.IsFatalEnabled) logger.Fatal(message, ex);
        }


        public static void DebugFormat(string message, params object[] args)
        {
            if (logger.IsDebugEnabled)
                logger.DebugFormat(message, args);
        }
        public static void InfoFormat(string message, params object[] args)
        {
            if (logger.IsInfoEnabled) logger.InfoFormat(message, args);
        }
        public static void WarnFormat(string message, params object[] args)
        {
            if (logger.IsWarnEnabled) logger.WarnFormat(message, args);
        }
        public static void ErrorFormat(string message, params object[] args)
        {
            if (logger.IsErrorEnabled) logger.ErrorFormat(message, args);
        }
        public static void FatalFormat(string message, params object[] args)
        {
            if (logger.IsFatalEnabled) logger.FatalFormat(message, args);
        }


        public static void Exception(string message, Exception e)
        {
            Exception(message,e,false);
        }

        public static void Exception(string message, Exception e, bool addStackTrace)
        {
            if (!logger.IsErrorEnabled) return;

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


        public static string GetMethodBase()
        {
            System.Reflection.MethodBase methodBase = (System.Reflection.MethodBase)(new System.Diagnostics.StackTrace().GetFrame(2).GetMethod());
            return methodBase.DeclaringType.Name + "." + methodBase.Name;
        }
        public static string GetDeclaringMethod()
        {
            System.Reflection.MethodBase methodBase = (System.Reflection.MethodBase)(new System.Diagnostics.StackTrace().GetFrame(2).GetMethod());
            return methodBase.DeclaringType.Namespace + "." + methodBase.DeclaringType.Name + "." + methodBase.Name;
        }

    }

}
