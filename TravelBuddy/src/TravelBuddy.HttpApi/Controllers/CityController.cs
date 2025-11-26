using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TravelBuddy.Cities;

namespace TravelBuddy.Controllers;

[Route("api/app/cities")]
public class CitiesController : TravelBuddyController
{
    private readonly ICitySearchService _service;

    public CitiesController(ICitySearchService service)
    {
        _service = service;
    }

    [HttpGet("search")]
    public Task<CitySearchResultDto> SearchAsync([FromQuery] string name)
    {
        var request = new CitySearchRequestDto { PartialName = name };
        return _service.SearchCitiesByNameAsync(request);
    }
}

