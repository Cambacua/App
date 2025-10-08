using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TravelBuddy.Cities
{
    public class CitySearchService: ICitySearchService
    {
        private readonly HttpClient _httpClient;

        public CitySearchService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        /*public async Task<List<CityDto>> SearchCitiesByNameAsync(string nombreParcial)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                 $"https://wft-geo-db.p.rapidapi.com/v1/geo/cities?namePrefix={nombreParcial}");

            request.Headers.Add("X-RapidAPI- Key", "API_KEY")
        }*/
}


