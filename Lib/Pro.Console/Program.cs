using Nistec;
using Nistec.Data.Entities;
using Nistec.Logging;
using Nistec.Serialization;
using Pro.Data;
using Pro.Lib.Api;
using Pro.Lib.Payments;
using Pro.Lib.Payments.Server;
using Pro.Server.Agents;
//using Pro.Server.Agents;
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
            //Nistec.Control.Runtime();

            PaymentCommand command = new PaymentCommand();
            command.RunPaymentAgent();

            //var dt = Types.ParseDateTime("03/03/2018");
            //Console.WriteLine(dt);

            //dt = Types.ParseDateTime("03/03/2018 12:34:15.4587");
            //Console.WriteLine(dt);

            //dt = Types.ParseDateTime("2018-03-03");
            //Console.WriteLine(dt);

            //dt = Types.ParseDateTime("2018-03-03 12:34:15.4587");
            //Console.WriteLine(dt);

            //dt = Types.ParseDateTime("2018-03-03T12:34:15.4587");
            //Console.WriteLine(dt);

            //dt = Types.ParseDateTime("2018-03-03T12:34:15 pm");
            //Console.WriteLine(dt);

            //string BrokerPw = AppSettingsContext.GetValue("TranzilaAutoCharge_BrokerPw");
            //Console.Write(BrokerPw);

            //Nistec.Controller.Window(args);
            //return;

            Console.WriteLine("finished");
            Console.ReadLine();
        }

        public static int RemoveLeadingZero(string value)
        {
            //string str = Regx.RegexReplace("^0+(?!$)", value, "");
            string str = value.TrimStart('0');

            int val;
            int.TryParse(str, out val);
            return val;
        }
        public static DateTime ParseDateTime(object value, DateTime defaultValue)
        {

            if (value == null || value == DBNull.Value || value.ToString() == "")
                return defaultValue;
            string str = value.ToString();

            try
            {
                int arg1 = 0;
                int arg2 = 0;
                int arg3 = 0;

                int h = 0;
                int m = 0;
                int s = 0;


                string[] parts = str.Split(new string[] { " ", "T" }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length > 0)
                {
                    string[] args = parts[0].Split('/', '-');
                    arg1 = RemoveLeadingZero(args[0]);
                    arg2 = RemoveLeadingZero(args[1]);
                    arg3 = RemoveLeadingZero(args[2]);
                }

                if (parts.Length > 1)
                {

                    string[] args = parts[1].Split(':', '.');

                    if (args.Length > 0)
                        h = RemoveLeadingZero(args[0]);
                    if (args.Length > 1)
                        m = RemoveLeadingZero(args[1]);
                    if (args.Length > 2)
                        s = RemoveLeadingZero(args[2]);
                }

                if (parts.Length > 2)
                {

                    //[AM|PM|am|pm]
                    switch (parts[2])
                    {
                        case "PM":
                        case "pm":
                            if (h < 12)
                                h = h + 12;
                            break;
                    }
                }

                if (str.IndexOf('-') > 0)
                    return new DateTime(arg1, arg2, arg3, h, m, s);
                else
                    return new DateTime(arg3, arg2, arg1, h, m, s);
            }
            catch
            {
                return defaultValue;
            }
        }
        public static DateTime? ParseDateTime(object value)
        {

            if (value == null || value == DBNull.Value || value.ToString() == "")
                return (DateTime?)null;
            string str = value.ToString();

            try
            {
                int arg1 = 0;
                int arg2 = 0;
                int arg3 = 0;

                int h = 0;
                int m = 0;
                int s = 0;


                string[] parts = str.Split(new string[] { " ", "T" }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length > 0)
                {
                    string[] args = parts[0].Split('/', '-');
                    arg1 = RemoveLeadingZero(args[0]);
                    arg2 = RemoveLeadingZero(args[1]);
                    arg3 = RemoveLeadingZero(args[2]);
                }

                if (parts.Length > 1)
                {

                    string[] args = parts[1].Split(':', '.');

                    if (args.Length > 0)
                        h = RemoveLeadingZero(args[0]);
                    if (args.Length > 1)
                        m = RemoveLeadingZero(args[1]);
                    if (args.Length > 2)
                        s = RemoveLeadingZero(args[2]);
                }

                if (parts.Length > 2)
                {

                    //[AM|PM|am|pm]
                    switch (parts[2])
                    {
                        case "PM":
                        case "pm":
                            if (h < 12)
                                h = h + 12;
                            break;
                    }
                }

                if (str.IndexOf('-') > 0)
                    return new DateTime(arg1, arg2, arg3, h, m, s);
                else
                    return new DateTime(arg3, arg2, arg1, h, m, s);
            }
            catch
            {
                return null;
            }
        }
        //public static DateTime? ParseDateTime(object value)
        //{

        //    if (value == null || value == DBNull.Value || value.ToString() == "")
        //        return (DateTime?)null;
        //    string str = value.ToString();

        //    try
        //    {
        //        int arg1 = 0;
        //        int arg2 = 0;
        //        int arg3 = 0;

        //        int h = 0;
        //        int m = 0;
        //        int s = 0;


        //        string[] parts= str.Split(new string[] {" ", "T" }, StringSplitOptions.RemoveEmptyEntries);

        //        if(parts.Length>0)
        //        {
        //            string[] args = parts[0].Split('/', '-');
        //            arg1 = Convert.ToInt32(args[0]);
        //            arg2 = Convert.ToInt32(args[1]);
        //            arg3 = Convert.ToInt32(args[2]);
        //        }

        //        if (parts.Length > 1)
        //        {

        //            string[] args = parts[1].Split(':', '.');

        //            if (args.Length > 0)
        //                h = Convert.ToInt32(args[0]);
        //            if (args.Length > 1)
        //                m = Convert.ToInt32(args[1]);
        //            if (args.Length > 2)
        //                s = Convert.ToInt32(args[2]);
        //        }

        //        if (parts.Length > 2)
        //        {

        //            //[AM|PM|am|pm]
        //            switch (parts[2])
        //            {
        //                case "PM":
        //                case "pm":
        //                    if (h < 12)
        //                        h = h + 12;
        //                    break;
        //            }
        //        }


        //        //    string[] args = str.Split(new string[] { "/", "-", ":", ".", "T" }, StringSplitOptions.RemoveEmptyEntries);
        //        //int arg1 = Convert.ToInt32(args[0]);
        //        //int arg2 = Convert.ToInt32(args[1]);
        //        //int arg3 = Convert.ToInt32(args[2]);

        //        //int h = 0;
        //        //int m = 0;
        //        //int s = 0;

        //        //if (args.Length > 3)
        //        //    h = Convert.ToInt32(args[3]);
        //        //if (args.Length > 4)
        //        //    m = Convert.ToInt32(args[4]);
        //        //if (args.Length > 5)
        //        //    s = Convert.ToInt32(args[5]);

        //        if(str.IndexOf('-')>0)
        //            return new DateTime(arg1, arg2, arg3, h, m, s);
        //        else
        //            return new DateTime(arg3, arg2, arg1, h, m, s);
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

     
        static void Test(string[] args)
        {


            Nistec.Controller.Window(args);
            return;

            //Console.WriteLine("start....");

            //Nistec.Controller.Run(args);

            //using (var db = DbContext.Create<DbPro>())
            //{
            //    var result = db.ExecuteReturnValue("sp_test", 0, "Id", 2);
            //    Console.WriteLine(result);
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
            //    Console.Write("SchedulerWorker ExecuteAsync {0}", queueItem.QueueId);

            //    PaymentBroker.ChargeWithToken(queueItem);
            //}
            //Console.Write("finished...");
            //Console.ReadLine();
            
            return;

            //PaymentAgent agent = new PaymentAgent();
            //agent.Start();
            //bool keeprun = true;
            //while (keeprun)
            //{
            //    string readed = Console.ReadLine();
            //    Console.WriteLine(readed);
            //    if (readed == "quit")
            //    {
            //        break;
            //    }
            //}
            //agent.Stop();
            //Console.WriteLine("finished...");
            //return;

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
