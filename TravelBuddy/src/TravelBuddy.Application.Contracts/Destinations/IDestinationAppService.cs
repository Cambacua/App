using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace TravelBuddy.Destinations
{
    public interface IDestinationAppService :
        ICrudAppService<
            DestinationDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateDestinationDto>
    {

        Task<List<DestinationRatingDto>> GetMyRatingsAsync();
        Task<DestinationRatingDto> RateDestinationAsync(CreateDestinationRatingDto input);

    }
}
