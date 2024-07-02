using API.BO.DTOs.Account;
using API.Service.Interfaces;
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
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public CustomerController(IAccountService accountService, IMapper mapper, IOrderService orderService)
        {
            _accountService = accountService;
            _mapper = mapper;
            _orderService = orderService;
        }
        [HttpPost("customer/login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO != null) {
                var response = await _accountService.Login(loginDTO);

                return Ok(response);
            }
            return NotFound();
        }


        [HttpPost("customer/register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (registerDTO != null)
            {
                var response = await _accountService.Register(registerDTO);
                return Ok(response);
            }
            return NotFound();
        }

        [HttpGet("customer/get-profile-by-email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var result = await _accountService.GetByEmail(email);
            var response = _mapper.Map<AccountDTO>(result);
            return Ok(response);
        }
        [HttpGet("customer/get-profile-by-id/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _accountService.GetById(id);
            var response = _mapper.Map<AccountDTO>(result);
            return Ok(response);
        }

        [HttpPut("customer/update-profile/{id}")]
        public async Task<IActionResult> UpdateProfile(string id, [FromBody] AccountUpdateDTO updateDTO)
        {
            var result = await _accountService.UpdateProfile(id, updateDTO);
            if (String.IsNullOrEmpty(result)) return Ok();
            else return BadRequest(result);
        }
        [HttpPut("customer/change-password/{id}")]
        public async Task<IActionResult> ChangePass(string id, ChangePassDTO changePassDTO)
        {
            var result = await _accountService.ChangePassword(id, changePassDTO);
            if (String.IsNullOrEmpty(result)) return Ok();
            else return BadRequest(result);
        }
    }
}
