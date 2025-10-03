using Autofac.Core;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBuddy.Destinations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Xunit;
using static System.Net.WebRequestMethods;

namespace TravelBuddy.Destination
{
    public abstract class DestinationAppServiceTest<TStartupModule> : TravelBuddyApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IDestinationAppService _service;
        private readonly IRepository<TravelBuddy.Destinations.Destination, Guid> _destinationRepository;
        protected DestinationAppServiceTest()
        {
            _service = GetRequiredService<IDestinationAppService>();
        }

        [Fact]

        public async Task CreateAsync_ShouldReturnCreateDestinationDTo()
        {
            //Arrange (preparaciond del test)
            var input = new CreateUpdateDestinationDto
            {
                Nombre = "Paris",
                Pais = "Francia",
                Descripcion = "La ciudad del amor",
                Ubicacion = "48.8566° N, 2.3522° E",
                ImagenUrl = "https://example.com/paris.jpg" // Fix for CS9035
            };

            //Act (ejecucion del test)
            var result = await _service.CreateAsync(input);

            //Assert (verificacion del test)
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(Guid.Empty);
            result.Nombre.ShouldBe(input.Nombre);
            result.Descripcion.ShouldBe(input.Descripcion);
            result.Pais.ShouldBe(input.Pais);
            result.Ubicacion.ShouldBe(input.Ubicacion);
        }

        [Fact]
        public async Task ShouldSaveNewDestinationAndBeRetrievable()
        {
            // Arrange: prepara el input
            //var destinationRepository = GetRequiredService<IDestinoRepository>();
            var input = new CreateUpdateDestinationDto
            {
                Nombre = "Tokyo",
                Pais = "Japon", //esta marcado como required
                Descripcion = "La capital tecnologica de Asia",
                Ubicacion = "35.6895° N, 139.6917° E",
                ImagenUrl = "https://ejemplo.com/tokyo-skyline.jpg"

            };

            // Act: Llama al application service
            var result = await _service.CreateAsync(input);

            // Assert:
            // 1. Usa el repositorio para buscar la entidad con el ID generado.
            var savedDestino = await _destinationRepository.GetAsync(result.Id);

            // 2. Verifica que la entidad existe y los datos coinciden.
            savedDestino.ShouldNotBeNull();
            savedDestino.Nombre.ShouldBe("Tokyo");
        }

        [Fact]
        public async Task ShouldThrowExceptionWhenNameIsNull()
        {
            // Arrange: Crea un DTO inválido (asumiendo que Nombre no puede ser null)
            var input = new CreateUpdateDestinationDto
            {
                Nombre = null, // Valor no permitido
                Pais = "Japon",
                Descripcion = "Prueba de fallo",
                Ubicacion = "35.6895° N, 139.6917° E",
                ImagenUrl = "https://ejemplo.com/fail.jpg"
            };

            // Act & Assert: Se espera que la llamada lance una excepción de validación
            await Assert.ThrowsAsync<AbpValidationException>(async () =>
            {
                await _service.CreateAsync(input);
            });
    }
}
}
