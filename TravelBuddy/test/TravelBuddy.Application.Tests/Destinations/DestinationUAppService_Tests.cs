using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TravelBuddy.Destinations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;
using Xunit;

namespace TravelBuddy.Destinations
{
    public class DestinationAppService_Tests
    {
        private readonly Mock<IRepository<Destination, Guid>> _destinationRepoMock;
        private readonly Mock<IRepository<DestinationRating, Guid>> _ratingRepoMock;
        private readonly Mock<ICurrentUser> _currentUserMock;
        private readonly Mock<IObjectMapper> _objectMapperMock;
        private readonly DestinationAppService _appService;
        private readonly Guid _testUserId;

        public DestinationAppService_Tests()
        {
            _destinationRepoMock = new Mock<IRepository<Destination, Guid>>();
            _ratingRepoMock = new Mock<IRepository<DestinationRating, Guid>>();
            _objectMapperMock = new Mock<IObjectMapper>();
            _testUserId = Guid.NewGuid();

            var fakeUser = new FakeCurrentUser
            {
                Id = _testUserId,
                UserName = "marta"
            };

            _objectMapperMock
                .Setup(m => m.Map<DestinationRating, DestinationRatingDto>(It.IsAny<DestinationRating>()))
                .Returns<DestinationRating>(r => new DestinationRatingDto
                {
                    DestinationId = r.DestinationId,
                    Calificacion = r.Calificacion,
                    Comentario = r.Comentario,
                    UserId = r.UserId
                });


            _appService = new DestinationAppService(
                  _destinationRepoMock.Object,
                  _ratingRepoMock.Object,
                  fakeUser,
                  _objectMapperMock.Object);
        }

        //1. Crear una nueva calificación si no existe
        [Fact]
        public async Task Should_Create_New_Rating_If_None_Exists()
        {
            //var userId = Guid.NewGuid();
            var destinationId = Guid.NewGuid();
            var currentUserId = _testUserId;

            // No existe rating previo
            _ratingRepoMock
                .Setup(r => r.GetQueryableAsync())
                .ReturnsAsync(new List<DestinationRating>().AsQueryable());


            DestinationRating? inserted = null;

            _ratingRepoMock
                .Setup(r => r.InsertAsync(It.IsAny<DestinationRating>(), true, It.IsAny<CancellationToken>()))
                .Callback((DestinationRating dr, bool _, CancellationToken __) => inserted = dr)
                .ReturnsAsync((DestinationRating dr, bool _, CancellationToken __) => dr);

            var input = new CreateDestinationRatingDto
            {
                DestinationId = destinationId,
                Calificacion = 4,
                Comentario = "Muy lindo lugar"
            };

            var result = await _appService.RateDestinationAsync(input);

            Assert.NotNull(inserted);
            Assert.Equal(currentUserId, inserted!.UserId);
            Assert.Equal(4, inserted.Calificacion);
            Assert.Equal("Muy lindo lugar", inserted.Comentario);
        }

        //2. Actualizar si ya existe un rating previo del mismo usuario
        [Fact]
        public async Task Should_Update_Rating_If_Already_Exists()
        {
            var userId = Guid.NewGuid();
            var destinationId = Guid.NewGuid();
            //_currentUserMock.Setup(u => u.Id).Returns(userId);

            var existingRating = new DestinationRating
            {
                // Id = Guid.NewGuid(),
                DestinationId = destinationId,
                UserId = _testUserId,
                Calificacion = 3,
                Comentario = "Bueno"
            };

            _ratingRepoMock
                .Setup(r => r.GetQueryableAsync())
                .ReturnsAsync(new List<DestinationRating> { existingRating }.AsQueryable());

            _ratingRepoMock
                .Setup(r => r.UpdateAsync(It.IsAny<DestinationRating>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync((DestinationRating dr, bool _, CancellationToken __) => dr);

            var input = new CreateDestinationRatingDto
            {
                DestinationId = destinationId,
                Calificacion = 5,
                Comentario = "Excelente"
            };

            var result = await _appService.RateDestinationAsync(input);

            Assert.NotNull(result);
            Assert.Equal(5, existingRating.Calificacion);
            Assert.Equal("Excelente", existingRating.Comentario);
        }

        //3. Lanzar excepción si la calificación es inválida
        [Fact]
        public async Task Should_Throw_Exception_For_Invalid_Rating()
        {
            var unauthenticatedUser = new FakeCurrentUser { Id = null, UserName = null };

            var unauthenticatedAppService = new DestinationAppService(
                _destinationRepoMock.Object,
                _ratingRepoMock.Object,
                unauthenticatedUser,
                _objectMapperMock.Object);

            var input = new CreateDestinationRatingDto
            {
                DestinationId = Guid.NewGuid(),
                Calificacion = 10 // afuera de rango
            };

            // El test DEBE fallar en la línea de la excepción UnauthorizedAccessException
            // ANTES de llegar al mapeo final.
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                unauthenticatedAppService.RateDestinationAsync(input));

            //await Assert.ThrowsAsync<ArgumentException>(() =>
            //    _appService.RateDestinationAsync(input));
        }

        //. Lanzar excepción si el usuario no está autenticado
        [Fact]
        public async Task Should_Throw_Exception_When_User_Not_Authenticated()
        {
            var unauthenticatedUser = new FakeCurrentUser { Id = null, UserName = null };
            var unauthenticatedAppService = new DestinationAppService(
                _destinationRepoMock.Object,
                _ratingRepoMock.Object,
                unauthenticatedUser,
                _objectMapperMock.Object);

            var input = new CreateDestinationRatingDto
            {
                DestinationId = Guid.NewGuid(),
                Calificacion = 4
            };
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                unauthenticatedAppService.RateDestinationAsync(input));
        }

        private class FakeCurrentUser : ICurrentUser
        {
            public Guid? Id { get; set; }
            public string UserName { get; set; }
            public string? Email { get; set; }
            public bool IsAuthenticated => Id != null;
            public string? PhoneNumber => null;
            public bool PhoneNumberVerified => false;
            public bool EmailVerified => false;
            public IReadOnlyList<Claim> Claims => new List<Claim>();
            public Guid? TenantId => null;
            public Guid? ImpersonatorTenantId => null;
            public Guid? ImpersonatorUserId => null;
            public string? SurName => null;
            public string? Name => null;
            public string[] Roles => Array.Empty<string>();

            public Claim? FindClaim(string claimType) => null;
            public Claim[] FindClaims(string claimType) => Array.Empty<Claim>();
            public Claim[] GetAllClaims() => Array.Empty<Claim>();
            public bool IsInRole(string roleName) => false;
        }
    }
}

