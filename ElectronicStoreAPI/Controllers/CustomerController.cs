﻿using API.BO.DTOs.Account;
using API.Service.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicStoreAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public CustomerController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO != null) {
                var response = await _accountService.Login(loginDTO.Input, loginDTO.Password);
                return Ok(response);
            }
            return NotFound();
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (registerDTO != null)
            {
                var response = await _accountService.Register(registerDTO.Email, registerDTO.Password, "customer");
                return Ok(response);
            }
            return NotFound();
        }
        [Route("/get-all")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _accountService.GetAccounts();
            var response = _mapper.Map<List<AccountDTO>>(result);
            return Ok(response);
        }
        [HttpGet("/get-profile-by-email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var result = await _accountService.GetByEmail(email);
            var response = _mapper.Map<AccountDTO>(result);
            return Ok(response);
        }
        [HttpGet("/get-profile-by-id/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _accountService.GetById(id);
            var response = _mapper.Map<AccountDTO>(result);
            return Ok(response);
        }

        [HttpPut("/update-profile/{id}")]
        public async Task<IActionResult> UpdateProfile(string id, [FromBody] AccountUpdateDTO updateDTO)
        {
            var result = await _accountService.UpdateProfile(id, updateDTO);
            var response = _mapper.Map<AccountDTO>(result);
            return Ok(response);
        }
    }
}
