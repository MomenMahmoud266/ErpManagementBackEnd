using AutoMapper;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.API.Profiles.Transaction;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<Payment, PaymentCreateRequest>().ReverseMap();
        CreateMap<Payment, PaymentUpdateRequest>().ReverseMap();

        CreateMap<Payment, PaymentGetByIdResponse>()
            .ForMember(d => d.SaleCode, opt => opt.MapFrom(s => s.Sale != null ? s.Sale.SaleCode : null))
            .ForMember(d => d.PurchaseCode, opt => opt.MapFrom(s => s.Purchase != null ? s.Purchase.PurchaseCode : null));

        CreateMap<Payment, PaginatedPaymentsData>()
            .ForMember(d => d.SaleCode, opt => opt.MapFrom(s => s.Sale != null ? s.Sale.SaleCode : null))
            .ForMember(d => d.PurchaseCode, opt => opt.MapFrom(s => s.Purchase != null ? s.Purchase.PurchaseCode : null));
    }
}