using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoolGameServer.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity obj);
        Task<TEntity> GetById(string id);
        Task<IEnumerable<TEntity>> GetAll();
        Task Update(TEntity obj,string id);
        Task Remove(string id);
    }
}