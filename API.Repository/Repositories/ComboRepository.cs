using API.BO.DTOs.Combo;
using API.BO.Models;
using API.Repository.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository.Repositories
{
    public class ComboRepository : BaseRepository<ComboRepository>, IComboRepository
    {
        private readonly IMongoCollection<Combo> _combo;

        public ComboRepository(IOptions<MongoDBContext> setting, IMongoClient client) : base(setting, client)
        {
            _combo = _database.GetCollection<Combo>("Combo"); 
        }

        public async Task<List<Combo>> GetAllCombo()
        {
            return await _combo.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<List<Combo>> GetAllAvailableCombo()
        {
            return await _combo.Find(c => c.IsAvailable).ToListAsync();
        }

        public async Task<Combo> GetComboById(string id)
        {
            return await _combo.Find(c => c.ComboId.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task CreateCombo(CreateComboDTO combo)
        {
            Combo newCombo = new Combo(combo.Name, combo.Products, combo.Price);
            await _combo.InsertOneAsync(newCombo);
        }

        public async Task UpdateCombo(string id, ComboDTO combo)
        {
            Combo currentCombo = await _combo.Find(c => c.ComboId.Equals(id)).FirstOrDefaultAsync();

            if (currentCombo == null) throw new Exception("Cannot find combo");

            //Change Data
            currentCombo.Name = String.IsNullOrEmpty(combo.Name) ? currentCombo.Name : combo.Name;
            currentCombo.Products = combo.Products == null ? currentCombo.Products : combo.Products;
            currentCombo.Price = combo.Price;
            currentCombo.IsAvailable = combo.IsAvailable;
            
            await _combo.ReplaceOneAsync(c => c.ComboId.Equals(id), currentCombo);
        }

        public async Task ChangeComboStatus(string id)
        {
            Combo combo = await _combo.Find(c => c.ComboId.Equals(id)).FirstOrDefaultAsync();
            if (combo == null) throw new Exception("Cannot find combo");

            combo.IsAvailable = !combo.IsAvailable;
            await _combo.ReplaceOneAsync(c => c.ComboId.Equals(id), combo);  
        }

        public async Task DeleteCombo(string id)
        {
            FilterDefinition<Combo> filterDefinition = Builders<Combo>.Filter.Eq("Id", id);
            await _combo.DeleteOneAsync(filterDefinition);
        }
    }
}
