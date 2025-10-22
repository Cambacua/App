using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using TravelBuddy.EntityFrameworkCore;

namespace TravelBuddy.Destinations
{
    public class DestinationRatingRepository: EfCoreRepository <TravelBuddyDbContext, DestinationRating, Guid>, IDestinationRatingRepository
    {
        public DestinationRatingRepository(IDbContextProvider<TravelBuddyDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
