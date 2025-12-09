using AutoMapper;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.Services.MappingProfiles;

public class ExpenseProfile : Profile
{
    public ExpenseProfile()
    {
        CreateMap<ExpenseCreateRequest, Expense>()
            .ForMember(dest => dest.ExpenseCategoryId, opt => opt.MapFrom(src => src.ExpenseCategoryId))
            .ReverseMap();
        CreateMap<ExpenseUpdateRequest, Expense>()
            .ForMember(dest => dest.ExpenseCategoryId, opt => opt.MapFrom(src => src.ExpenseCategoryId))
            .ReverseMap();
        
        CreateMap<ExpenseGetByIdResponse, Expense>().ReverseMap();
        CreateMap<PaginatedExpensesData, Expense>().ReverseMap();
    }
}