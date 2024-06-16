using API.BO.DTOs.Combo;
using API.Service.Interfaces;
using ElectronicStoreAPI.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicStoreAPI.Controllers
{
    [ApiController]
    public class ComboController : ControllerBase
    {
        private readonly IComboService _comboService;
        public ComboController(IComboService comboService)
        {
            _comboService = comboService;
        }

        [HttpGet(ApiEndpointConstant.Combo.CombosEndpoint)]
        public async Task<IActionResult> GetAllCombos()
        {
            var result = await _comboService.GetAllCombo();
            return Ok(result);
        }

        [HttpGet(ApiEndpointConstant.Combo.AvailableCombosEndpoint)]
        public async Task<IActionResult> GetAllAvailableCombos()
        {
            var result = await _comboService.GetAllAvailableCombo();
            return Ok(result);
        }

        [HttpPost(ApiEndpointConstant.Combo.CombosEndpoint)]
        public async Task<IActionResult> CreateCombo(CreateComboDTO combo)
        {
            await _comboService.CreateCombo(combo);
            return Ok("Action success");
        }

        [HttpPatch(ApiEndpointConstant.Combo.ComboEndpoint)]
        public async Task<IActionResult> UpdateCombo(string id, ComboDTO combo)
        {
            await _comboService.UpdateCombo(id, combo);
            return Ok();
        }

        [HttpPatch(ApiEndpointConstant.Combo.ComboStatusEndpoint)]
        public async Task<IActionResult> ChangeComboStatus(string id)
        {
            await _comboService.ChangeComboStatus(id);
            return Ok();
        }

        [HttpDelete(ApiEndpointConstant.Combo.ComboEndpoint)]
        public async Task<IActionResult> DeleteCombo(string id)
        {
            await _comboService.DeleteCombo(id);
            return Ok();
        }
    }
}
