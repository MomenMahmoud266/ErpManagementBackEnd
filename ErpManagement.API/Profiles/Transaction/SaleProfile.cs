using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.API.Profiles.Transactions;

public class SaleProfile : Profile
{
    public SaleProfile()
    {
        CreateMap<SaleCreateRequest, Sale>().ReverseMap();
        CreateMap<SaleUpdateRequest, Sale>().ReverseMap();

        CreateMap<Sale, SaleGetByIdResponse>()
            .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items))
            .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.Customer.FirstName))
            .ForMember(d => d.WarehouseName, opt => opt.MapFrom(s => s.Warehouse != null ? s.Warehouse.Name : null))
            .ForMember(d => d.BillerName, opt => opt.MapFrom(s => s.Biller != null ? s.Biller.FirstName : null))
            .ForMember(d => d.BranchId, opt => opt.MapFrom(s => s.BranchId))
            .ForMember(d => d.BranchName, opt => opt.MapFrom(s => s.Branch != null ? s.Branch.NameEn : string.Empty))
            .ForMember(d => d.PaidAmount, opt => opt.MapFrom(s => s.Payments.Sum(p => p.PaidAmount)))
            .ForMember(d => d.LastPaymentType, opt => opt.MapFrom(s =>
                s.Payments.OrderByDescending(p => p.PaymentDate).Select(p => p.PaymentType).FirstOrDefault()))
            .ForMember(d => d.ChangeAmount, opt => opt.MapFrom(s =>
                Math.Max(0, s.Payments.Sum(p => p.PaidAmount) - s.TotalAmount)))
            .ReverseMap();

        CreateMap<SaleItem, SaleItemGetBySaleResponse>()
            .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Product.Title))
            .ReverseMap();

        CreateMap<Sale, PaginatedSalesData>().ReverseMap();
    }
}