using System;
using System.Threading.Tasks;

namespace PoolGameServer.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
}