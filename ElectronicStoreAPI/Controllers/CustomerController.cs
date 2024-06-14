using API.BO.DTOs;
using API.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicStoreAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public CustomerController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (registerDTO != null)
            {
                var response = await _accountService.Register(registerDTO.Email, registerDTO.Password);
                return Ok(response);
            }
            return NotFound();
        }
    }
}
