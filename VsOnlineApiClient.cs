using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace AutoCloner.VsOnline
{
    public interface IVsOnlineApiClient
    {
        void SetBasicCredentials(string username, string password);

        T GetApi<T>(string org, string project, string api);
    }

    public class VsOnlineApiClient : IVsOnlineApiClient
    {
        private string username;
        private string password;
        
        public T GetApi<T>(string org, string project, string api)
        {
            var json = Get(org, project, api);
            return JsonConvert.DeserializeObject<T>(json);
        }

        private string Get(string org, string project, string api)
        {
            var uri = GetApiUri(org, project, api);

            var auth = string.Format("{0}:{1}", username, password);
            var bytes = Convert.ToBase64String(Encoding.ASCII.GetBytes(auth));

            using (var client = new HttpClient())
            {
                // TODO Change to support multiple auth types; people will normally want to do this with tokens
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", bytes);

                using (var response = client.GetAsync(uri).Result)
                {
                    response.EnsureSuccessStatusCode();
                    var responseBody = response.Content.ReadAsStringAsync().Result;
                    return responseBody;
                }
            }
        }

        private static Uri GetApiUri(string org, string project, string api)
        {
            var domainUri = new Uri($"https://{org}.visualstudio.com/{project}/_apis/{api}?api-version=5.0");
            return domainUri;
        }

        public void SetBasicCredentials(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

    }
}
