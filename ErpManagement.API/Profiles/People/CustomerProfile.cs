using AutoMapper;
using ErpManagement.Domain.Models.People;
using ErpManagement.Domain.DTOs.Request.People.Customer;
using ErpManagement.Domain.DTOs.Response.People.Customer;

namespace ErpManagement.API.Profiles.People;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerCreateRequest>().ReverseMap();
        CreateMap<Customer, CustomerUpdateRequest>().ReverseMap();
        CreateMap<Customer, CustomerGetByIdResponse>().ReverseMap();
        CreateMap<Customer, CustomerPaginatedData>().ReverseMap();
    }
}