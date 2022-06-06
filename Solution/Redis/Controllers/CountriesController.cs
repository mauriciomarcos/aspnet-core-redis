using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Redis.AppHttpClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redis.Controllers
{
    [ApiController]
    [Route("api/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ICoutriesHttpService _httpService;
        private const string CONTRIES_KEY = "Contries";

        public CountriesController(IDistributedCache distributedCache, ICoutriesHttpService httpService)
        {
            _distributedCache = distributedCache;
            _httpService = httpService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var countriesObjects = await _distributedCache.GetStringAsync(CONTRIES_KEY);
            if (!string.IsNullOrEmpty(countriesObjects))
                return Ok(JsonConvert.DeserializeObject<IEnumerable<Country>>(countriesObjects));


            var response = await _httpService.GetCountries("v2/all");
            var responseData = await response.Content.ReadAsStringAsync();
            var countries = JsonConvert.DeserializeObject<IEnumerable<Country>>(responseData);

            var memoryCaheEntryOpts = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
                SlidingExpiration = TimeSpan.FromSeconds(1200)
            };

            await _distributedCache.SetStringAsync(CONTRIES_KEY, responseData, memoryCaheEntryOpts);

            return Ok(countries);
        }
    }
}