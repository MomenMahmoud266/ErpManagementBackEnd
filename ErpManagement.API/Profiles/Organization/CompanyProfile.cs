using ErpManagement.Domain.DTOs.Request.Organization.Company;
using ErpManagement.Domain.DTOs.Response.Organization.Company;
using ErpManagement.Domain.Models.Organization;

namespace ErpManagement.Domain.MapperProfiles;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<CompanyCreateRequest, Company>().ReverseMap();
        CreateMap<CompanyUpdateRequest, Company>().ReverseMap();
        CreateMap<CompanyGetByIdResponse, Company>().ReverseMap();
        CreateMap<CompanyPaginatedData, Company>().ReverseMap();
    }
}