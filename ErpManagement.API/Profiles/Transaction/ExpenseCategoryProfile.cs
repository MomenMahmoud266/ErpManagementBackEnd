using AutoMapper;
using ErpManagement.Domain.DTOs.Request.Transaction;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.Services.MappingProfiles;

public class ExpenseCategoryProfile : Profile
{
    public ExpenseCategoryProfile()
    {
        CreateMap<ExpenseCategoryCreateRequest, ExpenseCategory>().ReverseMap();
        CreateMap<ExpenseCategoryUpdateRequest, ExpenseCategory>().ReverseMap();
        CreateMap<ExpenseCategoryGetByIdResponse, ExpenseCategory>().ReverseMap();
        CreateMap<PaginatedExpenseCategoriesData, ExpenseCategory>().ReverseMap();
    }
}