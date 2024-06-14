using API.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IList<T>> GetAll();
        Task<IList<T>> GetByCondition(
            Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            string[] includeProperties = null, 
            int? skip = null, int? take = null);

        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
    }
}
