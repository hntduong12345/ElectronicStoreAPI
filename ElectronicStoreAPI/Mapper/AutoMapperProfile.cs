using API.BO.DTOs;
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
            CreateMap<Account, AccountDTO>().ReverseMap();
        }
    }

}
