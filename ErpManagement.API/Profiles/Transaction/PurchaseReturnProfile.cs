// ErpManagement.Services\MappingProfiles\Transactions\PurchaseReturnProfile.cs
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.Services.MappingProfiles.Transactions;

public class PurchaseReturnProfile : Profile
{
    public PurchaseReturnProfile()
    {
        CreateMap<PurchaseReturnCreateRequest, PurchaseReturn>().ReverseMap();
        CreateMap<PurchaseReturnUpdateRequest, PurchaseReturn>().ReverseMap();
        CreateMap<PurchaseReturn, PurchaseReturnGetByIdResponse>().ReverseMap();
        CreateMap<PurchaseReturn, PaginatedPurchaseReturnsData>().ReverseMap();

        CreateMap<PurchaseReturnItem, PurchaseReturnItemDetailsResponse>()
            .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Product.Title))
            .ReverseMap();

        CreateMap<PurchaseReturnItem, PurchaseReturnItemCreateRequest>().ReverseMap();
        CreateMap<PurchaseReturnItem, PurchaseReturnItemUpdateRequest>().ReverseMap();
    }
}