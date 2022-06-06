using System.Net.Http;
using System.Threading.Tasks;

namespace Redis.AppHttpClient
{
    public interface ICoutriesHttpService
    {
        Task<HttpResponseMessage> GetCountries(string url);
    }
}