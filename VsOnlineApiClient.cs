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

        T GetApi<T>(string org, string api);
    }

    public class VsOnlineApiClient : IVsOnlineApiClient
    {
        private string username;
        private string password;
        
        public T GetApi<T>(string org, string project, string api)
        {
            var uri = new Uri($"https://{org}.visualstudio.com/{project}/_apis/{api}?api-version=5.0");
            var json = CallApiWithBasicCreds(uri);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public T GetApi<T>(string org, string api)
        {
            var uri = new Uri($"https://dev.azure.com/{org}/_apis/{api}?api-version=5.0");
            var json = CallApiWithBasicCreds(uri);
            return JsonConvert.DeserializeObject<T>(json);
        }

        private string CallApiWithBasicCreds(Uri uri)
        {
            var auth = string.Format("{0}:{1}", username, password);
            var bytes = Convert.ToBase64String(Encoding.ASCII.GetBytes(auth));

            using (var client = new HttpClient())
            {
                // Works for user/pass and user/token
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

        public void SetBasicCredentials(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}