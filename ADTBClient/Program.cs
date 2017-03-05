using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace ADTBClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseaddress = "http://localhost:59092";
            Token nToken = null;

            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "username", "rveluth" },
                    { "password", "Secure#123456789" }
                };

                var tokenresponse = client.PostAsync(baseaddress + "/token", new FormUrlEncodedContent(form)).Result;
                var token = tokenresponse.Content.ReadAsStreamAsync().Result;
                StreamReader sReader = new StreamReader(token);

                string s = sReader.ReadToEnd();
                sReader.Close();
                /*JavaScriptSerializer jSerializer = new JavaScriptSerializer();
                Dictionary<string, object> returnData = (Dictionary<string,object>)jSerializer.DeserializeObject(s);

                Token gToken = new Token
                {
                    AccessToken = returnData["access_token"].ToString(),
                    TokenType = returnData["token_type"].ToString(),
                    ExpiresIn = int.Parse(returnData["expires_in"].ToString())
                };

                Console.Write("{0},\n {1},\n, {2},\n", gToken.AccessToken, gToken.TokenType, gToken.ExpiresIn);*/

                nToken = JsonConvert.DeserializeObject<Token>(s);


                //.ReadAsAsync<Token>(
                //new[] { new JsonMediaTypeFormatter() }
                //).Result;

                //if (string.IsNullOrEmpty(token.Error))
                //    Console.WriteLine("Token issued is: {0}", token.AccessToken);
                //else
                //    Console.WriteLine("Error: {0}", token.Error);
            }

            using (HttpClient nClient = new HttpClient())
            {
                nClient.BaseAddress = new Uri(baseaddress);
                nClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", nToken.access_token);
                HttpResponseMessage response = nClient.GetAsync("/IsTokenAuthorized").Result;
                if (response.IsSuccessStatusCode)
                {
                    //Console.WriteLine("Success");
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                }
             }

            Console.ReadLine();
        }
    }
}
