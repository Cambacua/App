using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace TravelBuddy.Destinations
{
    public class DestinationRatingDto: EntityDto<Guid>
    {
        public string Calificacion { get; set; }
        public string Comentario { get; set; }
        public Guid DestinationId { get; set; }  // Para saber a qué destino pertenece
        public Guid UserId { get; set; }         // Para el filtro automático IUserOwned
    }
}
