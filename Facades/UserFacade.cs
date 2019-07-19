using System.Threading.Tasks;
using System.Linq;
using PoolGameServer.Entities;
using PoolGameServer.Persistence;
using PoolGameServer.Repositories;
using PoolGameServer.Populators;
using System.Collections.Generic;

namespace PoolGameServer.Facades
{
    public interface IUserFacade
    {
        Task<User> GetUserDetails(string UserId);
        Task<User> AddFriend(string friendId);
        Task<User> RemoveFriend(string friendId);
        Task<List<User>> GetNearestUsers();
        Task<List<User>> GetClosestRelationFriends();
    }
    public class UserFacade : IUserFacade
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPopulator<User> _userPopulator;
        public UserFacade(IUserRepository userRepository,
                           IUnitOfWork unitOfWork,
                           IPopulator<User> userPopulator)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _userPopulator = userPopulator;
        }

        public Task<User> AddFriend(string friendId, string meId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<User>> GetClosestRelationFriends()
        {
            // TODO : MATE THIS MORE EFICIENT
            var rows = await _userRepository.GetAll();
            return rows.Take(5).ToList();
            
        }

        public async Task<List<User>> GetNearestUsers()
        {
            // TODO : MATE THIS MORE EFICIENT
            var rows = await _userRepository.GetAll();
            return rows.Take(5).ToList();
        }

        public async Task<User> GetUserDetails(string UserId)
        {
            var user = await _userRepository.GetById(UserId);
            User populatesUser = null;
            if( user != null ){
                populatesUser = _userPopulator.populate(user);
            }            
            return populatesUser;
        }

        public Task<User> RemoveFriend(string friendId)
        {
            throw new System.NotImplementedException();
        }
        
    }
}