using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBuddy.Destinations;

namespace TravelBuddy.Cities
{
    public class CitySearchRequestDto
    //formato de datos que la API va a devolver al front
     {
         public string Name { get; set; }
         public string CountryName { get; set; }

     }
}
