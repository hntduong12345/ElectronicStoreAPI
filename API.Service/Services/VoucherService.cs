using API.BO.Models;
using API.Repository.Interfaces;
using API.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Service.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        public VoucherService(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }
        public async Task<List<Voucher>> GetAllVouchers()
        {
            var result = await _voucherRepository.GetAll();
            return result;
        }
        public async Task<Voucher> GetVoucher(string id)
        {
            var result = (await _voucherRepository.GetByCondition(filters: (p => p.VoucherId, id))).FirstOrDefault();
            return result;
        }
        public async Task AddVoucher(Voucher voucher)
        {
            if (voucher.Percentage > 100) throw new Exception("Voucher percentage can only be max 100%");
            var existedCode = (await _voucherRepository.GetByCondition(filters: (p => p.VoucherCode, voucher.VoucherCode))).Where(p => p.VoucherId != voucher.VoucherId).FirstOrDefault();
            if (existedCode != null) throw new Exception("Voucher Code already exist");
            await _voucherRepository.Add(voucher);
        }
        public async Task RemoveVoucher(string id)
        {
            var existedCode = (await _voucherRepository.GetByCondition(filters: (p => p.VoucherId, id))).FirstOrDefault();
            if (existedCode == null) throw new Exception("Voucher Code doesn't exist");
            
            await _voucherRepository.Remove(existedCode);
        }
        public async Task UpdateVoucher(Voucher voucher)
        {
            var existedCode = (await _voucherRepository.GetByCondition(filters: (p => p.VoucherCode, voucher.VoucherCode))).Where(p => p.VoucherId != voucher.VoucherId).FirstOrDefault();
            if (existedCode == null) throw new Exception("Voucher Code doesn't exist");
            //var voucher = (await _voucherRepository.GetByCondition()).FirstOrDefault();
            await _voucherRepository.Update(voucher);
        }
    }
}
