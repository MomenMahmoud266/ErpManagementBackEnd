using AutoMapper;
using ErpManagement.Domain.DTOs.Request.Transaction;
using ErpManagement.Domain.DTOs.Response.Transaction;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.API.Profiles.Transaction;

public class StockAdjustmentProfile : Profile
{
    public StockAdjustmentProfile()
    {
        CreateMap<StockAdjustment, StockAdjustmentCreateRequest>().ReverseMap();
        CreateMap<StockAdjustment, StockAdjustmentUpdateRequest>().ReverseMap();
        CreateMap<StockAdjustment, StockAdjustmentGetByIdResponse>().ReverseMap();
        CreateMap<StockAdjustment, PaginatedStockAdjustmentsData>().ReverseMap();

        CreateMap<StockAdjustmentItem, StockAdjustmentItemCreateRequest>().ReverseMap();
        CreateMap<StockAdjustmentItem, StockAdjustmentItemUpdateRequest>().ReverseMap();
    }
}