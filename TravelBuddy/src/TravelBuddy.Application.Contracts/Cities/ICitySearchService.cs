using System.Threading.Tasks;


namespace TravelBuddy.Cities
{
    public interface ICitySearchService
    {
        Task<CitySearchResultDto> SearchCitiesByNameAsync(CitySearchRequestDto request);
    }
}
