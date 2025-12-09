using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.API.Profiles.Transactions;

public class SaleReturnProfile : Profile
{
    public SaleReturnProfile()
    {
        CreateMap<SaleReturnCreateRequest, SaleReturn>().ReverseMap();
        CreateMap<SaleReturnUpdateRequest, SaleReturn>().ReverseMap();
        CreateMap<SaleReturn, SaleReturnGetByIdResponse>()
            .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items))
            .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.Customer.FirstName))
            .ForMember(d => d.WarehouseName, opt => opt.MapFrom(s => s.Warehouse != null ? s.Warehouse.Name : null))
            .ForMember(d => d.BillerName, opt => opt.MapFrom(s => s.Biller != null ? s.Biller.FirstName : null))
            .ForMember(d => d.SaleCode, opt => opt.MapFrom(s => s.Sale != null ? s.Sale.SaleCode : null))
            .ReverseMap();

        CreateMap<SaleReturnItem, SaleReturnItemDetailsResponse>()
            .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Product != null ? s.Product.Title : null))
            .ReverseMap();

        CreateMap<SaleReturn, PaginatedSaleReturnsData>().ReverseMap();
        CreateMap<SaleReturnItem, SaleReturnItemCreateRequest>().ReverseMap();
        CreateMap<SaleReturnItem, SaleReturnItemUpdateRequest>().ReverseMap();
    }
}