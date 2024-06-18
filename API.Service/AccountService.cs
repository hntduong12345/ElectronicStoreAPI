using API.BO.DTOs;
using API.BO.Models;
using API.Repository.Interfaces;
using API.Repository.Repositories;
using API.Service.Interface;
using DnsClient;
using Helper;
using MongoDB.Bson;

namespace API.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenService _tokenService;
        public AccountService(IAccountRepository accountRepository, ITokenService tokenService)
        {
            _accountRepository = accountRepository;
            _tokenService = tokenService;
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
        public async Task<LoginResponseDTO> Register(string email, string password, string role)
        {
            try
            {
                bool checkedExist = (await _accountRepository.GetByCondition(p => p.Password == password && p.Email == email)).Any();
                if (!checkedExist)
                {
                    Account account = new Account()
                    {
                        Email = email,
                        Password = password,
                        Role = role
                    };
                    var flag = await _accountRepository.Add(account);
                    if (flag)
                    {
                        var registered = (await _accountRepository.GetByCondition(p => p.Password == password && p.Email == email)).FirstOrDefault();
                        string jwtToken;
                        jwtToken = _tokenService.CreateToken(registered);
                        return new LoginResponseDTO()
                        {
                            AccessToken = jwtToken,
                        };
                    }
                    else throw new Exception();
                }
                else
                {
                    throw new Exception("This email has already being used!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<Account> GetUserInformation(string id)
        {
            var account = (await _accountRepository.GetByCondition(p => p.AccountId == id)).FirstOrDefault();
            if (account != null) { return account; }
            else throw new Exception();
        }

        public async Task<List<Account>> GetAccounts()
        {
            return (await _accountRepository.GetAll()).ToList();
        }
        public async Task<Account> GetByEmail(string email)
        {
            return (await _accountRepository.GetByCondition(p => p.Email == email)).FirstOrDefault();
        }
    }
}
