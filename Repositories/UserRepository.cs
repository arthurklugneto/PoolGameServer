using PoolGameServer.Entities;
using PoolGameServer.Persistence;

namespace PoolGameServer.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
         
    }
    public class UserRepository : BaseRepository<User> , IUserRepository
    {
        public UserRepository(IMongoContext context) : base(context)
        {
            
        }
    }
}