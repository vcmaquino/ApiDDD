using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Api.Domain.Dtos;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Services.User;
using Api.Domain.Repository;
using Api.Domain.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Service.Services
{
    public class LoginService : ILoginService
    {
        private IUserRepository _reporitory;
        private SigningConfigurations _signingConfigurations;
        private TokenConfigurations _tokenConfigurations;
        private IConfiguration _configuration { get; }
        public LoginService(IUserRepository reporitory,
                            SigningConfigurations signingConfigurations,
                             TokenConfigurations tokenConfigurations,
                             IConfiguration configuration)
        {
            _reporitory = reporitory;
            _signingConfigurations = signingConfigurations;
            _tokenConfigurations = tokenConfigurations;
            _configuration = configuration;
        }
        public async Task<object> FindByLogin(LoginDto user)
        {
            var baseUser = new UserEntity();
            if (user != null && !string.IsNullOrWhiteSpace(user.Email))
            {
                baseUser = await _reporitory.FindByLogin(user.Email);
                if (baseUser == null)
                {
                    return new
                    {
                        authenticad = false,
                        message = "Falha ao autenticar"
                    };
                }
                else
                {
                    var identity = new ClaimsIdentity(
                        new GenericIdentity(baseUser.Email),
                        new[]{
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),//id do token 0
                            new Claim(JwtRegisteredClaimNames.UniqueName,baseUser.Email),
                        }
                    );
                    DateTime createDate = DateTime.Now;
                    DateTime experationDate = createDate + TimeSpan.FromSeconds(_tokenConfigurations.Seconds); // 60 segundos
                    var handler = new JwtSecurityTokenHandler();
                    var token = CreateToken(identity, createDate, experationDate, handler);
                    return SuccessObject(createDate, experationDate, token, user);
                }


            }
            else
            {
                return new
                {
                    authenticad = false,
                    message = "Falha ao autenticar"
                };
            }
        }

        private string CreateToken(ClaimsIdentity identity, DateTime createDate, DateTime experationDate,
        JwtSecurityTokenHandler handler)
        {
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = experationDate,
            });

            var token = handler.WriteToken(securityToken);
            return token;

        }
        private object SuccessObject(DateTime createDate, DateTime experationDate, string token, LoginDto user)
        {
            return new
            {
                authenticated = true,
                created = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = experationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                acessToken = token,
                userName = user.Email,
                message = "Usuário Logado com sucesso"
            };
        }
    }
}
