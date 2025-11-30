using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelBuddy.Destinations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace TravelBuddy.Controllers
{
    [Route("api/destinations")]
    public class DestinationController : AbpController
    {
        private readonly IDestinationAppService _destinationAppService;

        public DestinationController(IDestinationAppService destinationAppService)
        {
            _destinationAppService = destinationAppService;
        }

        // 🔹 Crear nuevo destino
        [HttpPost]
        public async Task<DestinationDto> CreateAsync([FromBody] CreateUpdateDestinationDto input)
        {
            return await _destinationAppService.CreateAsync(input);
        }

    
        [HttpGet]
        public async Task<PagedResultDto<DestinationDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return await _destinationAppService.GetListAsync(input);
        }
        [HttpPost("rate-destination")]
        public async Task<DestinationRatingDto> RateDestinationAsync(CreateDestinationRatingDto input)
        {
            return await _destinationAppService.RateDestinationAsync(input);
        }

        [HttpGet("my-ratings")]
        public async Task<List<DestinationRatingDto>> GetMyRatingsAsync()
        {
            return await _destinationAppService.GetMyRatingsAsync();
        }
    }
}
