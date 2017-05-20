using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ConsoleTest
{
    public class ApiTest
    {
        public static void Run()
        {


            RunFormPost("http://localhost:25808", "/api/credit/notify");

        }

        static void RunFormPost(string url, string requestUrl)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                var content = new FormUrlEncodedContent(new[] 
               {
                new KeyValuePair<string, string>("Response" , "000"),
                new KeyValuePair<string, string>("o_tranmode" , "AK"),
                new KeyValuePair<string, string>("trid" , "50"),
                new KeyValuePair<string, string>("trBgColor" , ""),
                new KeyValuePair<string, string>("expmonth" , "10"),
                new KeyValuePair<string, string>("contact" , "nissim"),
                new KeyValuePair<string, string>("myid" , "054649967"),
                new KeyValuePair<string, string>("email" , "nissim%40myt.com"),
                new KeyValuePair<string, string>("currency" , "1"),
                new KeyValuePair<string, string>("nologo" , "1"),
                new KeyValuePair<string, string>("expyear" , "17"),
                new KeyValuePair<string, string>("supplier" , "baityehudi"),
                new KeyValuePair<string, string>("sum" , "1.00"),
                new KeyValuePair<string, string>("benid" , "5pb423r0odqe2kcvo40ku1bvm7"),
                new KeyValuePair<string, string>("o_cred_type" , ""),
                new KeyValuePair<string, string>("lang" , "il"),
                new KeyValuePair<string, string>("phone" , "0527464292"),
                new KeyValuePair<string, string>("o_npay" , ""),
                new KeyValuePair<string, string>("tranmode" , "AK"),
                new KeyValuePair<string, string>("ConfirmationCode" , "0000000"),
                new KeyValuePair<string, string>("cardtype" , "2"),
                new KeyValuePair<string, string>("cardissuer" , "6"),
                new KeyValuePair<string, string>("cardaquirer" , "6"),
                new KeyValuePair<string, string>("index" , "5"),
                new KeyValuePair<string, string>("Tempref" , "02720001"),
                new KeyValuePair<string, string>("TranzilaTK" , "W2e44ed3a9737dc2322"),
                new KeyValuePair<string, string>("ccno" , "")
              });

                string request= content.ReadAsStringAsync().Result;

                var result = client.PostAsync(requestUrl, content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Console.WriteLine(resultContent);
            }
        }

        static async void RunFormPost(string url, string key, string xml)
        {

            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:33662");
                var pairs = new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>(key, xml)
                };

                var content = new FormUrlEncodedContent(pairs);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                var result = client.PostAsync(url, content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Console.WriteLine(resultContent);
            }
        }

    }
}
