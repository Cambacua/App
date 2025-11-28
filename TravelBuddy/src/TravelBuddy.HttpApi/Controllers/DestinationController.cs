using Microsoft.AspNetCore.Mvc;
using Polly;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelBuddy.Destinations;
using Volo.Abp.AspNetCore.Mvc;
using TravelBuddy.Destinations;

namespace TravelBuddy.HttpApi.Destinations
{
    [Route("api/app/destination")]
    public class DestinationController : AbpController
    {
        private readonly DestinationAppService _service;

        public DestinationController(DestinationAppService service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<List<DestinationDto>> GetAllAsync()
        {
            return _service.GetAllAsync();
        }


    }

}
