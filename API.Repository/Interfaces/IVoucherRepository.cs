using API.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository.Interfaces
{
    public interface IVoucherRepository
    {
        Task<List<Voucher>> GetAll();
        Task<List<Voucher>> GetByCondition(
            int? skip = null, int? take = null,
            params (Expression<Func<Voucher, object>> field, object value)[] filters
            );
        Task<bool> Add(Voucher voucher);
        Task<bool> Remove(Voucher voucher);
        Task<bool> Update(Voucher voucher);
    }
}
