using ErpManagement.Domain.DTOs.Response.Shared.Tax;
using ErpManagement.Domain.DTOs.Request.Shared.Tax;
using ErpManagement.Domain.Models.Products;

namespace ErpManagement.Domain.MapperProfiles;

public class TaxProfile : Profile
{
    public TaxProfile()
    {
        CreateMap<TaxCreateRequest, Tax>().ReverseMap();
        CreateMap<TaxUpdateRequest, Tax>().ReverseMap();
        CreateMap<TaxGetByIdResponse, Tax>().ReverseMap();
        CreateMap<TaxPaginatedData, Tax>().ReverseMap();
    }
}