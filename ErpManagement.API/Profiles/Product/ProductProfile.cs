using AutoMapper;
using ErpManagement.Domain.Models.Products;
using ErpManagement.Domain.DTOs.Request.Shared;
using ErpManagement.Domain.DTOs.Response.Shared;
using ErpManagement.Domain.DTOs.Request.Products;
using ErpManagement.Domain.DTOs.Response.Products;

namespace ErpManagement.API.Profiles.Shared;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductCreateRequest>().ReverseMap();
        CreateMap<Product, ProductUpdateRequest>().ReverseMap();
        CreateMap<Product, ProductGetByIdResponse>().ReverseMap();
        CreateMap<Product, ProductPaginatedData>().ReverseMap();
    }
}