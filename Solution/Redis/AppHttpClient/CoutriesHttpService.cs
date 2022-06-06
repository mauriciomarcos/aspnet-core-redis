using System.Net.Http;
using System.Threading.Tasks;

namespace Redis.AppHttpClient
{
    public class CoutriesHttpService : ICoutriesHttpService
    {
        private readonly HttpClient _httpClient;

        public CoutriesHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> GetCountries(string url) =>  await _httpClient.GetAsync(url);        
    }
}