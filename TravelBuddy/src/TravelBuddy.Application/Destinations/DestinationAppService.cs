using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBuddy.Cities;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace TravelBuddy.Destinations
{
    public class DestinationAppService :
       CrudAppService<
           Destination,
           DestinationDto,
           Guid,
           Volo.Abp.Application.Dtos.PagedAndSortedResultRequestDto,IDestinationAppService>
    {
        private readonly ICitySearchService _citySearchService;

        public DestinationAppService(
            IRepository<Destination, Guid> repository,
            ICitySearchService citySearchService)
            : base(repository)
        {
            _citySearchService = citySearchService;
        }

        public async Task<CitySearchResultDto> SearchCitiesByNameAsync(CitySearchRequestDto request)
        {
            return await _citySearchService.SearchCitiesByNameAsync(request);
        }
    }
}
