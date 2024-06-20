using API.BO.DTOs.Account;
using API.BO.Models;
using API.Repository.Interfaces;
using API.Repository.Repositories;
using API.Service.Interfaces;
using DnsClient;
using Helper;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace API.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenService _tokenService;
        private readonly IOrderService _orderService;
        public AccountService(IAccountRepository accountRepository, ITokenService tokenService, IOrderService orderService)
        {
            _accountRepository = accountRepository;
            _tokenService = tokenService;
            _orderService = orderService;
        }

        public async Task<LoginResponseDTO> Login(string input, string password)
        {
            Expression<Func<Account, object>> filter = RegexUtil.IsEmail(input) ? p => p.Email : p => p.PhoneNumber;
            var login = (await _accountRepository.GetByCondition( 1, 10, (filter,input), (p => p.Password, password))).FirstOrDefault();
            if (login == null)
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
                bool checkedExist = (await _accountRepository.GetByCondition(1, 10, (p => p.Email, email))).Any();
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
                        var registered = (await _accountRepository.GetByCondition(1, 10, (p => p.Password, password),(p => p.Email, email))).FirstOrDefault();
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
            var account = (await _accountRepository.GetByCondition(1, 10, (p => p.AccountId, id))).FirstOrDefault();
            if (account != null) { return account; }
            else throw new Exception();
        }

        public async Task<List<Account>> GetAccounts()
        {
            return (await _accountRepository.GetAll()).ToList();
        }
        public async Task<Account> GetByEmail(string email)
        {
            return (await _accountRepository.GetByCondition(1, 10, (p => p.Email, email))).FirstOrDefault();
        }
        public async Task<Account> GetById(string id)
        {
            return (await _accountRepository.GetByCondition(1, 10, (p => p.AccountId, id))).FirstOrDefault();
        }
        public async Task<bool> UpdateProfile(string id, AccountUpdateDTO updateDTO)
        {
            try
            {
                var result = (await _accountRepository.GetByCondition(1, 10, (p => p.AccountId, id))).FirstOrDefault();
                result.Address = updateDTO.Address;
                result.FirstName = updateDTO.FirstName;
                result.LastName = updateDTO.LastName;
                result.PhoneNumber = updateDTO.PhoneNumber;
                await _accountRepository.Update(result);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
