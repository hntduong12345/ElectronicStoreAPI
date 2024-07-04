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
        Task<string> AddVoucher(Voucher voucher);
        Task<bool> RemoveVoucher(Voucher voucher);
        Task<string> UpdateVoucher(Voucher voucher);
        Task<Voucher> GetVoucher(string id);
    }
}
