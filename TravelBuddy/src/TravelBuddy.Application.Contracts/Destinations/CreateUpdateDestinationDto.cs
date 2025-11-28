using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBuddy.Destinations
{
    public class CreateUpdateDestinationDto
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Pais { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public string Ubicacion { get; set; }

        public decimal Precio { get; set; }

        [Required]
        public string ImagenUrl { get; set; }

        public bool Disponible { get; set; }

        public Guid CategoriaId { get; set; }
    }
}
