using System;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelBuddy.Cities;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace TravelBuddy.Destinations
{
    [Authorize]
    public class DestinationAppService :
       CrudAppService<
           Destination,
           DestinationDto,
           Guid,
           Volo.Abp.Application.Dtos.PagedAndSortedResultRequestDto,
           CreateUpdateDestinationDto>,
       IDestinationAppService
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