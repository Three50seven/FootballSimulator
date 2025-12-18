using Common.Core;
using Common.Core.Domain;
using Common.Core.Validation;
using FootballSimulator.Application.Models;
using FootballSimulator.Core.Domain;
using FootballSimulator.Core.Interfaces;

namespace FootballSimulator.Application.Services
{
    public class StadiumEditService : EditServiceBase<Stadium, IStadiumRepository>, IStadiumEditService
    {
        private readonly IStadiumRepository _stadiumRepository;
        private readonly IValidator<StadiumEditModel> _validator;

        public StadiumEditService(IStadiumRepository stadiumRepository, IUnitOfWork unitOfWork, IValidator<StadiumEditModel> validator)
            : base(stadiumRepository, unitOfWork)
        {
            _stadiumRepository = stadiumRepository;
            _validator = validator;
        }

        public async Task<StadiumEditModel?> BuildModel(Guid? guid)
        {
            if(!guid.HasValue)
                return new StadiumEditModel();

            var stadium = await _stadiumRepository.GetByGuidAsync(guid.Value);

            if (stadium == null || stadium.IsNew)
                return null;

            return new StadiumEditModel
            {
                Id = stadium.Id,
                Guid = stadium.Guid,
                Name = stadium.Name,
                Capacity = stadium.Capacity,
                CityId = stadium.CityId,
                StadiumTypeId = stadium.StadiumTypeId,
                ClimateTypeId = stadium.ClimateTypeId
            };
        }

        public async Task<CommandResult> DeleteAsync(Guid guid)
        {
            try
            {
                var entity = await _stadiumRepository.GetByGuidAsync(guid);

                if (entity == null)
                    throw new DataObjectNotFoundException(nameof(Stadium), guid);

                await DeleteAsync(entity);

                return CommandResult.Success();
            }
            catch (ValidationException vex)
            {
                return CommandResult.Fail(vex.BrokenRules);
            }
        }

        public Task<CommandResult> SaveAsync(StadiumEditModel model)
        {
            throw new NotImplementedException();
        }
    }
}
