using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace TravelBuddy.Destinations
{
    public class DestinationAppService :
            CrudAppService<
                Destination,
                DestinationDto,
                Guid,
                Volo.Abp.Application.Dtos.PagedAndSortedResultRequestDto,
                IDestinationAppService>
    {
        public DestinationAppService(IRepository<Destination, Guid> repository)
            : base(repository)
        {
        }
    }
}
