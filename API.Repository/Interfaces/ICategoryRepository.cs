﻿using API.BO.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository.Interfaces
{
    public interface ICategoryRepository
    {
		 IClientSessionHandle _session {  get; }
		Task<IList<Category>> GetAll();
        Task<Category?> Get(string id);
        Task<IList<Category>> GetRange(int start, int take);
        Task<IAggregateFluent<Category>> GetAggregatePipeline();
        Task Create(Category category);
        Task<bool> Update(Category category);
        Task<bool> Delete(Category category);
    }
}
