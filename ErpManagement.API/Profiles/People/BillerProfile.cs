using AutoMapper;
using ErpManagement.Domain.Models.People;
using ErpManagement.Domain.DTOs.Request.People.Biller;
using ErpManagement.Domain.DTOs.Response.People.Biller;

namespace ErpManagement.API.Profiles.People;

public class BillerProfile : Profile
{
    public BillerProfile()
    {
        CreateMap<Biller, BillerCreateRequest>().ReverseMap();
        CreateMap<Biller, BillerUpdateRequest>().ReverseMap();
        CreateMap<Biller, BillerGetByIdResponse>().ReverseMap();
        CreateMap<Biller, BillerPaginatedData>().ReverseMap();
    }
}