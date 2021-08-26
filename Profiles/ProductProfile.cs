using AutoMapper;
using BakeryApi.DTOs;
using BakeryApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryApi.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();

            CreateMap<Product, Product>()
                .ForMember(source => source.Id, opt => opt.Ignore())
                .ForMember(source => source.ProductCode, opt => opt.Ignore())
                .ForMember(source => source.HiddenProperty, opt => opt.Ignore());

            CreateMap<Review, ReviewDTO>().ReverseMap();
        }
    }
}
