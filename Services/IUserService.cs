using System.Collections.Generic;
using System.Threading.Tasks;
using PoolGameServer.Entities;
using PoolServer.Models;

namespace PoolServer.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(TokenRequest tokenRequest);
        Task<User> Register(RegisterRequest registerRequest);
    }
}