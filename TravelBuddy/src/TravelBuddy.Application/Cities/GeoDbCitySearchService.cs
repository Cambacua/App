using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
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

            try
            {
                var response = await _httpClient.SendAsync(httpRequest);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(json);

                var result = new CitySearchResultDto();

                if (doc.RootElement.TryGetProperty("data", out var dataElement) && dataElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var city in dataElement.EnumerateArray())
                    {
                        var name = city.GetProperty("city").GetString();
                        var country = city.TryGetProperty("country", out var countryProp) ? countryProp.GetString() : "Unknown";

                        result.Cities.Add(new CityDto
                        {
                            Name = name,
                            Country = country
                        });
                    }
                }

                return result;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"[API ERROR] {ex.Message}");
                // In a real app, log the error: _logger.LogError(ex, "Error fetching cities");
                // Return empty result instead of crashing
                return new CitySearchResultDto();
            }


        }
    }
}


