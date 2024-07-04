using API.BO.DTOs.Voucher;
using API.BO.Models;
using API.Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ElectronicStoreAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;
        private readonly IMapper _mapper;
        public VoucherController(IVoucherService voucherService, IMapper mapper)
        {
            _voucherService = voucherService;
            _mapper = mapper;
        }

        [HttpGet("Voucher/get-all")]
        public async Task<IActionResult> GetAllVoucher()
        {
            var result = await _voucherService.GetAllVouchers();
            var response = _mapper.Map<List<VoucherDTO>>(result);
            return Ok(response);
        }
        [HttpGet("Voucher/get-voucher/{id}")]
        public async Task<IActionResult> GetVoucher(string id)
        {
            var result = await _voucherService.GetVoucher(id);
            var response = _mapper.Map<VoucherDTO>(result);
            return Ok(response);
        }
        [HttpPost("Voucher/create/{accountId}")]
        public async Task<IActionResult> CreateVoucher(string accountId, [FromBody] VoucherCreateDTO createForm)
        {
            var mappedCreate = _mapper.Map<Voucher>(createForm);
            mappedCreate.AccountId = accountId;
            //mappedCreate.AccountId = ObjectId.Parse(accountId);
            var result = await _voucherService.AddVoucher(mappedCreate);
            if(result == "") return Ok();
            else return BadRequest(result);
        }
        [HttpPut("Voucher/update/{id}")]
        public async Task<IActionResult> UpdateVoucher(string id, [FromBody] VoucherUpdateDTO updateForm)
        {
            var mappedUpdate = _mapper.Map<Voucher>(updateForm);
            mappedUpdate.VoucherId = id;
            var result = await _voucherService.UpdateVoucher(mappedUpdate);
            if (result == "") return Ok();
            else return BadRequest(result);
        }
        [HttpPut("/disable-voucher/{id}")]
        public async Task<IActionResult> DisableVoucher(string id)
        {
            var result = await _voucherService.DisableVoucher(id);
            if (result) return Ok();
            else return BadRequest();
        }
    }
}
