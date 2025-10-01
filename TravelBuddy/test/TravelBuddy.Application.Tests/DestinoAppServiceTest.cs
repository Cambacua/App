using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBuddy.Destinations;

namespace TravelBuddy
{
    public class DestinoAppServiceTest : TravelBuddyApplicationTestBase<TravelBuddyApplicationTestModule>
    {
        private readonly IDestinationAppService destinationAppService;

        public DestinoAppServiceTest()
        {
            GetRequiredService<TravelBuddyApplicationTestModule>();
            destinationAppService = GetRequiredService<IDestinationAppService>();
        }

       /* public async Task DebeGuardarDestinosCorrectamente()
        {
            // Arrange
            var input = new CreateUpdateDestinationDto
            {
                Nombre = "París",
                Descripcion = "La ciudad del amor",
                Pais = "Francia",
                Ciudad = "París"
            }

            //Act
            var resultado = await destinoAppService.CrearAsync(input);

            //Assert
            resultado.ShouldNotBeNull();
            resultado.Id.ShouldNotBe(Guid.Empty);
            resultado.Nombre.ShouldBe(input.Nombre);
            resultado.Descripcion.ShouldBe(input.Descripcion);
            resultado.Pais.ShouldBe(input.Pais);
            resultado.Ciudad.ShouldBe(input.Ciudad);
        }*/

        /*public async Task NoDebeGuardarDestinoSinNombre()
        {
            // Arrange
            var input = new CrearDestinoDto
            {
                Nombre = "", // Nombre vacío
                Descripcion = "La ciudad del amor",
                Pais = "Francia",
                Ciudad = "París"
            };
            // Act & Assert
            await Should.ThrowAsync<ValidationException>(async () =>
            {
                await destinoAppService.CrearAsync(input);
            });
        }*/
    }
}
