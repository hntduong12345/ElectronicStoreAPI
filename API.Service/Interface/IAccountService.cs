using API.BO.DTOs.Account;
using API.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Service.Interface
{
    public interface IAccountService
    {
        Task<LoginResponseDTO> Login(string input, string password);
        Task<LoginResponseDTO> Register(string email, string password, string role);
        Task<Account> GetUserInformation(string id);
        Task<List<Account>> GetAccounts();
        Task<Account> GetByEmail(string email);
        Task<Account> GetById(string id);
        Task<Account> UpdateProfile(string id, AccountUpdateDTO updateDTO);
    }
}
