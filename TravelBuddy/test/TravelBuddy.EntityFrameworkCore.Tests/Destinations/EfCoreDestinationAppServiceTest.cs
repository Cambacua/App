using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBuddy.EntityFrameworkCore;
using Xunit;

namespace TravelBuddy.Destination
{
    [Collection(TravelBuddyTestConsts.CollectionDefinitionName)]
    public class EfCoreDestinationAppServiceTest : DestinationAppServiceTest<TravelBuddyEntityFrameworkCoreTestModule>
    {
    }
}
