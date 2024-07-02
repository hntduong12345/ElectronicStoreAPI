using API.BO.DTOs.Account;
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
        [Route("admin/get-all")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _accountService.GetAccounts();
            var response = _mapper.Map<List<AccountDTO>>(result);
            return Ok(response);
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
