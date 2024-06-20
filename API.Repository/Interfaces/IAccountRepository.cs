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
        Task<IList<Account>> GetAll();
        Task<IList<Account>> GetByCondition(
            Expression<Func<Account, bool>> filter = null,
            Func<IQueryable<Account>, IOrderedQueryable<Account>> orderBy = null,
            int? skip = null, int? take = null);

        Task<bool> Add(Account entity);
        Task<bool> Update(Account entity);
        Task<bool> Delete(Account entity);
    }
}
