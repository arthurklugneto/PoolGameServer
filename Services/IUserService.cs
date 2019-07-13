using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using PoolGameServer.Entities;
using PoolServer.Models;

namespace PoolServer.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(TokenRequest tokenRequest);
        Task<User> Register(RegisterRequest registerRequest);
        bool Validate(string token);

    }
}