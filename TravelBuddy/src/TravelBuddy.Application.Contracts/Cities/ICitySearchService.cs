using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBuddy.Cities
{
    public interface ICitySearchService
    {
        Task<List<CityDto>> SearchCitiesByNameAsync(string partialName);
    }
}
