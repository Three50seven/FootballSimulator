using FootballSimulator.Core.DTOs;
using FootballSimulator.Core.Interfaces;

namespace FootballSimulator.Application.Services
{
    public class StadiumManageModelService : IStadiumManageModelService
    {
        private readonly IStadiumRepository _stadiumRepository;

        public StadiumManageModelService(IStadiumRepository stadiumRepository)
        {
            _stadiumRepository = stadiumRepository;
        }

        public async Task<StadiumManageModel> BuildModelAsync()
        {
            var model = new StadiumManageModel();

            var stadiums = await _stadiumRepository.GetAllAsync();
            model.Results = [.. stadiums.Select(s => new StadiumSearchListItem
            {
                Id = s.Id,
                Guid = s.Guid,
                Name = s.Name,
                Capacity = s.Capacity,
                CityName = s.City?.Name,
                StadiumTypeName = s.StadiumType?.Name,
                ClimateTypeName = s.ClimateType?.Name
            })];

            await Task.CompletedTask;

            return model;
        }
    }
}
