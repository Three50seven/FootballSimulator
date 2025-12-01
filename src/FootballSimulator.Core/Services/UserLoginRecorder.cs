using Common.Core;
using FootballSimulator.Core.Domain;
using FootballSimulator.Core.Interfaces;

namespace FootballSimulator.Core.Services
{
    public class UserLoginRecorder : IUserLoginRecorder
    {
        private readonly ICommandRepository<UserLoginHistory, int> _userLoginHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserLoginRecorder(ICommandRepository<UserLoginHistory, int> userLoginHistoryRepository,
            IUnitOfWork unitOfWork)
        {
            _userLoginHistoryRepository = userLoginHistoryRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task RecordAsync(int userId, string ipAddress)
        {
            await _userLoginHistoryRepository.AddOrUpdateAsync(new UserLoginHistory(userId, ipAddress));
            await _unitOfWork.SaveAsync();
        }
    }
}
