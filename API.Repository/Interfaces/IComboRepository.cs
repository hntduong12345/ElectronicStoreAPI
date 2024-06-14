﻿using API.BO.DTOs.Combo;
using API.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository.Interfaces
{
    public interface IComboRepository
    {
        public Task<List<Combo>> GetAllCombo();
        public Task<List<Combo>> GetAllAvailableCombo();
        public Task<Combo> GetComboById(int id);
        public Task CreateCombo(CreateComboDTO combo);
        public Task UpdateCombo(int id, ComboDTO combo);
        public Task ChangeComboStatus(int id);
        public Task DeleteCombo(int id);
    }
}
