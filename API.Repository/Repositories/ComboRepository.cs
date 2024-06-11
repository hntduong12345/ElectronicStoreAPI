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

        public ComboRepository(IOptions<MongoDBContext> setting) : base(setting)
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

        public async Task CreateCombo()
        {
            
        }

        public async Task UpdateCombo()
        {

        }

        public async Task DisableCombo()
        {

        }

        public async Task DeleteCombo()
        {

        }
    }
}
