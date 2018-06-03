using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Linq;

namespace Netcell.Web
{
    public static class HttpHelper
    {
        public static string CombineRequestRawUrlQuery(System.Web.HttpRequest Request,params string[] qs)
        {
            if (qs == null || qs.Length == 0 || qs.Length % 2 != 0)
            {
                throw new System.ArgumentException("Query string array is not correct");
            }
            StringBuilder sb = new StringBuilder();

            if (Request.QueryString.Count > 0)
            {

                for (int i = 0; i < qs.Length; i++)
                {
                    string key = qs[i];
                    string value = qs[i + 1];

                    if (!Request.QueryString.AllKeys.Contains(key))
                    {
                        sb.AppendFormat("&{0}={1}", key, value);
                    }
                    i++;
                }
            }
            else
            {
                for (int i = 0; i < qs.Length; i++)
                {
                    string key = qs[i];
                    string value = qs[i + 1];
                    i++;
                    sb.AppendFormat("&{0}={1}", key, value);
                }
                sb.Replace('&', '?', 0, 1);
            }

            return Request.RawUrl + sb.ToString();
        }

        public static string HttpPost(string uri, string parameters)
        {
            // parameters: name1=value1&name2=value2	
            WebRequest webRequest = WebRequest.Create(uri);

            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            byte[] bytes = Encoding.ASCII.GetBytes(parameters);
            Stream os = null;
            try
            { // send the Post
                webRequest.ContentLength = bytes.Length;   //Count bytes to send
                os = webRequest.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);         //Send it
            }
            catch (WebException ex)
            {
                //"HttpPost: Request error",
                throw new NetException(AckStatus.WebException , ex.InnerException);
            }
            finally
            {
                if (os != null)
                {
                    os.Close();
                }
            }

            try
            { // get the response
                WebResponse webResponse = webRequest.GetResponse();
                if (webResponse == null)
                { return null; }
                StreamReader sr = new StreamReader(webResponse.GetResponseStream());
                return sr.ReadToEnd().Trim();
            }
            catch (WebException ex)
            {
                //"HttpPost: Response error",
                throw new NetException(AckStatus.WebException,  ex.InnerException);
            }
        } // end HttpPost 

        private static Stream getResponseStream(WebRequest request)
        {
            //grab the respons stream
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream ResponseStream = response.GetResponseStream();


            //create the response buffer
            byte[] ResponseBuffer = new byte[response.ContentLength];
            int BytesLeft = ResponseBuffer.Length;
            int TotalBytesRead = 0;
            bool EOF = false;

            //loop while not EOF
            while (!EOF)
            {
                //get the next chunk and calc the remaining bytes
                int BytesRead = ResponseStream.Read(ResponseBuffer, TotalBytesRead, BytesLeft);
                TotalBytesRead += BytesRead;
                BytesLeft -= BytesRead;

                //has EOF been reached
                EOF = (BytesLeft == 0);
            }



            ResponseStream.Close();

            //create a memory stream and pass in the respones buffer
            ResponseStream = new MemoryStream(ResponseBuffer);
            return ResponseStream;

        }




        private static XmlDocument GetResponseXml(WebRequest request)
        {
            //load the stream into an xml document
            XmlDocument ResponseDocument = new XmlDocument();
            ResponseDocument.Load(getResponseStream(request));
            return ResponseDocument;

        }


        public static string PostRetString(string url)
        {
            Stream stream = MakeRequestGet(url);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public static string PostRetString(string url, XmlDocument document)
        {
            WebRequest request = MakeRequestPost(url, document);
            Stream stream = getResponseStream(request);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public static string PostSoapRetString(string url, XmlDocument document, string action)
        {
            WebRequest request = MakeRequestSoapPost(url, document, action);
            Stream stream = getResponseStream(request);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }




        private static Stream MakeRequestGet(string url)
        {
            WebRequest Request = WebRequest.Create(url);
            Request.Method = "GET";
            Request.ContentType = "application/x-www-form-urlencoded";

            // If required by the server, set the credentials.
            Request.Credentials = CredentialCache.DefaultCredentials;

            // Request.ContentLength = 
            HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
            Stream RequestStream = response.GetResponseStream();
            return RequestStream;
        }

        private static WebRequest MakeRequestSoapPost(string url, XmlDocument document,string action)
        {
            //convert the document to stream
            MemoryStream Stream = new MemoryStream();
            XmlWriter Writer = XmlWriter.Create(Stream);
            document.WriteContentTo(Writer);

            //reset the stream position
            Stream.Position = 0;

            //create the request and set the content type and length
            WebRequest Request = WebRequest.Create(url);

            //Request.Headers.Add("SOAPAction: http://tempuri.org/" + action);
            //Request.ProtocolVersion = HttpVersion.Version11;
            //Request.Credentials = CredentialCache.DefaultCredentials; 

            
            Request.Method = "POST";
            Request.ContentType = "application/soap+xml; charset=utf-8";
            Request.ContentLength = Stream.Length;
            
            //get the request stream and post the xml
            Stream RequestStream = Request.GetRequestStream();
            RequestStream.Write(Stream.GetBuffer(), 0, (int)Stream.Length);
            return Request;
        }

        private static WebRequest MakeRequestPost(string url, XmlDocument document)
        {
            //convert the document to stream
            MemoryStream Stream = new MemoryStream();
            XmlWriter Writer = XmlWriter.Create(Stream);
            document.WriteContentTo(Writer);

            //reset the stream position
            Stream.Position = 0;

            //create the request and set the content type and length
            WebRequest Request = WebRequest.Create(url);
            Request.Method = "POST";
            Request.ContentType = "text/xml";
            Request.ContentLength = Stream.Length;
            
            //get the request stream and post the xml
            Stream RequestStream = Request.GetRequestStream();
            RequestStream.Write(Stream.GetBuffer(), 0, (int)Stream.Length);
            return Request;
        }

        public static XmlDocument PostRetXml(string url, XmlDocument document)
        {
            WebRequest Request = MakeRequestPost(url, document);
            return GetResponseXml(Request);

        }


    }
}
