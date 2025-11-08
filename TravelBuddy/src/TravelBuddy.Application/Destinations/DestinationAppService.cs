using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelBuddy.Destinations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;
using Volo.Abp.Users;

namespace TravelBuddy.Destinations
{
    // [Authorize]
    [RemoteService(false)]
    public class DestinationAppService :
        CrudAppService<Destination, DestinationDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateDestinationDto>,
        IDestinationAppService
    {
        private readonly IRepository<DestinationRating, Guid> _ratingRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<Destination, Guid> _destinationRepository;

        public DestinationAppService(
            IRepository<Destination, Guid> destinationRepository,
            IRepository<DestinationRating, Guid> ratingRepository,
            ICurrentUser currentUser)
            : base(destinationRepository)
        {
            _ratingRepository = ratingRepository;
            _currentUser = currentUser;
            _destinationRepository = destinationRepository;
        }

        public async Task<List<DestinationRatingDto>> GetMyRatingsAsync()
        {
            if (_currentUser?.Id == null)
                throw new UnauthorizedAccessException("Usuario no autenticado");

            var allRatings = await _ratingRepository.GetListAsync();
            var myRatings = allRatings.Where(r => r.UserId == _currentUser.Id).ToList();

            return ObjectMapper.Map<List<DestinationRating>, List<DestinationRatingDto>>(myRatings);
        }

        public async Task<DestinationRatingDto> RateDestinationAsync(CreateDestinationRatingDto input)
        {
            if (_currentUser?.Id == null)
                throw new UnauthorizedAccessException("Usuario no autenticado");

            if (input.Calificacion < 1 || input.Calificacion > 10)
                throw new ArgumentException("La calificación debe estar entre 1 y 10");

            var existing = await _ratingRepository.FirstOrDefaultAsync(
                r => r.DestinationId == input.DestinationId && r.UserId == _currentUser.Id.Value);

            if (existing != null)
            {
                existing.Calificacion = input.Calificacion;
                existing.Comentario = input.Comentario;

                await _ratingRepository.UpdateAsync(existing, true);
                return ObjectMapper.Map<DestinationRating, DestinationRatingDto>(existing);
            }

            // Nueva calificación
            var rating = new DestinationRating
            {
                DestinationId = input.DestinationId,
                Calificacion = input.Calificacion,
                Comentario = input.Comentario,
                UserId = _currentUser.Id.Value
            };

            await _ratingRepository.InsertAsync(rating, true);
            return ObjectMapper.Map<DestinationRating, DestinationRatingDto>(rating);
        }
    }
}
