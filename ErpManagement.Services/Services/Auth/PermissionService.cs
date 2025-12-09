using static ErpManagement.Domain.Constants.Statics.SDStatic;

namespace ErpManagement.Services.Services.Auth;

public class PermissionService(UserManager<ApplicationUser> userManager, IStringLocalizer<SharedResource> sharLocalizer,
                               RoleManager<ApplicationRole> roleManager, IUnitOfWork unitOfWork, IHttpContextAccessor accessor) : IPermissionService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IHttpContextAccessor _accessor = accessor;

    #region Roles

    public async Task<Response<IEnumerable<SelectListForUserResponse>>> GetAllRolesAsync(RequestLangEnum lang)
    {
        var result = await _unitOfWork.Roles.GetSpecificSelectAsync(x => x.IsActive,
            select: x => new SelectListForUserResponse
            {
                Id = x.Id,
                Name = (lang == RequestLangEnum.Ar ? x.NameAr : lang == RequestLangEnum.En ? x.Name : x.NameTr)!
            }, orderBy: x =>
            x.OrderByDescending(x => x.Id));

        if (!result.Any())
        {
            string resultIsSuccess = _sharLocalizer[Localization.NotFoundData];

            return new()
            {
                Data = [],
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        return new()
        {
            Data = result,
            IsSuccess = true
        };
    }

    public async Task<Response<PermCreateRoleRequest>> CreateRoleAsync(PermCreateRoleRequest model)
    {
        string err = _sharLocalizer[Localization.Error];

        if (await _roleManager.RoleExistsAsync(model.Name.ToLower()))
            return new Response<PermCreateRoleRequest>()
            {
                Message = string.Format(_sharLocalizer[Localization.IsExist],
                    _sharLocalizer[Localization.Auth.Role]),
                IsSuccess = false,
                Data = model
            };

        var role = new ApplicationRole()
        {
            Name = model.Name,
            NameAr = model.NameAr,
            NameTr = model.NameTr
        };
        role.InsertBy = role.UpdateBy = GetUserId();
        role.InsertDate = role.UpdateDate = new DateTime().NowEg();

        var result = await _roleManager.CreateAsync(role);
        if (result.Succeeded)
            return new()
            {
                Message = _sharLocalizer[Localization.Done],
                IsSuccess = true,
                Data = model
            };
        return new()
        {
            Message = result.Errors.Select(x => x.Description).First(),
            Error = result.Errors.Select(x => x.Description).First(),
        };
    }

    public async Task<Response<PermUpdateRoleRequest>> UpdateRoleAsync(string id, PermUpdateRoleRequest model)
    {
        var role = await _roleManager.FindByIdAsync(id);

        if (role is null || id != model.Id)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.Auth.Role], $"({id})");

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess,
                Data = new()
            };
        }

        string err = _sharLocalizer[Localization.Error];

        role.Name = model.Name;
        role.NameAr = model.NameAr;
        role.NameTr = model.NameTr;
        role.UpdateBy = GetUserId();
        role.UpdateDate = new DateTime().NowEg();

        var idResult = await _roleManager.UpdateAsync(role);

        return new Response<PermUpdateRoleRequest>()
        {
            Message = idResult.Succeeded ? _sharLocalizer[Localization.Updated] : _sharLocalizer[err],
            IsSuccess = idResult.Succeeded,
            Data = model
        };
    }

    public async Task<Response<string>> DeleteRoleAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        if (role is null)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.Auth.Role], $"({id})");

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        var allUsersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);

        if (allUsersInRole.Count > 0)
        {
            string resultIsSuccess = _sharLocalizer[Localization.CannotDeletedThisRole];

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        string err = _sharLocalizer[Localization.Error];

        var roleClaims = _roleManager.GetClaimsAsync(role).Result;

        if (roleClaims.Count > 0)
        {
            if (roleClaims.Count > 1)
                roleClaims.ToList().ForEach(async claim => await _roleManager.RemoveClaimAsync(role, claim));
            else
                await _roleManager.RemoveClaimAsync(role, roleClaims.First());
        }

        var obj = await _roleManager.DeleteAsync(role);

        bool result = obj.Succeeded;

        if (!result)
            return new()
            {
                Message = err,
                Error = err
            };
        return new()
        {
            Message = _sharLocalizer[Localization.Deleted],
            IsSuccess = true
        };
    }

    #endregion

    #region Spicial to roles of user

    public async Task<Response<PermGetAllUsersRolesResponse>> GetEachUserWithHisRolesAsync(PermGetEachUserWithRolesRequest model)
    {
        model.CreationEndDate.ToIncreaseOneHour();
        Expression<Func<ApplicationUser, bool>> filter = x => x.IsActive && !x.IsDeleted
        &&
        (model.CreationStartDate == null || x.InsertDate!.Value.Date >= model.CreationStartDate.Value.Date)
        &&
        (model.CreationEndDate == null || x.InsertDate!.Value.Date <= model.CreationEndDate.Value.Date);

        //var users = _userManager.Users.Where(filter);

        var result = new PermGetAllUsersRolesResponse
        {
            TotalRecords = await _unitOfWork.Users.CountAsync(filter),
            Data = _userManager.Users.Where(filter)
            .ToList()
            .Skip((model.PageNumber - 1) * model.PageSize)
            .Take(model.PageSize)
            .Select(user => new PermGetAllUsersRoles
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                Roles = [.. _userManager.GetRolesAsync(user).Result]
            })
            .ToList()
        };

        return new Response<PermGetAllUsersRolesResponse>()
        {
            Data = result,
            IsSuccess = true
        };
    }

    public async Task<Response<PermGetManagementModelResponse>> GetUserRolesAsync(string userId, RequestLangEnum lang)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var roles = _roleManager.Roles.Where(x => x.Id != SuperAdmin.Id).ToList();

        var result = new PermGetManagementModelResponse
        {
            UserId = user!.Id,
            UserName = user.UserName!,
            ListOfCheckBoxes = roles.Select(role => new CheckBox
            {
                DisplayValue = (lang == RequestLangEnum.Ar ? role.NameAr : lang == RequestLangEnum.En ? role.Name : role.NameTr)!,
                IsSelected = _userManager.IsInRoleAsync(user, role.Name!).Result
            }).ToList()
        };

        if (result is null)
        {
            string resultMsg = _sharLocalizer[Localization.NotFoundData];

            return new()
            {
                Message = resultMsg,
                Error = resultMsg
            };
        }

        return new()
        {
            IsSuccess = true,
            Data = result
        };
    }

    public async Task<Response<PermGetManagementModelResponse>> UpdateUserRolesAsync(PermGetManagementModelResponse model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        var userRoles = await _userManager.GetRolesAsync(user!);

        await _userManager.RemoveFromRolesAsync(user!, userRoles);
        await _userManager.AddToRolesAsync(user!, model.ListOfCheckBoxes.Where(x => x.IsSelected).Select(y => y.DisplayValue));

        return new()
        {
            Data = model,
            Message = _sharLocalizer[Localization.Updated],
            IsSuccess = true
        };
    }

    #endregion

    #region Spicial to permissions(Claims) of role

    public async Task<Response<PermGetRolesPermissionsResponse>> GetRolePermissionsAsync(string roleId, RequestLangEnum lang)
    {
        var role = await _roleManager.FindByIdAsync(roleId);

        if (role is null)
        {
            string resultMsg = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.Auth.Role], roleId);
            return new()
            {
                Message = resultMsg,
                Error = resultMsg
            };
        }

        var roleClaims = _roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList();
        var allClaims = PermissionsStatic.GenerateAllPermissions(); // It is meant all endpoints
        var allPermissions = allClaims.Select(p => new CheckBox
        {
            DisplayValue = p
        }).ToList();

        foreach (var permission in allPermissions)
            if (roleClaims.Any(c => c == permission.DisplayValue))
                permission.IsSelected = true;

        var result = new PermGetRolesPermissionsResponse
        {
            RoleId = roleId,
            RoleName = (lang == RequestLangEnum.Ar ? role.NameAr : lang == RequestLangEnum.En ? role.Name : role.NameTr)!,
            ListOfCheckBoxes = allPermissions
        };

        return new Response<PermGetRolesPermissionsResponse>()
        {
            Data = result,
            IsSuccess = true
        };
    }

    public async Task<Response<PermUpdateRolePermissionsRequest>> UpdateRolePermissionsAsync(PermUpdateRolePermissionsRequest model)
    {
        var role = await _roleManager.FindByIdAsync(model.RoleId);

        if (role is null)
        {
            string resultMsg = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.Auth.Role], model.RoleId);

            return new()
            {
                Message = resultMsg,
                Error = resultMsg
            };
        }

        var roleClaims = await _roleManager.GetClaimsAsync(role);//true

        var falseValues = model.ListOfCheckBoxes.Where(y => !y.IsSelected).Select(x => x.DisplayValue).ToList();

        var claims = roleClaims.Where(x => falseValues.Contains(x.Value)).ToList();

        if (claims.Count > 0)
            foreach (var claim in claims)
                await _roleManager.RemoveClaimAsync(role, claim);

        var selectedClaims = model.ListOfCheckBoxes.Where(c => c.IsSelected).ToList();

        if (selectedClaims.Count > 0)
        {
            foreach (var claim in selectedClaims)
                await _roleManager.AddClaimAsync(role, new Claim(RequestClaims.Permission, claim.DisplayValue));
        }
        return new()
        {
            Message = _sharLocalizer[Localization.Updated],
            IsSuccess = true,
            Data = model
        };
    }

    #endregion

    private string GetUserId() =>
        _accessor!.HttpContext == null ? string.Empty : _accessor!.HttpContext!.User.GetUserId();
}
