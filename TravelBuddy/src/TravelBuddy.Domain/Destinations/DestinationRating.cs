using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace TravelBuddy.Destinations
{
    public class DestinationRating: AuditedAggregateRoot<Guid>
    {
        public int Calificacion { get; set; }
        public string Comentario { get; set; }
        public Guid DestinationId { get; set; }
        public Guid UserId { get; set; }
        public Destination Destination { get; set; }
    }
}
