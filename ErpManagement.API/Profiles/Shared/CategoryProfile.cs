using ErpManagement.Domain.DTOs.Request.Shared.Category;
using ErpManagement.Domain.DTOs.Response.Shared.Category;
using ErpManagement.Domain.Models.Products;

namespace ErpManagement.Domain.MapperProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryCreateRequest, Category>().ReverseMap();
        CreateMap<CategoryUpdateRequest, Category>().ReverseMap();
        CreateMap<CategoryGetByIdResponse, Category>().ReverseMap();
        CreateMap<CategoryPaginatedData, Category>().ReverseMap();
    }
}