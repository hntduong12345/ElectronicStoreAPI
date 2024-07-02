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

        public async Task<List<Voucher>> GetCustomerVouchers(string id)
        {
            var result = (await _voucherRepository.GetByCondition(filters: (p => p.AccountId, id)));
            return result;
        }
        public async Task<Voucher> GetVoucher(string id)
        {
            var result = (await _voucherRepository.GetByCondition(filters: (p => p.VoucherId, id))).FirstOrDefault();
            return result;
        }
        public async Task<string> AddVoucher(Voucher voucher)
        {
            try
            {
                var existedCode = (await _voucherRepository.GetByCondition(filters: (p => p.VoucherCode, voucher.VoucherCode))).Where(p => p.VoucherId != voucher.VoucherId).FirstOrDefault();
                if (existedCode != null) throw new Exception("Voucher Code already exist");
                await _voucherRepository.Add(voucher);
                return "";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public async Task<bool> RemoveVoucher(Voucher voucher)
        {
            try
            {
                await _voucherRepository.Remove(voucher);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<string> UpdateVoucher(Voucher voucher)
        {
            try
            {
                var existedCode = (await _voucherRepository.GetByCondition(filters: (p => p.VoucherCode, voucher.VoucherCode))).Where(p => p.VoucherId != voucher.VoucherId).FirstOrDefault();
                if (existedCode != null) throw new Exception("Voucher Code already exist");
                //var voucher = (await _voucherRepository.GetByCondition()).FirstOrDefault();
                await _voucherRepository.Update(voucher);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<bool> DisableVoucher(string id)
        {
            try
            {
                var voucher = (await _voucherRepository.GetByCondition(filters: (p => p.VoucherId, id))).FirstOrDefault();
                voucher.Type = BO.Models.Enum.VoucherStatusEnum.DISABLED;
                await _voucherRepository.Update(voucher);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
