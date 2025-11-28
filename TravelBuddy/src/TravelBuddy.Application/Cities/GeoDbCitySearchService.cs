/*using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace TravelBuddy.Cities
{
    public class GeoDbCitySearchService : ICitySearchService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public GeoDbCitySearchService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<CitySearchResultDto> SearchCitiesByNameAsync(CitySearchRequestDto request)
        {
            var baseUrl = _config["GeoDb:BaseUrl"];
            var apiKey = _config["GeoDb:ApiKey"];
            var apiHost = _config["GeoDb:ApiHost"];

            var url = $"{baseUrl}?namePrefix={request.PartialName}";

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            httpRequest.Headers.Add("X-RapidAPI-Key", apiKey);
            httpRequest.Headers.Add("X-RapidAPI-Host", apiHost);

            var response = await _httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);

            var result = new CitySearchResultDto();

            foreach (var city in doc.RootElement.GetProperty("data").EnumerateArray())
            {
                result.Cities.Add(new CityDto
                {
                    Name = city.GetProperty("city").GetString(),
                    Country = city.GetProperty("country").GetString()
                });
            }

            return result;
        }
    }
}*/
/*using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TravelBuddy.Cities
{
    public class GeoDbCitySearchService : ICitySearchService
    {
        private readonly HttpClient _httpClient;
        private readonly GeoDbOptions _options;

        public GeoDbCitySearchService(
            HttpClient httpClient,
            IOptions<GeoDbOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;

            string cleanApiKey = RemoveNonAsciiCharacters(_options.ApiKey);
            //  headers 
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-RapidAPI-Key", _options.ApiKey);
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-RapidAPI-Host", _options.ApiHost);

        }
        private static string RemoveNonAsciiCharacters(string input)
        {
            return string.IsNullOrEmpty(input)
                ? input
                : Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(input));
        }
        public async Task<CitySearchResultDto> SearchCitiesByNameAsync(CitySearchRequestDto request)
        {
            var url = $"{_options.BaseUrl}?namePrefix={request.PartialName}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);

            var result = new CitySearchResultDto();

            foreach (var city in doc.RootElement.GetProperty("data").EnumerateArray())
            {
                result.Cities.Add(new CityDto
                {
                    Name = city.GetProperty("city").GetString(),
                    Country = city.GetProperty("country").GetString()
                });
            }

            return result;
        }
    }
}*/
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Options;
using TravelBuddy.Cities;

namespace TravelBuddy.Cities
{
    public class GeoDbCitySearchService : ICitySearchService
    {
        private readonly HttpClient _httpClient;
        private readonly GeoDbOptions _options;

        public GeoDbCitySearchService(
            HttpClient httpClient,
            IOptions<GeoDbOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;

            _httpClient.DefaultRequestHeaders.Clear();

        }

        public async Task<CitySearchResultDto> SearchCitiesByNameAsync(CitySearchRequestDto request)
        {
            // GeoNames endpoint
            var url = $"{_options.BaseUrl}?q={request.Name}&maxRows=10&username={_options.ApiKey}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<GeoNamesResponse>(json);

            var result = new CitySearchResultDto();

            if (data?.Geonames != null)
            {
                foreach (var city in data.Geonames)
                {
                    result.Cities.Add(new CityDto
                    {
                        Name = city.Name,             
                        Country = city.CountryName   
                    });
                }
            }

            return result;
        }
    }
}
