using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace TravelBuddy.Destinations
{
    public class DestinationAppService :
            CrudAppService<
                Destination,
                DestinationDto,
                Guid,
                Volo.Abp.Application.Dtos.PagedAndSortedResultRequestDto,
                IDestinationAppService>
    {
        private readonly IRepository<DestinationRating, Guid> _ratingRepository;
        public DestinationAppService(IRepository<Destination, Guid> repository,
            IRepository<DestinationRating, Guid> ratingRepository)
            : base(repository)
        {
            _ratingRepository = ratingRepository;
        }
        public async Task<List<DestinationRatingDto>> GetMyRatingsAsync()
        {
            // ABP automáticamente aplica el filtro IUserOwned 
            var ratings = await _ratingRepository.GetListAsync();
            return ObjectMapper.Map<List<DestinationRating>, List<DestinationRatingDto>>(ratings);
        }
        public async Task<DestinationRatingDto> RateDestinationAsync(CreateDestinationRatingDto input)
        {
            if (CurrentUser.Id == null)
            {
                throw new UnauthorizedAccessException("Usuario no autenticado");
            }
            // Crear nueva calificación
            var rating = new DestinationRating
            {
                DestinationId = input.DestinationId,
                Calificacion = input.Calificacion,
                Comentario = input.Comentario,
                UserId = CurrentUser.Id.Value  // ASOCIAR AL USUARIO 
            };

            await _ratingRepository.InsertAsync(rating);

            return ObjectMapper.Map<DestinationRating, DestinationRatingDto>(rating);
        }
    }
}



