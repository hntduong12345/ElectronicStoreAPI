using API.BO.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository.Repositories
{
    public abstract class BaseRepository<T> where T : class
    {
        protected IMongoClient _client;
        protected IMongoDatabase _database;

        public BaseRepository(IOptions<MongoDBContext> setting, IMongoClient client)
        {
            _client = client;// new MongoClient(setting.Value.ConnectionURI);
            _database = _client.GetDatabase(setting.Value.DatabaseName);
        }
    }
}
