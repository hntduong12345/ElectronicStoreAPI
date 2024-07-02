using API.BO.DTOs.Combo;
using API.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Service.Interfaces
{
    public interface IComboService
    {
        public Task<List<Combo>> GetAllCombo();
        public Task<List<Combo>> GetAllAvailableCombo();
        public Task<GetComboDTO> GetComboById(string id);
        public Task CreateCombo(CreateComboDTO combo);
        public Task UpdateCombo(string id, ComboDTO combo);
        public Task ChangeComboStatus(string id);
        public Task DeleteCombo(string id);
    }
}
