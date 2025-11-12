using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelBuddy.Destinations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;
using Xunit;

namespace TravelBuddy.Destinations
{
    public class DestinationIntegration_Tests
    {
        // 1) Debe devolver solo las calificaciones del usuario actual
        [Fact]
        public async Task Should_Return_Only_Current_User_Ratings()
        {
            var userA = Guid.NewGuid();
            var userB = Guid.NewGuid();

            var ratings = new List<DestinationRating>
            {
                new DestinationRating(Guid.NewGuid(), Guid.NewGuid(), 5, "Excelente", userA),
                new DestinationRating(Guid.NewGuid(), Guid.NewGuid(), 2, "Malo",      userB),
                new DestinationRating(Guid.NewGuid(), Guid.NewGuid(), 4, "Muy lindo", userA)
            };

            var ratingRepoMock = new Mock<IRepository<DestinationRating, Guid>>();
            ratingRepoMock
                .Setup(r => r.GetListAsync(It.IsAny<bool>(), default))
                .ReturnsAsync(ratings);

            var destinationRepoMock = new Mock<IRepository<Destination, Guid>>();

            var objectMapperMock = new Mock<IObjectMapper>();
            objectMapperMock
                .Setup(m => m.Map<List<DestinationRating>, List<DestinationRatingDto>>(It.IsAny<List<DestinationRating>>()))
                .Returns<List<DestinationRating>>(list =>
                    list.Select(r => new DestinationRatingDto
                    {
                        DestinationId = r.DestinationId,
                        Calificacion = r.Calificacion,
                        Comentario = r.Comentario,
                        UserId = r.UserId
                    }).ToList()
                );

            var fakeUser = new FakeCurrentUser(userA);

            var appService = new DestinationAppService(
                destinationRepoMock.Object,
                ratingRepoMock.Object,
                fakeUser,
                objectMapperMock.Object
            );

            var result = await appService.GetMyRatingsAsync();

            result.ShouldNotBeNull();
            result.Count.ShouldBe(2);
            result.All(r => r.UserId == userA).ShouldBeTrue();
        }

        // 2) Debe lanzar excepción si el usuario NO está autenticado
        [Fact]
        public async Task Should_Throw_Exception_When_User_Not_Authenticated()
        {
            var destinationRepoMock = new Mock<IRepository<Destination, Guid>>();
            var ratingRepoMock = new Mock<IRepository<DestinationRating, Guid>>();
            var objectMapperMock = new Mock<IObjectMapper>();

            var unauthenticatedUser = new FakeCurrentUser(null);

            var appService = new DestinationAppService(
                destinationRepoMock.Object,
                ratingRepoMock.Object,
                unauthenticatedUser,
                objectMapperMock.Object
            );

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => appService.GetMyRatingsAsync());
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
