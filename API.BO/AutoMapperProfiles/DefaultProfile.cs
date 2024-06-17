using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using API.BO.Models;
using API.BO.DTOs;
using API.BO.Constants;
namespace API.BO.AutoMapperProfiles
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<Category,CategoryDto>();
            CreateMap<Product, ProductDto>().ForMember(
                des => des.RelativeUrl,
                setup => setup.MapFrom(
                    src => $"{GoogleBucketConstant.GOOGLE_BUCKET_BASE_URL}/{GoogleBucketConstant.GOOGLE_BUCKET}/{src.RelativeUrl}" )) ;
        }
    }
}
