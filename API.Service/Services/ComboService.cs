using API.BO.DTOs.Combo;
using API.BO.Models;
using API.Repository.Interfaces;
using API.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Service.Services
{
    public class ComboService : IComboService
    {
        private readonly IComboRepository _comboRepository;

        public ComboService(IComboRepository comboRepository)
        {
            _comboRepository = comboRepository;
        }

        public async Task ChangeComboStatus(int id)
        {
            await _comboRepository.ChangeComboStatus(id);
        }

        public async Task CreateCombo(CreateComboDTO combo)
        {
            await _comboRepository.CreateCombo(combo);
        }

        public async Task DeleteCombo(int id)
        {
            await _comboRepository.DeleteCombo(id);
        }

        public async Task<List<Combo>> GetAllAvailableCombo()
        {
            return await _comboRepository.GetAllAvailableCombo();
        }

        public async Task<List<Combo>> GetAllCombo()
        {
            return await _comboRepository.GetAllCombo();
        }

        public async Task<Combo> GetComboById(int id)
        {
            return await _comboRepository.GetComboById(id);
        }

        public async Task UpdateCombo(int id, ComboDTO combo)
        {
            await _comboRepository.UpdateCombo(id, combo);
        }
    }
}
