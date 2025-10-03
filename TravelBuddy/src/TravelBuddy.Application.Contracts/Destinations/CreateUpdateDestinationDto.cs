using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBuddy.Destinations
{
    public class CreateUpdateDestinationDto
    {
        public required string Nombre { get; set; }
        public required string Pais { get; set; }
        public required string Descripcion { get; set; }
        public required string Ubicacion { get; set; }
        //public decimal Precio { get; set; }
        public required string ImagenUrl { get; set; }
        public bool Disponible { get; set; }
        public Guid CategoriaId { get; set; }
    }
}
