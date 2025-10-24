using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBuddy.Destinations
{
    public class CreateDestinationRatingDto
    {
        [Required]
        public Guid DestinationId { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "La calificación debe ser entre 1 y 5")]
        public int Calificacion { get; set; }
        //public string Calificacion { get; set; }
        [StringLength(500)]
        public string? Comentario { get; set; }


    }
}
