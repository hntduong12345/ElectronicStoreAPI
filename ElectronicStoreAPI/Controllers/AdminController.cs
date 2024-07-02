using API.BO.DTOs.Account;
using API.BO.Models;
using API.Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicStoreAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AdminController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        [Route("admin/get-all-user")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _accountService.GetAccounts();
            var response = _mapper.Map<List<AccountDTO>>(result);
            return Ok(response);
        }
        [Route("admin/get-all-admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllAdmin()
        {
            var result = await _accountService.GetAccountsByAdminRole();
            var response = _mapper.Map<List<AccountDTO>>(result);
            return Ok(response);
        }
        [Route("admin/get-all-staff")]
        [HttpGet]
        public async Task<IActionResult> GetAllStaff()
        {
            var result = await _accountService.GetAccountsByStaffRole();
            var response = _mapper.Map<List<AccountDTO>>(result);
            return Ok(response);
        }
   
        [HttpPost("admin/create-admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] AccountAdminDTO accountDTO)
        {
            var result = await _accountService.CreateAdminAccount(accountDTO);
            if (String.IsNullOrEmpty(result)) return Ok();
            else return BadRequest(result);
        }
        [HttpPost("admin/create-staff")]
        public async Task<IActionResult> CreateStaff([FromBody] AccountAdminDTO accountDTO)
        {
            var result = await _accountService.CreateStaffAccount(accountDTO);
            if (String.IsNullOrEmpty(result)) return Ok();
            else return BadRequest(result);
        }
        [Route("admin/change-status/{id}")]
        [HttpPut]
        public async Task<IActionResult> ChangeStatus(string id)
        {
            var result = await _accountService.ChangeStatus(id);
            if (String.IsNullOrEmpty(result)) return Ok();
            else return BadRequest(result);
        }
    }
}
