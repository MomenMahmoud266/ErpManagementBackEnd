using ErpManagement.Domain.DTOs.Request.Purchases;
using ErpManagement.Domain.DTOs.Response.Purchases;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.API.Profiles.Transactions;

public class PurchaseProfile : Profile
{
    public PurchaseProfile()
    {
        CreateMap<PurchaseCreateRequest, Purchase>().ReverseMap();
        CreateMap<PurchaseUpdateRequest, Purchase>().ReverseMap();
        CreateMap<Purchase, PurchaseGetByIdResponse>()
            .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items))
            .ReverseMap();

        CreateMap<PurchaseItem, PurchaseItemGetByPurchaseResponse>()
            .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Product.Title))
            .ReverseMap();

        CreateMap<Purchase, PaginatedPurchasesData>().ReverseMap();
    }
}