using System;
using Volo.Abp.Domain.Entities.Auditing;


namespace TravelBuddy.Destinations
{
    public class DestinationRating : AuditedAggregateRoot<Guid>, IUserOwned
    {
        public int Calificacion { get; set; }
        public string Comentario { get; set; }
        public Guid DestinationId { get; set; }
        public Guid UserId { get; set; }
        public DestinationRating() { }
        public DestinationRating(Guid id, Guid destinationId, int calificacion, string comentario, Guid userId)
            : base(id)
        {
            DestinationId = destinationId;
            UserId = userId;
            Calificacion = calificacion;
            Comentario = comentario;
        }
    }
}
