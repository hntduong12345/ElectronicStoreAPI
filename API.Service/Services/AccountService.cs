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
using System.Data;
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
            if(login.Status == AccountStatusEnum.DEACTIVATED)
            {
                throw new Exception("This account has been deactivated");
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
                if (!RegexUtil.IsEmail(registerDTO.Email))
                    throw new Exception("Email is not in correct format");
                bool checkedExist = (await _accountRepository.GetByCondition(filters: (p => p.Email, registerDTO.Email))).Any();
                if (!checkedExist)
                {
                    Account account = new Account()
                    {
                        Email = registerDTO.Email,
                        FirstName = registerDTO.FirstName,
                        LastName = registerDTO.LastName,
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
        //Admin
        public async Task<List<Account>> GetAccountsByCustomerRole()
        {
            var list = await GetListAdmin(AccountRoleEnum.CUSTOMER);
            return list;
        }
        public async Task<List<Account>> GetAccountsByAdminRole()
        {
            var list = await GetListAdmin(AccountRoleEnum.ADMIN);
            return list;
        }

        public async Task<List<Account>> GetAccountsByStaffRole()
        {
            var list = await GetListAdmin(AccountRoleEnum.STAFF);
            return list;
        }
        
        public async Task<string> CreateAdminAccount(AccountAdminDTO accountDTO)
        {
            try
            {
                var flag = await CreateAccount(accountDTO,AccountRoleEnum.ADMIN);
                return flag;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> CreateStaffAccount(AccountAdminDTO accountDTO)
        {
            try
            {
                var flag = await CreateAccount(accountDTO, AccountRoleEnum.STAFF);
                return flag;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> UpdateAccountAdmin(string id, AccountAdminDTO accountDTO)
        {
            try
            {
                var result = (await _accountRepository.GetByCondition(filters: (p => p.AccountId, id))).FirstOrDefault();
                if (result == null) throw new Exception("Can't find account");
                result.FirstName = accountDTO.FirstName;
                result.LastName = accountDTO.LastName;
                result.Password = accountDTO.Password;
                await _accountRepository.Update(result);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> DeleteAccount(string id)
        {
            try
            {
                var result = (await _accountRepository.GetByCondition(filters: (p => p.AccountId, id))).FirstOrDefault();
                if (result == null) throw new Exception("Can't find account");
                if (result.Role == AccountRoleEnum.CUSTOMER) throw new Exception("Can't delete customer account");
                await _accountRepository.Delete(result);
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
        async Task<string> CreateAccount(AccountAdminDTO accountDTO,AccountRoleEnum role)
        {
            try
            {
                if (!RegexUtil.IsEmail(accountDTO.Email))
                    throw new Exception("Email is not in correct format");
                var result = (await _accountRepository.GetByCondition(filters: (p => p.Email, accountDTO.Email))).Any();
                if (result)
                    throw new Exception("This email has already existed an account");
                Account account = new Account()
                {
                    Email = accountDTO.Email,
                    FirstName = accountDTO.FirstName,
                    LastName = accountDTO.LastName,
                    Password = accountDTO.Password,
                    Role = role
                };
                var flag = await _accountRepository.Add(account);
                if (flag) return "";
                throw new Exception("Can't create account");
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
        async Task<List<Account>> GetListAdmin(AccountRoleEnum role)
        {
            return await _accountRepository.GetByCondition(filters: (p => p.Role, role));
        }

    }
}
