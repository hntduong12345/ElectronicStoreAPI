using API.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAll();
        Task<List<Account>> GetByCondition(
            int? skip = null, int? take = null,
            params (Expression<Func<Account, object>> field, object value)[] filters
            );
        Task<bool> Add(Account entity);
        Task<bool> Update(Account entity);
        Task<bool> Delete(Account entity);
    }
}
