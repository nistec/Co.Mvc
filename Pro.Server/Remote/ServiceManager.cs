
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Configuration;
using Nistec;
using Nistec.Threading;
using Nistec.Collections;
using Nistec.Logging;
using System.Collections;
using System.Collections.Concurrent;

namespace Pro.Server
{

    public class ServiceManager
    {
 
        private int isRunning = 0;
 

        private bool keepAlive = false;
        private GenericThreadPool threadPool;
        private object syncPending = new object();
        private object syncQueue = new object();
        private bool EnableAdminCommand = true;

        static readonly ConcurrentQueue<SchedulerCommand> queue = new ConcurrentQueue<SchedulerCommand>();

        public ServiceManager()
        {
            EnableAdminCommand = ConfigSrv.Enable_Scheduler_Commands;
        }

        public static ServiceManager Instance
        {
            get { return new ServiceManager(); }
        }

        public void Start()
        {
            if (keepAlive)
                return;

            Netlog.Debug("Start AdminManager");

            keepAlive = true;
            threadPool = new GenericThreadPool(ConfigSrv.AdminMaxThread);
            threadPool.StartThreadPool(new ParameterizedThreadStart(CommandProcess));
            
        }

        private void CommandProcess(object obj)
        {
            int threadSleep = ConfigSrv.AdminIntervalSetting;
            while (keepAlive)
            {

                while (!EnableAdminCommand)
                {
                    Thread.Sleep(120000);
                }
                if (!keepAlive)
                {
                    break;
                }
                //Console.WriteLine("PengingRunning: " + isRunning.ToString());
                //Netcell.Log.Debug("CommandProcess sync");

                lock (syncPending)
                {
                    try
                    {
                        if (0 == Interlocked.Exchange(ref isRunning, 1))
                        {
                            //Console.WriteLine("Penging load...{0}",Thread.CurrentThread.Name);
                            Interlocked.Increment(ref isRunning);
                            while(true)
                            {
                                SchedulerCommand activeCommand = new SchedulerCommand();
                                if (activeCommand.IsEmpty)
                                {
                                    activeCommand.Dispose();
                                    break;
                                }
                                activeCommand.ExecuteCommandsAsync();
                                activeCommand.Dispose();
                                //queue.Enqueue(activeCommand);
                                Thread.Sleep(100);
                            }
                        }
                        else
                        {
                            //Console.WriteLine("Penging locked...{0}", Thread.CurrentThread.Name); 
                        }
                    }
                    catch (Exception ex)
                    {
                        //Netcell.Log.ErrorFormat("CommandProcess error: {0}", ex.Message);
                        new ServerException("CommandProcess error ", "ServiceManager",ex);
                    }
                    finally
                    {
                        Interlocked.Decrement(ref isRunning);
                    }
                }
                //Console.WriteLine("Penging keep Alive...{0}..{1}", isRunning,Thread.CurrentThread.Name);
                Thread.Sleep(threadSleep);
            }
        }

 
        private void QueueListner()
        {
            Console.WriteLine("AdminManager QueueListner...");
            Netlog.Info("QueueListner started...");

            Thread.Sleep(1000);

            while (keepAlive)
            {
                SchedulerCommand ac = null;
                try
                {
                    if (!queue.IsEmpty)
                    {
                        if (queue.TryDequeue(out ac))
                        {
                            if (ac != null && !ac.IsEmpty)
                            {
                                ac.ExecuteCommands();
                            }
                        }
                        //ac = queue.Dequeue();
                    }

                    //lock (((ICollection)queue).SyncRoot)//syncQueue)
                    //{
                    //    if (queue.Count > 0)
                    //    {
                    //        ac = queue.Dequeue();
                    //    }
                    //}

                    //if (ac != null && !ac.IsEmpty)
                    //{
                    //    ac.ExecuteCommands();
                    //}
                }
                catch (Exception ex)
                {
                    new ServerException("QueueListner error", "ServiceManager", ex);
                }

                Thread.Sleep(1000);
            }
        }

  
 
        public void Stop()
        {
            Netlog.Debug("Stop AdminManager");
            keepAlive = false;
            int count = 0;
            while (isRunning > 0 && count < 20)
            {
                Thread.Sleep(100);
                count++;
            }
            //if (threadQueue != null)
            //{
            //    threadQueue.StopThreadPool(); ;
            //    threadQueue.Dispose();
            //}
            if (threadPool != null)
            {
                threadPool.StopThreadPool(); ;
                threadPool.Dispose();
            }
        }

  
    }
}
