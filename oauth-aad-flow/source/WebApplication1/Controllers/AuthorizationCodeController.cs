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

    public class AuthorizationCodeController : Controller {

        public ActionResult SignInV1() {
            var requestUri = "https://login.microsoftonline.com/" + Settings.Default.TenantName + "/oauth2/authorize";
            var clientId = Settings.Default.ClientId;
            var redirectUri = this.Request.Url.GetLeftPart(UriPartial.Authority) + this.Url.Action("GetTokenV1");
            var resource = Settings.Default.ResourceUri;
            return new RedirectResult(requestUri + "? " +
                "client_id=" + Uri.EscapeDataString(clientId) + "&" +
                "response_type=code" + "&" +
                "redirect_uri=" + Uri.EscapeDataString(redirectUri) + "&" +
                "resource=" + Uri.EscapeDataString(resource)
            );
        }

        public async Task<ActionResult> GetTokenV1(string code) {
            var requestUri = "https://login.microsoftonline.com/" + Settings.Default.TenantName + "/oauth2/token";
            var clientId = Settings.Default.ClientId;
            var resource = Settings.Default.ResourceUri;
            var redirectUri = this.Request.Url.GetLeftPart(UriPartial.Authority) + this.Url.Action("GetTokenV1");
            var clientSecret = Settings.Default.ClientSecret;
            var requestContent =
                "grant_type=authorization_code" + "&" +
                "client_id=" + Uri.EscapeDataString(clientId) + "&" +
                "resource=" + Uri.EscapeDataString(resource) + "&" +
                "code=" + Uri.EscapeDataString(code) + "&" +
                "redirect_uri=" + Uri.EscapeDataString(redirectUri) + "&" +
                "client_secret=" + Uri.EscapeDataString(clientSecret);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(requestUri));
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Content = new StringContent(requestContent, Encoding.UTF8, "application/x-www-form-urlencoded");
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.SendAsync(requestMessage);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            return this.Json(responseContent, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SignInV2() {
            var requestUri = "https://login.microsoftonline.com/" + Settings.Default.TenantName + "/oauth2/v2.0/authorize";
            var clientId = Settings.Default.ClientId;
            var redirectUri = this.Request.Url.GetLeftPart(UriPartial.Authority) + this.Url.Action("GetTokenV2");
            var scope = string.Join(" ", Settings.Default.Scope.Split(' ').Select(item => Settings.Default.ResourceUri + "/" + item));
            return new RedirectResult(requestUri + "? " +
                "client_id=" + Uri.EscapeDataString(clientId) + "&" +
                "response_type=code" + "&" +
                "redirect_uri=" + Uri.EscapeDataString(redirectUri) + "&" +
                "scope=offline_access%20" + Uri.EscapeDataString(scope)
            );
        }

        public async Task<ActionResult> GetTokenV2(string code) {
            var requestUri = "https://login.microsoftonline.com/" + Settings.Default.TenantName + "/oauth2/v2.0/token";
            var clientId = Settings.Default.ClientId;
            var scope = string.Join(" ", Settings.Default.Scope.Split(' ').Select(item => Settings.Default.ResourceUri + "/" + item));
            var redirectUri = this.Request.Url.GetLeftPart(UriPartial.Authority) + this.Url.Action("GetTokenV2");
            var clientSecret = Settings.Default.ClientSecret;
            var requestContent =
                "grant_type=authorization_code" + "&" +
                "client_id=" + Uri.EscapeDataString(clientId) + "&" +
                "scope=offline_access%20" + Uri.EscapeDataString(scope) + "&" +
                "code=" + Uri.EscapeDataString(code) + "&" +
                "redirect_uri=" + Uri.EscapeDataString(redirectUri) + "&" +
                "client_secret=" + Uri.EscapeDataString(clientSecret);
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