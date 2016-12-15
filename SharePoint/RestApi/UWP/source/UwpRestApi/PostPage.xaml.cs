using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Karamem0.Samples.SharePoint.UwpRestApi {

    public sealed partial class PostPage : Page {

        private string SiteUrl { get; set; }

        private string AccessToken { get; set; }

        public PostPage() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            var tuple = (Tuple<string, string>)e.Parameter;
            this.SiteUrl = tuple.Item1;
            this.AccessToken = tuple.Item2;
        }

        private async void OnCheckInTextBoxClick(object sender, RoutedEventArgs e) {
            var jsonContent = new JObject(
                new JProperty("__metadata", new JObject(new JProperty("type", "SP.Data.UwpRestApiListItem"))),
                new JProperty("Title", "出勤"),
                new JProperty("Timestamp", DateTime.Now)
            );
            using (var httpClient = new HttpClient()) {
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json; odata=verbose");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + this.AccessToken);
                var requestUri = new Uri(this.SiteUrl + 
                    "/_api/web/lists/getbytitle('" + Uri.EscapeUriString("タイム レコーダー") + "')/items", 
                    UriKind.Absolute);
                var stringContent = new StringContent(JsonConvert.SerializeObject(jsonContent));
                Debug.WriteLine(await stringContent.ReadAsStringAsync());
                stringContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;odata=verbose");
                var httpRes = await httpClient.PostAsync(requestUri, stringContent);
                Debug.WriteLine(await httpRes.Content.ReadAsStringAsync());
            }
        }

        private async void OnCheckOutTextBoxClick(object sender, RoutedEventArgs e) {
            var jsonContent = new JObject(
                new JProperty("__metadata", new JObject(new JProperty("type", "SP.Data.UwpRestApiListItem"))),
                new JProperty("Title", "退勤"),
                new JProperty("Timestamp", DateTime.Now)
            );
            using (var httpClient = new HttpClient()) {
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json; odata=verbose");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + this.AccessToken);
                var requestUri = new Uri(this.SiteUrl +
                    "/_api/web/lists/getbytitle('" + Uri.EscapeUriString("タイム レコーダー") + "')/items",
                    UriKind.Absolute);
                var stringContent = new StringContent(JsonConvert.SerializeObject(jsonContent));
                Debug.WriteLine(await stringContent.ReadAsStringAsync());
                stringContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;odata=verbose");
                var httpRes = await httpClient.PostAsync(requestUri, stringContent);
                Debug.WriteLine(await httpRes.Content.ReadAsStringAsync());
            }
        }

    }

}
