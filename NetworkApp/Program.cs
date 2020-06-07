using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NetworkApp
{
class Program
    {
        static void Main(string[] args)
        {
            // we need URI
            //string myURI = "https://www.opiframe.com";
            string myURI = "https://www.voidviewer.com/";
            //string myURI = "https://www.voidviewer.com/css/default.css";

            // we need WebClient class object
            // this handles connection to URI
            /*
            WebRequest myRequest = WebRequest.Create(myURI);
            WebResponse myResponse = myRequest.GetResponse();
            Stream stream = myResponse.GetResponseStream();
            StreamReader readerA = new StreamReader(stream);
            string data = readerA.ReadToEnd();
            Console.WriteLine(data);
            Console.ReadLine();
            */

            // 1) Hae kotisivusi sisältö
            //    käytä URIBuilder luokkaa URIn tekemiseen
            // 2) hae kotisivun header

            // Haetaan JSON viesti

            //TestPOST();
            //TestURIBuilder();
            //TestHttpRequest();
            //var vJSON = YleAPIAsync();
            //Console.WriteLine(vJSON.Result.data);
            TestHttpRequestB();
            Console.ReadLine();
        }

        public static void TestHttpRequestB()
        {
            Console.WriteLine("---- TestHttpRequestB:");
            HttpWebRequest HttpWReq = (HttpWebRequest)WebRequest.Create("https://voidviewer.com");
            // Turn off connection keep-alives.  
            HttpWReq.KeepAlive = false;

            HttpWebResponse HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();

            // Get the HTTP protocol version number returned by the server.  
            String ver = HttpWResp.ProtocolVersion.ToString();
            Console.WriteLine(ver);
            HttpWResp.Close();

            // ---------------------------------------
            Console.WriteLine("---- TestHttpRequestB, group connection:");
            // Create a connection group name.  
            SHA1Managed Sha1 = new SHA1Managed();
            Byte[] updHash = Sha1.ComputeHash(Encoding.UTF8.GetBytes("kukku" + "kukku" + "kukku"));
            String secureGroupName = Encoding.Default.GetString(updHash);

            // Create a request for a specific URL.  
            WebRequest myWebRequest = WebRequest.Create("https://voidviewer.com");

            myWebRequest.Credentials = new NetworkCredential("kukku", "kukku", "kukku");
            myWebRequest.ConnectionGroupName = secureGroupName;

            WebResponse myWebResponse = myWebRequest.GetResponse();

            // Insert the code that uses myWebResponse.  

            myWebResponse.Close();
        }

        public static void TestPOST()
        {
            Console.WriteLine("---- TestPOST:");
            // Create a request using a URL that can receive a post.
            WebRequest request = WebRequest.Create("http://www.contoso.com/PostAccepter.aspx ");
            // Set the Method property of the request to POST.
            request.Method = "POST";

            // Create POST data and convert it to a byte array.
            string postData = "This is a test that posts this string to a Web server.";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData); // <--- encoding

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.Write(((HttpWebResponse)response).StatusDescription);

            // Close the response.
            response.Close();
            Console.ReadLine();
        }

        public static void TestURIBuilder()
        {
            Console.WriteLine("---- TestURIBuilder:");
            var uriBuilder = new UriBuilder
            {
                Scheme = "https",
                Host = "voidviewer.com",
                Path = "/"
            };

            Uri uri = uriBuilder.Uri;

            WebRequest request = WebRequest.Create(uri);
            //using WebResponse response = request.GetResponse();
            WebResponse response = request.GetResponse();

            var content = response;
            Console.WriteLine(content);
            var headers = response.Headers;
            Console.WriteLine(headers);
            var allKeys = response.Headers.AllKeys;

            foreach (string iter in allKeys)
            {
                Console.WriteLine(iter);
            }

            Console.ReadLine();
        }

        public static void TestHttpRequest()
        {
            Console.WriteLine("---- TestHttpRequest:");
            var uri = new Uri("https://voidviewer.com");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            var response = (HttpWebResponse)request.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);

            var data = reader.ReadToEnd();
            Console.WriteLine(data);

            Console.ReadLine();
        }

        public static async Task<dynamic> YleAPIAsync()
        {
            // app_id=2ce46bd7&app_key=cfdee75058d9f24f5a48a92098ae76d2
            // secret key: efcbfa6021fed7bc
            //var endPoint = "https://external.api.yle.fi/v1/programs/items.json?app_id=2ce46bd7&app_key=cfdee75058d9f24f5a48a92098ae76d2";
            //var endPointB = "https://external.api.yle.fi/v1/programs/categories.json?app_id=2ce46bd7&app_key=cfdee75058d9f24f5a48a92098ae76d2";
            //var endPointC = "https://external.api.yle.fi/v1/programs/items.json?app_id=2ce46bd7&app_key=cfdee75058d9f24f5a48a92098ae76d2";
            var endPointD = "https://external.api.yle.fi/v1/series/items/1-2523689.json?app_id=2ce46bd7&app_key=cfdee75058d9f24f5a48a92098ae76d2";

            /*
            using (StreamReader r = new StreamReader(endPointD))
            {
                string json = r.ReadToEnd();
                dynamic items = JsonConvert.DeserializeObject(json);
                foreach (var item in items)
                {
                    //Console.WriteLine("{0} {1}", item.description);
                }
                Console.WriteLine(items);
            }
            */

            //dynamic array = JsonConvert.DeserializeObject(json);

            string url = endPointD;

            HttpClient client = new HttpClient();

            string response = await client.GetStringAsync(url);

            Console.WriteLine("AWAIT");
            dynamic data = JsonConvert.DeserializeObject(response);
            return data;
        }
    }
}