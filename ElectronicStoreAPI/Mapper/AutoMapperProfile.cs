using API.BO.DTOs.Account;
using API.BO.DTOs.Voucher;
using API.BO.Models;
using AutoMapper;
using ElectronicStoreAPI.Constants;
using System.Globalization;

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
            CreateMap<Voucher, VoucherDTO>()
                //.ForMember(destinationMember: p => p.CreatedDate, p => p.MapFrom(p => p.CreatedDate.ToString(DateConstant.DateFormat)))
                //.ForMember(destinationMember: p => p.ExpiryDate, p => p.MapFrom(p => p.ExpiryDate.ToString(DateConstant.DateFormat)))
                .ReverseMap();
            CreateMap<Voucher, VoucherCreateDTO>().ReverseMap();
            //.ForMember(destinationMember: p => p.ExpiryDate, p => p.MapFrom(p => p.ExpiryDate.ToString(DateConstant.DateFormat)));
            //.ForMember(p => p.ExpiryDate, p => p.MapFrom(p => DateTime.ParseExact(p.ExpiryDate, DateConstant.DateFormat, null)));
            CreateMap<Voucher, VoucherUpdateDTO>().ReverseMap();
                //.ForMember(destinationMember: p => p.ExpiryDate, p => p.MapFrom(p => p.ExpiryDate.ToString(DateConstant.DateFormat)));
                //.ForMember(p => p.ExpiryDate, p => p.MapFrom(p => DateTime.ParseExact(p.ExpiryDate, DateConstant.DateFormat, null)));
        }
    }

}
