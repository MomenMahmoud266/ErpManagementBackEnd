namespace ErpManagement.Services.IServices.Auth;

public interface IPermissionService
{
    // Crud of roles
    Task<Response<IEnumerable<SelectListForUserResponse>>> GetAllRolesAsync(RequestLangEnum lang);
    Task<Response<PermCreateRoleRequest>> CreateRoleAsync(PermCreateRoleRequest model);
    Task<Response<PermUpdateRoleRequest>> UpdateRoleAsync(string id, PermUpdateRoleRequest model);
    Task<Response<string>> DeleteRoleAsync(string id);

    // Spicial to roles of user
    Task<Response<PermGetAllUsersRolesResponse>> GetEachUserWithHisRolesAsync(PermGetEachUserWithRolesRequest model);
    Task<Response<PermGetManagementModelResponse>> GetUserRolesAsync(string userId, RequestLangEnum lang);
    Task<Response<PermGetManagementModelResponse>> UpdateUserRolesAsync(PermGetManagementModelResponse model);

    // Spicial to permissions(Claims) of role
    Task<Response<PermGetRolesPermissionsResponse>> GetRolePermissionsAsync(string roleId, RequestLangEnum lang);
    Task<Response<PermUpdateRolePermissionsRequest>> UpdateRolePermissionsAsync(PermUpdateRolePermissionsRequest model);

}
