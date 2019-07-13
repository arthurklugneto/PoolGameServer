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
using PoolServer.Entities;
using PoolServer.Models;

namespace PoolServer.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(TokenRequest tokenRequest);
        Task<User> Register(RegisterRequest registerRequest);
        bool Validate(string token);

        // MOVA ISSO PARA UM FACADE!!!
        Task<User> GetUserDetails(string UserId);

    }

    public class UserService : IUserService
    {
        private readonly JWTSettings _jwtSettings;
        private readonly UserRegisterInitialValues _userRegisterInitialValues;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICryptographyService _cryptographyService;

        public UserService(IOptions<JWTSettings> jwtSettings,
                           IOptions<UserRegisterInitialValues> userRegisterInitialValues,
                           IUserRepository userRepository,
                           ICryptographyService cryptographyService,
                           IUnitOfWork unitOfWork)
        {
            _jwtSettings = jwtSettings.Value;
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
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                IssuedAt = DateTime.UtcNow , 
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ValidityMinutes),
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
                },
                Experience = new UserExperience(){
                    Level = 1,
                    Experience = 0
                }
            };

            _userRepository.Add(user);
            await _unitOfWork.Commit();
            return user;
        }

        public bool Validate(string token){
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var tokenValidator = GetValidationParameters();
            SecurityToken validatedToken = null;
            try
            {
                var valid = tokenHandler.ValidateToken(stripBearerFromToken(token),tokenValidator,out validatedToken);
                return valid.Identity.IsAuthenticated;    
            }
            catch (System.Exception)
            {
                return false;
            }            
        }

        private string stripBearerFromToken(string token){
            return token.Replace("bearer ","");
        }

        private string getEncryptedPasswordForTokenRequest(TokenRequest tokenRequest){
            return tokenRequest.Password == null ? null : _cryptographyService.GenerateSHA512String(tokenRequest.Password);
        }

        private string generateEncryptedPasswordFromRegisterRequest(RegisterRequest registerRequest){
            return registerRequest.Password == null ? null : _cryptographyService.GenerateSHA512String(registerRequest.Password);
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = true, // Because there is no audiance in the generated token
                ValidateIssuer = true,   // Because there is no issuer in the generated token
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)) // The same key as the one that generate the token
            };
        }

        /*
            TODO : ADD THIS TO FACADE
         */
        public async Task<User> GetUserDetails(string UserId)
        {
            var user = await _userRepository.GetById(UserId);
            return user;
        }
    }
}