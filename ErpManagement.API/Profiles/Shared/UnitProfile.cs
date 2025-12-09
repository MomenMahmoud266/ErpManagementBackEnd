using ErpManagement.Domain.DTOs.Response.Shared.Unit;
using ErpManagement.Domain.DTOs.Request.Shared.Unit;
using ErpManagement.Domain.Models.Products;

namespace ErpManagement.Domain.MapperProfiles;

public class UnitProfile : Profile
{
    public UnitProfile()
    {
        CreateMap<UnitCreateRequest, Unit>().ReverseMap();
        CreateMap<UnitUpdateRequest, Unit>().ReverseMap();
        CreateMap<UnitGetByIdResponse, Unit>().ReverseMap();
        CreateMap<UnitPaginatedData, Unit>().ReverseMap();
    }
}