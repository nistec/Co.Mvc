
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading;
using System.Configuration;
using System.Data;
using Nistec.Threading;
using System.Security;
using Pro.Lib.Api;
using System.Threading.Tasks;

namespace Pro.Server.Agents
{

    public class PaymentAgent
    {

        private int isSchedulerRunning = 0;
        private bool keepAlive =false;
        private GenericThreadPool threadPool;
        static object m_lock = new object();
        int MaxConnection = 30;
        int m_Connections;
        int Interval = 30000;
        int Server = 0;
        int SchedulerMode = 0;//0=single,1=bulk,2= vari 
        int CurrentSchedulerMode = 0;//0=single,1=bulk,2= vari 
        public PaymentAgent()
        {
            MaxConnection = AgentsConfig.SchedulerMaxConnection;
            Interval = AgentsConfig.SchedulerInterval;
            Server = 0;// ViewConfig.Server;
            SchedulerMode = AgentsConfig.SchedulerMode;
        }
        int GetCurrentMode()
        {
            if (0 == Interlocked.Exchange(ref CurrentSchedulerMode, 1))
            {
                Interlocked.Exchange(ref CurrentSchedulerMode, 1);
                return 1;
            }
            else
            {
                Interlocked.Exchange(ref CurrentSchedulerMode, 0);
                return 0;
            }
        }
        public static PaymentAgent Instance
        {
            get { return new PaymentAgent(); }
        }

        public void Start()
        {
            if (keepAlive)
                return;

            Log4.Debug("Start Scheduler");
            keepAlive=true;
            threadPool = new GenericThreadPool(/*"Watch",*/  AgentsConfig.SchedulerMaxThread);

            threadPool.StartThreadPool(new ParameterizedThreadStart(SchedulerProcess));//new ThreadStart(SchedulerProcess));

            //if (SchedulerMode == 2)//vari
            //    threadPool.StartThreadPool(new ThreadStart(SchedulerProcessVari));
            //else if (SchedulerMode == 1)//bulk
            //    threadPool.StartThreadPool(new ThreadStart(SchedulerProcessBulk));
            //else // single
            //    threadPool.StartThreadPool(new ThreadStart(SchedulerProcess));
        }

        #region vari
        /*
        private void SchedulerProcessVari()
        {

            while (keepAlive)
            {
                //Scheduler_Queue activeScheduler = null;

                List<Scheduler_Queue> queueItems = null;

                try
                {

                    while (!(keepAlive && App_Servers.IsEnableScheduler(server)))
                    {
                        Thread.Sleep(Interval);
                    }

                    while (Thread.VolatileRead(ref m_Connections) >= MaxConnection)
                    {
                        Thread.Sleep(100);
                    }

                    if (!keepAlive)
                    {
                        break;
                    }


                    //Console.WriteLine("Scheduler load...{0}",Thread.CurrentThread.Name);

                    Interlocked.Increment(ref isSchedulerRunning);

                    lock (m_lock)
                    {
                        int bulkMode = GetCurrentMode();

                        queueItems = Scheduler_Queue_Context.DequeueBulk(bulkMode, Server);
                        //Log.WarnFormat("SchedulerProcess Dequeue");

                        if (queueItems != null && queueItems.Count > 0)
                        {
                            Interlocked.Increment(ref m_Connections);

                            foreach (var queueItem in queueItems)
                            {
                                SchedulerItem(queueItem);
                            }
                        }

                    }
                }
                catch (ThreadAbortException)
                {
                    Log.WarnFormat("SchedulerProcess bulk ThreadAborted");
                }
                catch (Exception ex)
                {
                    MsgException.Trace(AckStatus.FatalSchedulerException, "Scheduler bulk error:{0} ", ex.Message + " Trace:" + ex.StackTrace);
                    //TODO:check this.
                    //try
                    //{
                    //    lock (typeof(Scheduler_Queue_Context))
                    //    {
                    //        Scheduler_Queue_Context.DeCompletedBulk(queueItem., (int)SchedulerState.Error);
                    //    }

                    //}
                    //catch (Exception exx)
                    //{
                    //    TraceException.Trace(TraceStatus.FatalSchedulerException, "Error: Scheduler error:{0}", exx.Message);
                    //}
                }
                finally
                {
                    Interlocked.Decrement(ref isSchedulerRunning);
                }
                Thread.Sleep(Interval);
            }
            Log.WarnFormat("Scheduler bulk not keep Alive");
        }
        */

        #endregion

        #region bulk
        /*
        private void SchedulerProcessBulk()
        {
            int server = ViewConfig.Server;

            while (keepAlive)
            {
                //Scheduler_Queue activeScheduler = null;

                List<Scheduler_Queue> queueItems = null;

                try
                {

                    while (!(keepAlive && App_Servers.IsEnableScheduler(server)))
                    {
                        Thread.Sleep(Interval);
                    }

                    while (Thread.VolatileRead(ref m_Connections) >= MaxConnection)
                    {
                        Thread.Sleep(100);
                    }

                    if (!keepAlive)
                    {
                        break;
                    }

                    Interlocked.Increment(ref isSchedulerRunning);

                    lock (m_lock)
                    {
                        queueItems = Scheduler_Queue_Context.DequeueBulk(1, Server);
                        //Log.WarnFormat("SchedulerProcess Dequeue");

                        if (queueItems != null && queueItems.Count > 0)
                        {
                            Interlocked.Increment(ref m_Connections);

                            foreach (var queueItem in queueItems)
                            {
                                SchedulerItem(queueItem);
                                Thread.Sleep(100);
                            }
                        }

                    }
                }
                catch (ThreadAbortException)
                {
                    Log.WarnFormat("SchedulerProcess bulk ThreadAborted");
                }
                catch (Exception ex)
                {
                    MsgException.Trace(AckStatus.FatalSchedulerException, "Scheduler bulk error:{0} ", ex.Message + " Trace:" + ex.StackTrace);
                    //TODO:check this.
                    //try
                    //{
                    //    lock (typeof(Scheduler_Queue_Context))
                    //    {
                    //        Scheduler_Queue_Context.DeCompletedBulk(queueItem., (int)SchedulerState.Error);
                    //    }

                    //}
                    //catch (Exception exx)
                    //{
                    //    TraceException.Trace(TraceStatus.FatalSchedulerException, "Error: Scheduler error:{0}", exx.Message);
                    //}
                }
                finally
                {
                    Interlocked.Decrement(ref isSchedulerRunning);
                }
                Thread.Sleep(Interval);
            }
            Log.WarnFormat("Scheduler bulk not keep Alive");
        }

        private void SchedulerItem(Scheduler_Queue queueItem)
        {
            int server = ViewConfig.Server;

            try
            {

                if (queueItem != null && !queueItem.IsEmpty)
                {
                    if (queueItem.SchedulerState == SchedulerState.Expired)
                    {
                        //TODO ADD ALERT
                        MsgException.Trace(AckStatus.FatalSchedulerException, "SchedulerState item is Expired: " + queueItem.Print());
                    }
                    else
                    {

                        ThreadPool.QueueUserWorkItem(SchedulerWorker, queueItem);
                    }
                }

            }
            catch (ThreadAbortException)
            {
                Log.WarnFormat("Scheduler item  ThreadAborted");
            }
            catch (Exception ex)
            {
                MsgException.Trace(AckStatus.FatalSchedulerException, "Scheduler item error:{0} ", ex.Message + " Trace:" + ex.StackTrace);

                try
                {
                    lock (typeof(Scheduler_Queue_Context))
                    {
                        Scheduler_Queue_Context.DeCompleted(queueItem.QueueId, (int)SchedulerState.Error);
                    }
                }
                catch (Exception exx)
                {
                    MsgException.Trace(AckStatus.FatalSchedulerException, "Error : Scheduler bulk item DeCompleted error:{0}", exx.Message);
                }
            }
        }
        */
        #endregion

        #region single
        private void SchedulerProcess(object state)
        {

            while (keepAlive)
            {
                 //Scheduler_Queue activeScheduler = null;

                 PaymentChargeItem queueItem = null;

                 try
                 {

                     //while (!(keepAlive && App_Servers.IsEnableScheduler(server)))
                     //{
                     //    Thread.Sleep(Interval);
                     //}

                     while (Thread.VolatileRead(ref m_Connections) >= MaxConnection)
                     {
                         Thread.Sleep(100);
                     }

                     if (!keepAlive)
                     {
                         break;
                     }


                     //Console.WriteLine("Scheduler load...{0}",Thread.CurrentThread.Name);

                     Interlocked.Increment(ref isSchedulerRunning);

                     lock (m_lock)
                     {
                         queueItem = PaymentApi.PaymentChargeGet();
                         //Log.WarnFormat("SchedulerProcess Dequeue");

                         if (queueItem != null && queueItem.IsValid)
                         {
                             if (queueItem.IsExpired)
                             {
                                 //TODO ADD ALERT
                                 Log4.ErrorFormat("SchedulerState is Expired: " + queueItem.QueueId);
                             }
                             else
                             {
                                 Interlocked.Increment(ref m_Connections);
                                 Task.Factory.StartNew(() => SchedulerWorker(queueItem));
                             }

                          }
                     }
                 }
                 catch (ThreadAbortException)
                 {
                     Log4.WarnFormat("SchedulerProcess ThreadAborted");
                 }
                 catch (Exception ex)
                 {
                      Log4.ErrorFormat("Scheduler error:{0} ", ex.Message + " Trace:" + ex.StackTrace);

                     //try
                     //{
                     //    //using (DalController dal = new DalController())
                     //    //{
                     //    //    dal.Scheduler_DeCompleted(activeScheduler.QueueId, (int)SchedulerState.Error);
                     //    //}

                     //    lock (typeof(Scheduler_Queue_Context))
                     //    {
                     //        Scheduler_Queue_Context.DeCompleted(queueItem.QueueId, (int)SchedulerState.Error);
                     //    }

                     //}
                     //catch (Exception exx)
                     //{
                     //    MsgException.Trace(AckStatus.FatalSchedulerException, "Error: Scheduler error:{0}", exx.Message);
                     //}
                 }
                finally
                {
                    Interlocked.Decrement(ref isSchedulerRunning);
                }
                Thread.Sleep(Interval);
            }
            Log4.WarnFormat("Scheduler not keep Alive");
        }

        void SchedulerWorker(PaymentChargeItem queueItem)
        {
            try
            {
                if (queueItem != null && queueItem.IsValid)
                {
                    Log4.DebugFormat("SchedulerWorker ExecuteAsync {0}", queueItem.QueueId);

                    PaymentBroker.ChargeWithToken(queueItem);

                    //using (ActiveScheduler sch = new ActiveScheduler(queueItem))
                    //{
                    //    sch.ExecuteAsync();
                    //}

                    //lock (typeof(Scheduler_Queue_Context))
                    //{
                    //    Scheduler_Queue_Context.DeCompleted(queueItem.QueueId);
                    //}
                    //activeCampaign.Dispose();
                }
            }
            catch (ThreadAbortException)
            {
                Log4.WarnFormat("SchedulerWorker ThreadAborted");
            }
            catch (Exception ex)
            {
                Log4.ErrorFormat("SchedulerWorker error:{0} ", ex.Message);
            }

            Interlocked.Decrement(ref m_Connections);
        }

        #endregion

        public void Stop()
        {
            Log4.Debug("Stop Scheduler");
            try
            {
                 
                keepAlive = false;
                int count = 0;
                while (Thread.VolatileRead(ref isSchedulerRunning) > 0 && count < 30)
                {
                    Thread.Sleep(1000);
                    count++;
                    //Log.Debug("SchedulerStop:" + count.ToString());
                }
                
                if (threadPool != null)
                {
                    try
                    {
                        threadPool.StopThreadPool(); ;
                        threadPool.Dispose();
                    }
                    catch (SecurityException)
                    {
                        //e = e;
                    }
                    catch (ThreadStateException)
                    {
                        //ex = ex;
                        // In case the thread has been terminated 
                        // after the check if it is alive.
                    }
                }
            }
            catch (Exception ex)
            {
                Log4.ErrorFormat("Stop Scheduler error:{0}", ex.Message);
            }
        }
    }
}

