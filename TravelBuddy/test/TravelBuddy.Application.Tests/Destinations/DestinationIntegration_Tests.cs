using Autofac.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBuddy.Destinations;
using Volo.Abp.Users;
using Volo.Abp.Testing;
using Xunit;


namespace TravelBuddy.Destinations
{
    public class DestinationIntegration_Tests : TravelBuddyTestBase<TravelBuddyEntityFrameworkCoreTestModule>

    {
        private readonly IDestinationAppService _appService;
        private readonly ICurrentUser _currentUser;

        public DestinationIntegration_Tests()
        {
            _appService = GetRequiredService<IDestinationAppService>();
            _currentUser = GetRequiredService<ICurrentUser>();
        }

        [Fact]
        public async Task Should_Return_Only_Current_User_Ratings()
        {
            // Arrange: Crea dos usuarios diferentes
            var userA = Guid.NewGuid();
            var userB = Guid.NewGuid();

            //Inyecta caliicaciones en el repositorio InMemory
            var ratingRepo = GetRequiredService<Volo.Abp.Domain.Repositories.IRepository<DestinationRating, Guid>>();

            await ratingRepo.InsertAsync(new DestinationRating(Guid.NewGuid(), Guid.NewGuid(), 5, "Excelente", userA));
            await ratingRepo.InsertAsync(new DestinationRating(Guid.NewGuid(), Guid.NewGuid(), 2, "Malo", userB));
            await ratingRepo.InsertAsync(new DestinationRating(Guid.NewGuid(), Guid.NewGuid(), 4, "Muy lindo", userA));

            // Simula que el usuario actual es userA
            ReplaceCurrentUser(userA);

            //Act
            var result = await _appService.GetMyRatingsAsync();

            //Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(2); //solo los de userA
            result.All(r => r.UserId == userA).ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Throw_Exception_When_User_Not_Authenticated()
        {
            //Arrange
            ReplaceCurrentUser(null);

            //Act & Assert
            await Should.ThrowAsync<UnauthorizedAccessException>(
              () => _appService.GetMyRatingsAsync());
        }

        private void ReplaceCurrentUser(Guid? userId)
        {
            var fakeUser = new FakeCurrentUser(userId);
            //Usando IServiceCollection directamente y el contenedor que ya tiene
            var serviceCollection = GetRequiredService<IServiceCollection>();
            serviceCollection.Replace(ServiceDescriptor.Singleton<ICurrentUser>(fakeUser));
        }

        private class FakeCurrentUser : ICurrentUser
        {
            public FakeCurrentUser(Guid? userId)
            {
                Id = userId;
            }
            public Guid? Id { get; }
            public string UserName => Id.HasValue ? $"user_{Id}" : null;
            public string Name => " ";
            public string? SurName => null;
            public bool IsAuthenticated => Id.HasValue;
            public string? PhoneNumber => null;
            public bool PhoneNumberVerified => false;
            public string? Email => null;
            public bool EmailVerified => false;
            public Guid? TenantId => null;
            public string[] Roles => Array.Empty<string>();
            public System.Security.Claims.Claim? FindClaim(string claimType) => null;
            public System.Security.Claims.Claim[] FindClaims(string claimType) => Array.Empty<System.Security.Claims.Claim>();
            public System.Security.Claims.Claim[] GetAllClaims() => Array.Empty<System.Security.Claims.Claim>();
            public bool IsInRole(string roleName) => false;
        }
    }
}
