using Moq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TravelBuddy.Destinations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Xunit;

namespace TravelBuddy.Destinations
{
    public class DestinationAppService_Tests
    {
        private readonly Mock<IRepository<Destination, Guid>> _destinationRepoMock;
        private readonly Mock<IRepository<DestinationRating, Guid>> _ratingRepoMock;
        private readonly DestinationAppService _appService;
        private readonly Mock<ICurrentUser> _currentUserMock;

        public DestinationAppService_Tests()
        {
           
            _destinationRepoMock = new Mock<IRepository<Destination, Guid>>();
            _ratingRepoMock = new Mock<IRepository<DestinationRating, Guid>>();
            _currentUserMock = new Mock<ICurrentUser>();

            _currentUserMock.Setup(u => u.Id).Returns(Guid.NewGuid());
            _currentUserMock.Setup(u => u.UserName).Returns("marta");
            _appService = new DestinationAppService(_destinationRepoMock.Object, _ratingRepoMock.Object, _currentUserMock.Object);


          
        }
        [Fact]
        public async Task Should_Update_Rating_If_Already_Exists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _currentUserMock.Setup(u => u.Id).Returns(userId);

            var destinationId = Guid.NewGuid();

            var existingRating = new DestinationRating
            {
                DestinationId = destinationId,
                UserId = userId,
                Calificacion = 3,
                Comentario = "Bueno"
            };

            // ✅ Mock: simulamos que GetQueryableAsync() devuelve el rating existente
            var ratings = new List<DestinationRating> { existingRating }.AsQueryable();
            _ratingRepoMock
                .Setup(r => r.GetQueryableAsync())
                .ReturnsAsync(ratings);

           


            var input = new CreateDestinationRatingDto
            {
                DestinationId = destinationId,
                Calificacion = 5,
                Comentario = "Excelente"
            };

            // Act
            var result = await _appService.RateDestinationAsync(input);

            // Assert
            Assert.Equal(5, existingRating.Calificacion);
            Assert.Equal("Excelente", existingRating.Comentario);
            _ratingRepoMock
    .Setup(r => r.UpdateAsync(It.IsAny<DestinationRating>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
    .ReturnsAsync((DestinationRating dr, bool _, CancellationToken __) => dr);

        }


        [Fact]
        public async Task Should_Throw_Exception_For_Invalid_Rating()
        {
            var input = new CreateDestinationRatingDto
            {
                DestinationId = Guid.NewGuid(),
                Calificacion = 12 // fuera de rango
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _appService.RateDestinationAsync(input));
        }
    }
}
