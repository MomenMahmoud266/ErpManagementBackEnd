using ErpManagement.Domain.DTOs.Request.Shared.Variant;
using ErpManagement.Domain.DTOs.Response.Shared.Variant;
using ErpManagement.Domain.Models.Products;

namespace ErpManagement.Domain.MapperProfiles;

public class VariantProfile : Profile
{
    public VariantProfile()
    {
        CreateMap<VariantCreateRequest, Variant>().ReverseMap();
        CreateMap<VariantUpdateRequest, Variant>().ReverseMap();
        CreateMap<VariantGetByIdResponse, Variant>().ReverseMap();
        CreateMap<VariantPaginatedData, Variant>().ReverseMap();
    }
}