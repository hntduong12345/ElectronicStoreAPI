﻿using API.BO.DTOs.Account;
using API.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Service.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResponseDTO> Login(LoginDTO loginDTO);
        Task<LoginResponseDTO> Register(RegisterDTO registerDTO);
        Task<Account> GetUserInformation(string id);
        Task<List<Account>> GetAccounts();
        Task<Account> GetByEmail(string email);
        Task<Account> GetById(string id);
        Task<string> UpdateProfile(string id, AccountUpdateDTO updateDTO);
        Task<string> ChangePassword(string id, ChangePassDTO changePassDTO);
        Task<string> ChangeStatus(string id);
    }
}
