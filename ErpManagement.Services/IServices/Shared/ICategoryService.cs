namespace ErpManagement.Services.IServices.Shared;

public interface ICategoryService
{
    Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang);
    Task<Response<CategoryGetAllResponse>> GetAllAsync(RequestLangEnum lang, CategoryGetAllFiltrationsRequest model);
    Task<Response<CategoryGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<CategoryCreateRequest>> CreateAsync(CategoryCreateRequest model);
    Task<Response<CategoryUpdateRequest>> UpdateAsync(int id, CategoryUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}