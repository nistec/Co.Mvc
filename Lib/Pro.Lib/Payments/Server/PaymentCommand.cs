using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pro.Lib.Payments.Server
{
    public class PaymentCommand
    {
        public void RunPaymentAgent()
        {
            Console.WriteLine("Start Payment charge...");
            Console.WriteLine("Run auto? y/n");
            string ack= Console.ReadLine();

            PaymentAgent agent = new PaymentAgent();
            if (ack == "y")
            {
                agent.Start();
                bool keeprun = true;
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
            }
            else
            {
                agent.SingleProcess();
                while (true)
                {
                    Console.WriteLine("Run more? y/n");
                    string domore = Console.ReadLine();
                    if (domore == "y")
                        agent.SingleProcess();
                    else
                        break;
                }
            }
            Console.WriteLine("finished...");
        }
    }
}
