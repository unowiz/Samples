using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Samples.SharePoint.ConsoleRestApi {

    public static class Program {

        private static readonly string TokenUrl = "https://login.microsoftonline.com/common/oauth2/token";

        private static readonly string ClientId = "<clientid>";

        private static readonly string UserName = "<username>";

        private static readonly string Password = "<password>";

        private static readonly string SharePointUrl = "https://<tenantname>.sharepoint.com";

        private static void Main(string[] args) {
            var accessToken = GetAccessToken();
            if (accessToken != null) {
                GetList(accessToken);
            }
            Console.ReadLine();
        }

        private static string GetAccessToken() {
            try {
                var requestBody =
                    "grant_type=password"
                    + "&" + "username=" + Uri.EscapeDataString(UserName)
                    + "&" + "password=" + Uri.EscapeDataString(Password)
                    + "&" + "resource=" + Uri.EscapeDataString(SharePointUrl)
                    + "&" + "client_id=" + Uri.EscapeDataString(ClientId);
                Console.WriteLine(requestBody);
                var requestBuffer = Encoding.UTF8.GetBytes(requestBody);
                var webRequest = WebRequest.Create(TokenUrl);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.GetRequestStream().Write(requestBuffer, 0, requestBuffer.Length);
                var webResponse = webRequest.GetResponse();
                var responseBody = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
                Console.WriteLine(responseBody);
                var serializer = new JsonSerializer();
                var json = (Dictionary<string, string>)serializer.Deserialize(new StringReader(responseBody), typeof(Dictionary<string, string>));
                return json["access_token"];
            } catch (WebException ex) {
                var webResponse = ex.Response;
                var responseBody = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
                Console.WriteLine(responseBody);
                return null;
            }
        }

        private static void GetList(string accessToken) {
            try {
                var webRequest = WebRequest.Create(SharePointUrl + "/_api/web/lists");
                webRequest.Method = "GET";
                webRequest.Headers.Add("Authorization", "Bearer " + accessToken);
                webRequest.ContentType = "application/json;odata=verbose";
                var webResponse = webRequest.GetResponse();
                var responseBody = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
                Console.WriteLine(responseBody);
            } catch (WebException ex) {
                var webResponse = ex.Response;
                var responseBody = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
                Console.WriteLine(responseBody);
            }
        }

    }

}
