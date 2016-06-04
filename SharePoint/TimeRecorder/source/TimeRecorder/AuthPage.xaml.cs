using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Karamem0.Samples.SharePoint.TimeRecorder {

    public sealed partial class AuthPage : Page {

        private static readonly string AppId = "00000003-0000-0ff1-ce00-000000000000";

        private static readonly string AuthPath = "/_layouts/15/OAuthAuthorize.aspx";

        private static readonly string ServicePath = "/_vti_bin/client.svc";

        private static readonly string TokenUrl = "https://accounts.accesscontrol.windows.net/{0}/tokens/OAuth/2";

        private static readonly string ClientId = "<client_id>";

        private static readonly string ClientSecret = "<client_secret>";

        private static readonly string RedirectUrl = "https://localhost/TimeRecorder";

        private string SiteUrl { get; set; }

        private string TenantRealm { get; set; }

        public AuthPage() {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            this.SiteUrl = ((string)e.Parameter).TrimEnd('/');
            var authUrl = this.SiteUrl + AuthPath +
                "?isdlg=1" +
                "&client_id=" + Uri.EscapeUriString(ClientId) +
                "&scope=Web.Manage" +
                "&response_type=code" +
                "&redirect_uri=" + Uri.EscapeUriString(RedirectUrl);
            this.WebView.Source = new Uri(authUrl, UriKind.Absolute);
            this.WebView.NavigationStarting += this.OnWebViewNavigationStarting;
            using (var httpClient = new HttpClient()) {
                var serviceUrl = this.SiteUrl.TrimEnd('/') + ServicePath;
                var httpReq = new HttpRequestMessage();
                httpReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer");
                httpReq.Method = HttpMethod.Get;
                httpReq.RequestUri = new Uri(serviceUrl, UriKind.Absolute);
                var httpRes = await httpClient.SendAsync(httpReq);
                var bearer = httpRes.Headers.WwwAuthenticate.First();
                var match = Regex.Match(bearer.Parameter, "realm=\"(.+?)\"");
                this.TenantRealm = match.Groups[1].Value;
            }
        }

        private async void OnWebViewNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs e) {
            if (e.Uri.ToString().StartsWith(RedirectUrl) == true) {
                var code = e.Uri.Query.TrimStart('?').Split('&')
                    .Select(x => x.Split('='))
                    .Where(x => x[0] == "code")
                    .Select(x => x[1])
                    .FirstOrDefault();
                using (var httpClient = new HttpClient()) {
                    var tokenUrl = string.Format(TokenUrl, this.TenantRealm);
                    var httpReq = new HttpRequestMessage();
                    httpReq.Method = HttpMethod.Post;
                    httpReq.Content = new FormUrlEncodedContent(new Dictionary<string, string>() {
                        { "grant_type", "authorization_code" },
                        { "client_id", ClientId + "@" + this.TenantRealm },
                        { "client_secret", ClientSecret },
                        { "code", code },
                        { "redirect_uri", RedirectUrl },
                        { "resource", AppId + "/" +  new Uri(this.SiteUrl).Host + "@" + this.TenantRealm },
                    });
                    var reqContent = await httpReq.Content.ReadAsStringAsync();
                    httpReq.RequestUri = new Uri(tokenUrl, UriKind.Absolute);
                    var httpRes = await httpClient.SendAsync(httpReq);
                    var resContent = await httpRes.Content.ReadAsStringAsync();
                    var jsonObject = JsonConvert.DeserializeObject(resContent);
                    var accessToken = ((JObject)jsonObject)["access_token"];
                    var rootFrame = Window.Current.Content as Frame;
                    if (rootFrame != null) {
                        rootFrame.Navigate(typeof(PostPage), Tuple.Create(this.SiteUrl, (string)accessToken));
                    }
                }
            }
        }

    }

}
