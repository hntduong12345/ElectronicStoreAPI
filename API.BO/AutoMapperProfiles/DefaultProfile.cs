using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using API.BO.Models;
using API.BO.Constants;
using API.BO.DTOs.Category;
using API.BO.DTOs.Product;
namespace API.BO.AutoMapperProfiles
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {           
        }   
    }
    public class CategoryProfile : Profile 
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
    public class ProductProfile : Profile   
    {
        public ProductProfile() 
        {
            CreateMap<Product, ProductDto>()
                .ForMember(
                   des => des.RelativeUrl,
                   setup => setup
                   .MapFrom(
                       src => $"{GoogleBucketConstant.GOOGLE_BUCKET_BASE_URL}/{GoogleBucketConstant.GOOGLE_BUCKET}/{src.RelativeUrl}"));
            CreateMap<UpdateProductDto, Product>()
                .ForAllMembers(
                    opt => opt
                    .Condition( (dto, dest, dtoField )=> dtoField is not null)) ;
        }
    }
}
