using API.BO.DTOs.Account;
using API.BO.Models;
using AutoMapper;

namespace ElectronicStoreAPI.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            AccountMappingProfile();
            
        }
    
        void AccountMappingProfile()
        {
            CreateMap<Account, AccountUpdateDTO>().ReverseMap();
            CreateMap<Account, AccountDTO>().ReverseMap();
        
        }
    }

}
