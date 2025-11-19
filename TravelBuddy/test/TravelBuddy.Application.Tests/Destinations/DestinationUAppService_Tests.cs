
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Mock<IObjectMapper> _objectMapperMock;
        private readonly DestinationAppService _appService;
        private readonly Guid _testUserId;

        public DestinationAppService_Tests()
        {
            _destinationRepoMock = new Mock<IRepository<Destination, Guid>>();
            _ratingRepoMock = new Mock<IRepository<DestinationRating, Guid>>();
            _objectMapperMock = new Mock<IObjectMapper>();
            _testUserId = Guid.NewGuid();

            var fakeUser = new FakeCurrentUser(_testUserId);

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

        [Fact]
        public async Task Should_Create_New_Rating_If_None_Exists()
        {
            var destinationId = Guid.NewGuid();

            _ratingRepoMock
                .Setup(r => r.GetQueryableAsync())
                .ReturnsAsync(new List<DestinationRating>().AsQueryable());

            DestinationRating? inserted = null;

            _ratingRepoMock
                .Setup(r => r.InsertAsync(It.IsAny<DestinationRating>(), true, It.IsAny<CancellationToken>()))
                .Callback((DestinationRating dr, bool autoSave, CancellationToken _) => inserted = dr)
                .ReturnsAsync((DestinationRating dr, bool autoSave, CancellationToken _) => dr);

            var input = new CreateDestinationRatingDto
            {
                DestinationId = destinationId,
                Calificacion = 4,
                Comentario = "Muy lindo lugar"
            };

            var result = await _appService.RateDestinationAsync(input);

            Assert.NotNull(inserted);
            Assert.Equal(_testUserId, inserted!.UserId);
            Assert.Equal(4, inserted.Calificacion);
            Assert.Equal("Muy lindo lugar", inserted.Comentario);
        }

        [Fact]
        public async Task Should_Update_Rating_If_Already_Exists()
        {
            var destinationId = Guid.NewGuid();

            var existingRating = new DestinationRating
            {
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
                .ReturnsAsync((DestinationRating dr, bool autoSave, CancellationToken _) => dr);

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

        [Fact]
        public async Task Should_Throw_Exception_For_Invalid_Rating()
        {
            var unauth = new FakeCurrentUser(null);

            var unauthAppService = new DestinationAppService(
                _destinationRepoMock.Object,
                _ratingRepoMock.Object,
                unauth,
                _objectMapperMock.Object);

            var input = new CreateDestinationRatingDto
            {
                DestinationId = Guid.NewGuid(),
                Calificacion = 10
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => unauthAppService.RateDestinationAsync(input));
        }

        [Fact]
        public async Task Should_Throw_Exception_When_User_Not_Authenticated()
        {
            var unauth = new FakeCurrentUser(null);

            var unauthAppService = new DestinationAppService(
                _destinationRepoMock.Object,
                _ratingRepoMock.Object,
                unauth,
                _objectMapperMock.Object);

            var input = new CreateDestinationRatingDto
            {
                DestinationId = Guid.NewGuid(),
                Calificacion = 4
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => unauthAppService.RateDestinationAsync(input));
        }

        private class FakeCurrentUser : ICurrentUser
        {
            public FakeCurrentUser(Guid? id)
            {
                Id = id;
            }

            public Guid? Id { get; }
            public string UserName => Id.HasValue ? $"user_{Id}" : string.Empty;
            public string? Email => null;
            public bool IsAuthenticated => Id.HasValue;
            public string? PhoneNumber => null;
            public bool PhoneNumberVerified => false;
            public bool EmailVerified => false;
            public IReadOnlyList<System.Security.Claims.Claim> Claims => Array.Empty<System.Security.Claims.Claim>();
            public Guid? TenantId => null;
            public Guid? ImpersonatorTenantId => null;
            public Guid? ImpersonatorUserId => null;
            public string? SurName => null;
            public string? Name => null;
            public string[] Roles => Array.Empty<string>();

            public System.Security.Claims.Claim? FindClaim(string claimType) => null;
            public System.Security.Claims.Claim[] FindClaims(string claimType) => Array.Empty<System.Security.Claims.Claim>();
            public System.Security.Claims.Claim[] GetAllClaims() => Array.Empty<System.Security.Claims.Claim>();
            public bool IsInRole(string roleName) => false;
        }
    }
}

