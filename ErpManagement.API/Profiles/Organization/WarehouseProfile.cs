using ErpManagement.Domain.DTOs.Request.Organization.Warehouse;
using ErpManagement.Domain.DTOs.Response.Organization.Warehouse;
using ErpManagement.Domain.Models.Organization;

namespace ErpManagement.Domain.MapperProfiles;

public class WarehouseProfile : Profile
{
    public WarehouseProfile()
    {
        CreateMap<WarehouseCreateRequest, Warehouse>().ReverseMap();
        CreateMap<WarehouseUpdateRequest, Warehouse>().ReverseMap();
        CreateMap<WarehouseGetByIdResponse, Warehouse>().ReverseMap();
        CreateMap<WarehousePaginatedData, Warehouse>().ReverseMap();
    }
}