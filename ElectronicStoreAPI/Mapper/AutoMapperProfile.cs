using API.BO.DTOs.Account;
using API.BO.DTOs.Voucher;
using API.BO.Models;
using AutoMapper;

namespace ElectronicStoreAPI.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            AccountMappingProfile();
            VoucherMappingProfile();
            
        }
    
        void AccountMappingProfile()
        {
            CreateMap<Account, AccountUpdateDTO>().ReverseMap();
            CreateMap<Account, AccountDTO>().ReverseMap();
        
        }

        void VoucherMappingProfile()
        {
            CreateMap<Voucher, VoucherDTO>().ReverseMap();
            CreateMap<Voucher, VoucherCreateDTO>().ReverseMap();
            CreateMap<Voucher, VoucherUpdateDTO>().ReverseMap();

        }
    }

}
