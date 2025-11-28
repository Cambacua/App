using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using TravelBuddy.Destinations;

namespace TravelBuddy.Destinations
{
    public class DestinationAppService : ApplicationService
    {
        public async Task<List<DestinationDto>> GetAllAsync()
        {
            // Simulación de datos (mock)
            return new List<DestinationDto>
            {
                new DestinationDto
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Paris",
                    Descripcion = "Ciudad del amor y las luces.",
                    Ubicacion = "Francia",
                    Precio = 1500,
                    ImagenUrl = "https://picsum.photos/400/200?random=1",
                    Disponible = true
                },
                new DestinationDto
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Londres",
                    Descripcion = "Capital del Reino Unido.",
                    Ubicacion = "Inglaterra",
                    Precio = 1800,
                    ImagenUrl = "https://picsum.photos/400/200?random=2",
                    Disponible = true
                },
                new DestinationDto
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Buenos Aires",
                    Descripcion = "Capital de Argentina.",
                    Ubicacion = "Argentina",
                    Precio = 900,
                    ImagenUrl = "https://picsum.photos/400/200?random=3",
                    Disponible = true
                }
            };
        }
    }
}
