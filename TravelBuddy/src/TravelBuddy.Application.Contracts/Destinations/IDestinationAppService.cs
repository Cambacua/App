using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using TravelBuddy.Cities;

namespace TravelBuddy.Destinations
{
    public interface IDestinationAppService :
        ICrudAppService<
            DestinationDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateDestinationDto>
    {
        Task<CitySearchResultDto> SearchCitiesByNameAsync(CitySearchRequestDto request);
    }
}
