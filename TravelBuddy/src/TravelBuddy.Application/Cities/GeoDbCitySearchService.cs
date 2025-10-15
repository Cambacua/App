using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TravelBuddy.Cities
{
    public class GeoDbCitySearchService : ICitySearchService
    {
        private readonly HttpClient _httpClient;

        public GeoDbCitySearchService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CitySearchResultDto> SearchCitiesByNameAsync(CitySearchRequestDto request)
        {
            var url = $"https://wft-geo-db.p.rapidapi.com/v1/geo/cities?namePrefix={request.PartialName}";

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            httpRequest.Headers.Add("X-RapidAPI-Key", "TU_API_KEY_AQUI");
            httpRequest.Headers.Add("X-RapidAPI-Host", "wft-geo-db.p.rapidapi.com");

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
}

