using PoolGameServer.Entities;
using PoolGameServer.Persistence;

namespace PoolGameServer.Repositories
{
    public interface ICountryRepository : IRepository<Country>
    {
        
    }
    public class CountryRepository : BaseRepository<Country> , ICountryRepository
    {
        public CountryRepository(IMongoContext context) : base(context)
        {
            
        }
    }
}