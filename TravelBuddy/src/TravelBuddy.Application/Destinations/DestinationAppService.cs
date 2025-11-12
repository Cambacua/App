using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelBuddy.Destinations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;
using Volo.Abp.Users;

namespace TravelBuddy.Destinations
{
    [Authorize]
    [RemoteService(false)]
    public class DestinationAppService :
        CrudAppService<Destination, DestinationDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateDestinationDto>,
        IDestinationAppService
    {
        private readonly IRepository<DestinationRating, Guid> _ratingRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<Destination, Guid> _destinationRepository;
        private readonly IObjectMapper _objectMapper;

        public DestinationAppService(
            IRepository<Destination, Guid> destinationRepository,
            IRepository<DestinationRating, Guid> ratingRepository,
            ICurrentUser currentUser,
            IObjectMapper objectMapper = null)
            : base(destinationRepository)
        {
            _ratingRepository = ratingRepository;
            _currentUser = currentUser;
            _destinationRepository = destinationRepository;
            _objectMapper = objectMapper;

        }

        public async Task<List<DestinationRatingDto>> GetMyRatingsAsync()
        {
            if (_currentUser?.Id == null)
                throw new UnauthorizedAccessException("Usuario no autenticado");

            var allRatings = await _ratingRepository.GetListAsync();
            var myRatings = allRatings.Where(r => r.UserId == _currentUser.Id).ToList();

            return _objectMapper.Map<List<DestinationRating>, List<DestinationRatingDto>>(myRatings);
        }

        public async Task<DestinationRatingDto> RateDestinationAsync(CreateDestinationRatingDto input)
        {
            if (_currentUser?.Id == null)
                throw new UnauthorizedAccessException("Usuario no autenticado");

            if (input.Calificacion < 1 || input.Calificacion > 5)
                throw new ArgumentException("La calificación debe estar entre 1 y 5");

            //var existing = await _ratingRepository.FirstOrDefaultAsync(
            //    r => r.DestinationId == input.DestinationId && r.UserId == _currentUser.Id.Value);

            var queryable = await _ratingRepository.GetQueryableAsync();
            var existing = queryable.FirstOrDefault(
                r => r.DestinationId == input.DestinationId && r.UserId == _currentUser.Id.Value);


            if (existing != null)
            {
                existing.Calificacion = input.Calificacion;
                existing.Comentario = input.Comentario;

                await _ratingRepository.UpdateAsync(existing, true);
                return _objectMapper.Map<DestinationRating, DestinationRatingDto>(existing);
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
            return _objectMapper.Map<DestinationRating, DestinationRatingDto>(rating);
        }
    }
}

