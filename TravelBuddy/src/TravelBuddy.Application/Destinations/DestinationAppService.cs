/*using System;
using System.Threading.Tasks;
using TravelBuddy.Cities;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace TravelBuddy.Destinations
{
    public class DestinationAppService :
       CrudAppService<
           Destination,
           DestinationDto,
           Guid,
           Volo.Abp.Application.Dtos.PagedAndSortedResultRequestDto,IDestinationAppService>
    {
        private readonly ICitySearchService _citySearchService;
        private readonly ICurrentUser _currentUser;

        public DestinationAppService(
            IRepository<Destination, Guid> repository,
            ICitySearchService citySearchService)
            : base(repository)
        {
            _citySearchService = citySearchService;
        }

        public async Task<CitySearchResultDto> SearchCitiesByNameAsync(CitySearchRequestDto request)
        {
            var user = _currentUser;
            return await _citySearchService.SearchCitiesByNameAsync(request);
        }
    }
}*/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace TravelBuddy.Destinations
{
    public class DestinationAppService : ApplicationService
    {
        public async Task<List<DestinationDto>> GetAllAsync()
        {
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
                    Disponible = true,
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                    CategoriaId = Guid.NewGuid(),
                    CategoriaName = "Europa"
                },
                new DestinationDto
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Londres",
                    Descripcion = "Capital del Reino Unido.",
                    Ubicacion = "Inglaterra",
                    Precio = 1800,
                    ImagenUrl = "https://picsum.photos/400/200?random=2",
                    Disponible = true,
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                    CategoriaId = Guid.NewGuid(),
                    CategoriaName = "Europa"
                },
                new DestinationDto
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Buenos Aires",
                    Descripcion = "Capital de Argentina.",
                    Ubicacion = "Argentina",
                    Precio = 900,
                    ImagenUrl = "https://picsum.photos/400/200?random=3",
                    Disponible = true,
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                    CategoriaId = Guid.NewGuid(),
                    CategoriaName = "Sudamérica"
                }
            };
        }
    }
}
