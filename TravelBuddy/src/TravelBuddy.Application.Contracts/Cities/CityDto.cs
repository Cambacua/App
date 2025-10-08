using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBuddy.Cities
{
    public class CityDto
    //formato de datos que la API va a devolver al front
    {
        public string Nombre { get; set; }
        public string Pais { get; set; }
        public string Region { get; set; }

    }
}
