using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PoolGameServer.Entities;
using PoolGameServer.Persistence;
using PoolGameServer.Repositories;
using PoolServer.Configurations;
using PoolServer.Models;

namespace PoolServer.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly UserRegisterInitialValues _userRegisterInitialValues;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICryptographyService _cryptographyService;

        public UserService(IOptions<AppSettings> appSettings,
                           IOptions<UserRegisterInitialValues> userRegisterInitialValues,
                           IUserRepository userRepository,
                           ICryptographyService cryptographyService,
                           IUnitOfWork unitOfWork)
        {
            _appSettings = appSettings.Value;
            _userRegisterInitialValues = userRegisterInitialValues.Value;
            _userRepository = userRepository;
            _cryptographyService = cryptographyService;
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Authenticate(TokenRequest tokenRequest)
        {
            // validate user against credentials provided
            var users = await _userRepository.GetAll();
            var user = users.ToList()
                        .Where(p => p.UserName == tokenRequest.Username && p.Password == getEncryptedPasswordForTokenRequest(tokenRequest))
                        .FirstOrDefault();

            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            // remove password before returning
            user.Password = null;

            return user;
        }

        public async Task<User> Register(RegisterRequest registerRequest)
        {
            var hashedPassword = generateEncryptedPasswordFromRegisterRequest(registerRequest);
            if( hashedPassword == null ) return null;

            var users = await _userRepository.GetAll();
            if( users.Where(p => p.UserName == registerRequest.Username
                                || p.Email == registerRequest.Email
                                ).Count() != 0 ) return null;

            var user = new User(){
                Name = registerRequest.Name,
                UserName = registerRequest.Username,
                Email = registerRequest.Email,
                Password = hashedPassword,
                Balance = new UserBalance(){
                    CoinsPool = _userRegisterInitialValues.InitialCoins,
                    StarsPool = _userRegisterInitialValues.InitialStars
                }
            };

            _userRepository.Add(user);
            await _unitOfWork.Commit();
            return user;
        }

        private string getEncryptedPasswordForTokenRequest(TokenRequest tokenRequest){
            return tokenRequest.Password == null ? null : _cryptographyService.GenerateSHA512String(tokenRequest.Password);
        }

        private string generateEncryptedPasswordFromRegisterRequest(RegisterRequest registerRequest){
            return registerRequest.Password == null ? null : _cryptographyService.GenerateSHA512String(registerRequest.Password);
        }

    }
}