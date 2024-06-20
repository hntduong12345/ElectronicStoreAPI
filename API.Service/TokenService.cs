using API.BO.Models;
using API.Repository.Interfaces;
using API.Service.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Service
{
    public class TokenService : ITokenService
    {
        private const int EXP_TIME = 30 * 60;
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;
        public TokenService(IAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        public string CreateToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]),
                new Claim(JwtRegisteredClaimNames.NameId, account.AccountId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim("Role", account.Role),
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);



            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddSeconds(EXP_TIME),
                signingCredentials: credential
                );

            return tokenHandler.WriteToken(token);
        }
    }
}
