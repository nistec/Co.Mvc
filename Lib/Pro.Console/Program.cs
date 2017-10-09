using Nistec.Data.Entities;
using Nistec.Logging;
using Nistec.Serialization;
using Pro.Data;
using Pro.Lib.Api;
using Pro.Lib.Payments;
using Pro.Server.Agents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {

            string BrokerPw = AppSettingsContext.GetValue("TranzilaAutoCharge_BrokerPw");
            Console.Write(BrokerPw);

            //Nistec.Controller.Window(args);
            //return;

            Console.WriteLine("finished");
            Console.ReadLine();
        }

        static void Test(string[] args)
        {


            Nistec.Controller.Window(args);
            return;

            Console.WriteLine("start....");
            
            //Nistec.Controller.Run(args);

            //using (var db = DbContext.Create<DbPro>())
            //{
            //    var result = db.ExecuteReturnValue("sp_test", 0, "Id", 2);
            //    Console.WriteLine( result);
            //}

            //Netlog.DebugFormat("Pro.Console start:{0}", "test");
            //Logger.Instance.Debug("Pro.Console start: test");
            //if (Nistec.Generic.NetConfig.Get<bool>("ChargeAutoEnable"))
            //    PaymentBroker.RunAuto();
            //else
            //    PaymentBroker.Run();

            //var queueItem = PaymentApi.PaymentChargeGet();
            //if (queueItem != null && queueItem.IsValid)
            //{
            //     Console.Write("SchedulerWorker ExecuteAsync {0}", queueItem.QueueId);

            //    PaymentBroker.ChargeWithToken(queueItem);
            //}
            Console.Write("finished...");
            Console.ReadLine();
            
            return;
            PaymentAgent agent = new PaymentAgent();
            agent.Start();
            bool keeprun=true;
            while (keeprun)
            {
                string readed = Console.ReadLine();
                Console.WriteLine(readed);
                if (readed == "quit")
                {
                    break;
                }
            }
            agent.Stop();
            Console.WriteLine("finished...");
            return;

            string value = "Jan 06 2015";//"January 06, 2015";// "10/15/16";// "10/15/2016";// "2016-10-15";
            string format = "MMM dd yyyy";//"MMMM dd, yyyy";// "MM/dd/yy";// "MM/dd/yyyy";// "yyyy-MM-dd";

            var item = DbContext.EntityGet<DbPro, PaymentItem>("PayId", 17662);
            Console.Write(item);
            //PaymentBroker.ChargeWithToken(item);
            
            //DateTime dt = DateTime.ParseExact(value, format, System.Globalization.CultureInfo.InvariantCulture);


            //Console.WriteLine(dt.ToString());

            var SignKey1 = Nistec.Generic.UUID.NewUuid().ToString().Replace("-","");
            Console.WriteLine(SignKey1);

           
            var SignKey2 = Nistec.Generic.UUID.NewUuid().ToString().Replace("-", "");
            Console.WriteLine(SignKey2);

            File.AppendAllText(Environment.CurrentDirectory + "// guid.txt", SignKey1 + "   " + SignKey2);


           // ApiTest.Run();




            Console.WriteLine("finished");
            Console.ReadLine();
        }
    }
}
