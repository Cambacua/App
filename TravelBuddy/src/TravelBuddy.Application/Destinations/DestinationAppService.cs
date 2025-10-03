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
                Destination,   // entidad
                DestinationDto, //DTO que devuelve 
                Guid,       // tipo de clave primaria 
                Volo.Abp.Application.Dtos.PagedAndSortedResultRequestDto,  // para listar con paginacion 
                 //CreateUpdateDestinationDto>, // DTO para crear/actualizar
                IDestinationAppService // interfaz para implementar  
    {
        public DestinationAppService(IRepository<Destination, Guid> repository)
            : base(repository)
        {
        }
    }
}
