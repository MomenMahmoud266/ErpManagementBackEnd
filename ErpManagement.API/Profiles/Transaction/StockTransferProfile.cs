using AutoMapper;
using ErpManagement.Domain.DTOs.Request.Transaction;
using ErpManagement.Domain.DTOs.Response.Transaction;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.API.Profiles.Transaction;

public class StockTransferProfile : Profile
{
    public StockTransferProfile()
    {
        CreateMap<StockTransfer, StockTransferCreateRequest>().ReverseMap();
        CreateMap<StockTransfer, StockTransferUpdateRequest>().ReverseMap();
        CreateMap<StockTransfer, StockTransferGetByIdResponse>().ReverseMap();
        CreateMap<StockTransfer, PaginatedStockTransfersData>().ReverseMap();

        CreateMap<StockTransferItem, StockTransferItemCreateRequest>().ReverseMap();
        CreateMap<StockTransferItem, StockTransferItemUpdateRequest>().ReverseMap();
    }
}