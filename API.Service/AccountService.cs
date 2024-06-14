using API.BO.DTOs;
using API.BO.Models;
using API.Repository.Interfaces;
using API.Repository.Repositories;
using API.Service.Interface;
using Helper;

namespace API.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenService _tokenService;
        public AccountService(IAccountRepository accountRepository, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _accountRepository = accountRepository;
        }

        public async Task<LoginResponseDTO> Login(string input, string password)
        {
            var login = (await _accountRepository.GetByCondition(p => p.Password == password && (RegexUtil.IsEmail(input) ? p.Email == input : p.PhoneNumber == input))).FirstOrDefault();
            if(login == null)
            {
                throw new Exception();
            }
            string jwtToken;
            jwtToken = _tokenService.CreateToken(login);
            return new LoginResponseDTO()
            {
                AccessToken = jwtToken,
            };
        }
        public async Task<LoginResponseDTO> Register(string email, string password)
        {
            Random rnd = new Random();
            Account account = new Account()
            {
                AccountId = rnd.Next(1,20),
                Email = email,
                Password = password
            };
            var flag = await _accountRepository.Add(account);
            if (flag)
            {
                return await Login(email, password);
            }
            else throw new Exception();
        }
        public async Task<Account> GetUserInformation(int id)
        {
            var account = (await _accountRepository.GetByCondition(p => p.AccountId == id)).FirstOrDefault();
            if (account != null) { return account; }
            else throw new Exception();
        }
    }
}
