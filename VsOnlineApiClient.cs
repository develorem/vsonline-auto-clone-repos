using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace vs_api
{
    public class VsOnlineApiClient
    {
        private readonly Settings settings;

        public VsOnlineApiClient(Settings settings)
        {
            this.settings = settings;
        }

        public T GetApi<T>(string org, string project, string api)
        {
            var json = Get(org, project, api);
            return JsonConvert.DeserializeObject<T>(json);

        }
        public string Get(string org, string project, string api)
        {
            var uri = GetApiUri(org, project, api);

            var auth = string.Format("{0}:{1}", settings.Username, settings.Password);
            var bytes = Convert.ToBase64String(Encoding.ASCII.GetBytes(auth));

            using (var client = new HttpClient())
            {
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
            var relativePath = $"{project}/_apis/{api}?api-version=5.0";
            var apiUri = new Uri(domainUri, relativePath);
            return apiUri;
        }
    }
}
