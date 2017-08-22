using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Properties;

namespace WebApplication1.Controllers {

    public class ClientCredentialsController : Controller {

        public async Task<ActionResult> GetTokenV1() {
            var requestUri = "https://login.microsoftonline.com/" + Settings.Default.TenantName + "/oauth2/token";
            var clientId = Settings.Default.ClientId;
            var resource = Settings.Default.ResourceUri;
            var clientSecret = Settings.Default.ClientSecret;
            var requestContent =
                "client_id=" + Uri.EscapeDataString(clientId) + "&" +
                "resource=" + Uri.EscapeDataString(resource) + "&" +
                "client_secret=" + Uri.EscapeDataString(clientSecret) + "&" +
                "grant_type=client_credentials";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(requestUri));
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Content = new StringContent(requestContent, Encoding.UTF8, "application/x-www-form-urlencoded");
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.SendAsync(requestMessage);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            return this.Json(responseContent, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetTokenV2() {
            var requestUri = "https://login.microsoftonline.com/" + Settings.Default.TenantName + "/oauth2/v2.0/token";
            var clientId = Settings.Default.ClientId;
            var scope = Settings.Default.ResourceUri + "/.default";
            var clientSecret = Settings.Default.ClientSecret;
            var requestContent =
                "client_id=" + Uri.EscapeDataString(clientId) + "&" +
                "scope=" + Uri.EscapeDataString(scope) + "&" +
                "client_secret=" + Uri.EscapeDataString(clientSecret) + "&" +
                "grant_type=client_credentials";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(requestUri));
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Content = new StringContent(requestContent, Encoding.UTF8, "application/x-www-form-urlencoded");
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.SendAsync(requestMessage);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            return this.Json(responseContent, JsonRequestBehavior.AllowGet);
        }

    }

}
