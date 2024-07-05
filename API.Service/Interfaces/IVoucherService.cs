using API.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Service.Interfaces
{
    public interface IVoucherService
    {
        Task<List<Voucher>> GetAllVouchers();
        Task AddVoucher(Voucher voucher);
        Task UpdateVoucher(Voucher voucher);
        Task RemoveVoucher(string id);
        Task<Voucher> GetVoucher(string id);
    }
}
