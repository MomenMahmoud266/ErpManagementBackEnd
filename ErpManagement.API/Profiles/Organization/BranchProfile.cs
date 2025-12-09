using ErpManagement.Domain.DTOs.Request.Organization.Branch;
using ErpManagement.Domain.DTOs.Response.Organization.Branch;
using ErpManagement.Domain.Models.Organization;

namespace ErpManagement.Domain.MapperProfiles;

public class BranchProfile : Profile
{
    public BranchProfile()
    {
        CreateMap<BranchCreateRequest, Branch>().ReverseMap();
        CreateMap<BranchUpdateRequest, Branch>().ReverseMap();
        CreateMap<BranchGetByIdResponse, Branch>().ReverseMap();
        CreateMap<BranchPaginatedData, Branch>().ReverseMap();
    }
}