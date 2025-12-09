using ErpManagement.Domain.DTOs.Request.Shared.Brand;
using ErpManagement.Domain.DTOs.Response.Shared.Brand;
using ErpManagement.Domain.Models.Products;

namespace ErpManagement.Domain.MapperProfiles;

public class BrandProfile : Profile
{
    public BrandProfile()
    {
        CreateMap<BrandCreateRequest, Brand>().ReverseMap();
        CreateMap<BrandUpdateRequest, Brand>().ReverseMap();
        CreateMap<BrandGetByIdResponse, Brand>().ReverseMap();
        CreateMap<BrandPaginatedData, Brand>().ReverseMap();
    }
}