using API.BO.DTOs.Account;
using API.BO.Models.Enum;
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

        public async Task<LoginResponseDTO> Login(LoginDTO loginDTO)
        {
            Expression<Func<Account, object>> filter = RegexUtil.IsEmail(loginDTO.Input) ? p => p.Email : p => p.PhoneNumber;
            var login = (await _accountRepository.GetByCondition( 1, 10, (filter, loginDTO.Input), (p => p.Password, loginDTO.Password))).FirstOrDefault();
            if (login == null)
            {
                throw new Exception("Wrong email or password");
            }
            string jwtToken;
            jwtToken = _tokenService.CreateToken(login);
            return new LoginResponseDTO()
            {
                AccessToken = jwtToken,
            };
        }
        public async Task<LoginResponseDTO> Register(RegisterDTO registerDTO)
        {
            try
            {
                bool checkedExist = (await _accountRepository.GetByCondition(filters: (p => p.Email, registerDTO.Email))).Any();
                if (!checkedExist)
                {
                    Account account = new Account()
                    {
                        Email = registerDTO.Email,
                        FirstName = registerDTO.FirstName,
                        LastName = registerDTO.LastName,
                        Address = registerDTO.Address,
                        PhoneNumber = registerDTO.PhoneNumber,
                        Password = registerDTO.Password,
                        Role = AccountRoleEnum.CUSTOMER
                    };
                    var flag = await _accountRepository.Add(account);
                    if (flag)
                    {
                        var registered = (await _accountRepository.GetByCondition(null, null, (p => p.Password, registerDTO.Password),(p => p.Email, registerDTO.Email))).FirstOrDefault();
                        string jwtToken;
                        jwtToken = _tokenService.CreateToken(registered);
                        return new LoginResponseDTO()
                        {
                            AccessToken = jwtToken,
                        };
                    }
                    else throw new Exception("The account was not added");
                }
                else
                {
                    throw new Exception("This email has already being used!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Account> GetUserInformation(string id)
        {
            var account = (await _accountRepository.GetByCondition(filters: (p => p.AccountId, id))).FirstOrDefault();
            if (account != null) { return account; }
            else throw new Exception();
        }

        public async Task<List<Account>> GetAccounts()
        {
            return (await _accountRepository.GetAll()).ToList();
        }
        public async Task<Account> GetByEmail(string email)
        {
            return (await _accountRepository.GetByCondition(filters: (p => p.Email, email))).FirstOrDefault();
        }
        public async Task<Account> GetById(string id)
        {
            return (await _accountRepository.GetByCondition(filters: (p => p.AccountId, id))).FirstOrDefault();
        }
        public async Task<string> UpdateProfile(string id, AccountUpdateDTO updateDTO)
        {
            try
            {
                var result = (await _accountRepository.GetByCondition(filters: (p => p.AccountId, id))).FirstOrDefault();
                if (result == null) throw new Exception("Can't find account");
                result.Address = updateDTO.Address;
                result.FirstName = updateDTO.FirstName;
                result.LastName = updateDTO.LastName;
                result.PhoneNumber = updateDTO.PhoneNumber;
                await _accountRepository.Update(result);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> ChangePassword(string id,ChangePassDTO changePassDTO)
        {
            try
            {
                var result = (await _accountRepository.GetByCondition(filters: (p => p.AccountId, id))).FirstOrDefault();
                if (result == null) throw new Exception("Can't find account");
                if (result.Password != changePassDTO.oldPassword) throw new Exception("Incorrect password");
                result.Password = changePassDTO.newPassword;
                await _accountRepository.Update(result);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> ChangeStatus(string id)
        {
            try
            {
                var result = (await _accountRepository.GetByCondition(filters: (p => p.AccountId, id))).FirstOrDefault();
                if (result == null) throw new Exception("Can't find account");
                if (result.Status == AccountStatusEnum.ACTIVATED)
                    result.Status = AccountStatusEnum.DEACTIVATED;
                else
                    result.Status = AccountStatusEnum.ACTIVATED;
                await _accountRepository.Update(result);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
