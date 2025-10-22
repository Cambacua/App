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

        //public Guid DestinationId { get; set; }
        //public Guid UserId { get; set; }
    }
}
