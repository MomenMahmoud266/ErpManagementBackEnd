using ErpManagement.Domain.DTOs.Request.Shared;
using ErpManagement.Domain.DTOs.Response.Shared;
using ErpManagement.Domain.Models.Products;

namespace ErpManagement.Domain.MapperProfiles;

public class ProductTypeProfile : Profile
{
    public ProductTypeProfile()
    {
        CreateMap<ProductTypeCreateRequest, ProductType>().ReverseMap();
        CreateMap<ProductTypeUpdateRequest, ProductType>().ReverseMap();
        CreateMap<ProductTypeGetByIdResponse, ProductType>().ReverseMap();
        CreateMap<ProductTypePaginatedData, ProductType>().ReverseMap();
    }
}