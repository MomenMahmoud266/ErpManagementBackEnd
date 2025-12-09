using AutoMapper;
using ErpManagement.Domain.Models.People;
using ErpManagement.Domain.DTOs.Request.People.Supplier;
using ErpManagement.Domain.DTOs.Response.People.Supplier;

namespace ErpManagement.API.Profiles.People;

public class SupplierProfile : Profile
{
    public SupplierProfile()
    {
        CreateMap<Supplier, SupplierCreateRequest>().ReverseMap();
        CreateMap<Supplier, SupplierUpdateRequest>().ReverseMap();
        CreateMap<Supplier, SupplierGetByIdResponse>().ReverseMap();
        CreateMap<Supplier, SupplierPaginatedData>().ReverseMap();
    }
}