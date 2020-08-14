using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace boticario.ExternalAPIs.boticario
{
    public class BoticarioConnection
    {
        private static readonly string urlBase = "https://mdaqk8ek5j.execute-api.us-east-1.amazonaws.com/v1/cashback";

        public static async Task<T> Connect<T>(string route) where T : class
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(route).Result;

                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<T>(json);
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
