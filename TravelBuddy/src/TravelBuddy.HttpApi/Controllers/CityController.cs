using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TravelBuddy.Cities;

namespace TravelBuddy.Controllers;

[Route("api/app/cities")]

[Authorize] 

//Con esto, si NO hay token 401. Con token funciona
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
        var request = new CitySearchRequestDto { Name = name };
        return _service.SearchCitiesByNameAsync(request);
    }
}

