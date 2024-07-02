using API.BO.DTOs.Combo;
using API.BO.Models;
using API.BO.Models.Documents;
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
        private readonly IProductRepository _productRepository;

        public ComboService(IComboRepository comboRepository, IProductRepository productRepository)
        {
            _comboRepository = comboRepository;
            _productRepository = productRepository;
        }

        public async Task ChangeComboStatus(string id)
        {
            await _comboRepository.ChangeComboStatus(id);
        }

        public async Task CreateCombo(CreateComboDTO combo)
        {
            await _comboRepository.CreateCombo(combo);
        }
        
        public async Task DeleteCombo(string id)
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

        public async Task<GetComboDTO> GetComboById(string id)
        {
            Combo combo = await _comboRepository.GetComboById(id);
            GetComboDTO responseData = new GetComboDTO()
            {
                ComboId = combo.ComboId,
                Name = combo.Name,
                IsAvailable = combo.IsAvailable,
                Price = combo.Price,
                Products = new List<Product>()
            };

            foreach(ComboProducts product in combo.Products)
            {
                responseData.Products.Add(await _productRepository.Get(product.ProductId));
            }

            return responseData;
        }

        public async Task UpdateCombo(string id, ComboDTO combo)
        {
            await _comboRepository.UpdateCombo(id, combo);
        }
    }
}
