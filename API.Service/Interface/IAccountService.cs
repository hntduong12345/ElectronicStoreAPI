using API.BO.DTOs;
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
        Task<LoginResponseDTO> Register(string email, string password);
        Task<Account> GetUserInformation(int id);
    }
}
