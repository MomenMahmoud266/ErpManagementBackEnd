namespace ErpManagement.Services.Services.Auth;
public class AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager,
                                   JwtSettings jwt, IStringLocalizer<SharedResource> sharLocalizer, ILogger<AuthenticationService> logger,
                                   IHttpContextAccessor accessor, SignInManager<ApplicationUser> signInManager,
                                   RoleManager<ApplicationRole> roleManager) : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly JwtSettings _jwt = jwt;
    private readonly ILogger<AuthenticationService> _logger = logger;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IHttpContextAccessor _accessor = accessor;

    #region Authentication

    public async Task<Response<AuthLoginUserResponse>> LoginUserAsync(AuthLoginUserRequest model)
    {
        string err = _sharLocalizer[Localization.Error];

        var user = await _userManager.FindByNameAsync(model.UserName);

        if (user is null)
            return new()
            {
                Data = new()
                {
                    UserName = model.UserName
                },
                Error = _sharLocalizer[Localization.PasswordNotmatch],
                Message = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.UserName]),
                IsSuccess = false
            };

        var test = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, model.RememberMe, false);
        if (!test.Succeeded)
            return new()
            {
                Data = new()
                {
                    UserName = model.UserName
                },
                Error = err,
                Message = _sharLocalizer[Localization.PasswordNotmatch],
                IsSuccess = false
            };

        //if (!user.IsActive)
        //{
        //    string resultIsSuccess = string.Format(_sharLocalizer[Localization.NotActive],
        //        _sharLocalizer[Localization.User], model.UserName);

        //    return new()
        //    {
        //        Data = new()
        //        {
        //            UserName = model.UserName
        //        },
        //        Error = resultIsSuccess,
        //        IsSuccess = resultIsSuccess
        //    };
        //}


        //if (model.DeviceId != null)
        //{
        //    //user.DeviceId = model.DeviceId;

        //    await _unitOfWork.UserDevices.AddAsync(new ApplicationUserDevice
        //    {
        //        UserId = user.Id,
        //        DeviceId = model.DeviceId
        //    });

        //    await _unitOfWork.CompleteAsync();
        //}

        var currentUserRoles = (await _userManager.GetRolesAsync(user)).ToList();
        //string superAdminRole = Domain.Constants.Enums.RolesEnums.Superadmin.ToString().Trim();

        var jwtSecurityToken = await CreateJwtToken(user);
        var result = new AuthLoginUserResponse
        {
            UserName = user.UserName!,
            Email = user.Email!,
            RoleNames = currentUserRoles,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            ExpiresOn = jwtSecurityToken.ValidTo,
            TenantId = user.TenantId
        };

        return new Response<AuthLoginUserResponse>
        {
            IsSuccess = true,
            Data = result
        };
    }

    public async Task<Response<AuthUpdateUserRequest>> UpdateUserAsync(string id, AuthUpdateUserRequest model)
    {
        if (id == null || model.Id != id)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.User], id);

            return new Response<AuthUpdateUserRequest>()
            {
                Data = model,
                Error = resultIsSuccess,
                Message = resultIsSuccess
            };
        }
        string err = _sharLocalizer[Localization.Error];

        var obj = _mapper.Map<AuthUpdateUserRequest, ApplicationUser>(model, (await _userManager.FindByIdAsync(id))!);
        obj.UpdateDate = new DateTime().NowEg();
        obj.UpdateBy = _accessor!.HttpContext == null ? string.Empty : _accessor!.HttpContext!.User.GetUserId();

        bool isSucceeded = (await _userManager.UpdateAsync(obj)).Succeeded;

        return new Response<AuthUpdateUserRequest>()
        {
            IsSuccess = isSucceeded,
            Data = model,
            Message = isSucceeded ? _sharLocalizer[Localization.Updated] : _sharLocalizer[err]
        };
    }

    public async Task<Response<string>> ShowPasswordToSpecificUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.User], id);

            return new Response<string>()
            {
                Data = id,
                Error = resultIsSuccess,
                Message = resultIsSuccess
            };
        }

        string err = _sharLocalizer[Localization.Error];

        return new Response<string>()
        {
            IsSuccess = false,
            Error = err,
            Message = "This endpoint has been removed for security reasons."
        };
    }

    public async Task<Response<AuthChangePassOfUserResponse>> ChangePasswordAsync(AuthChangePassOfUserRequest model)
    {
        string err = _sharLocalizer[Localization.Error];

        var mappedResponse = _mapper.Map<AuthChangePassOfUserResponse>(model);

        if (model.CurrentPassword == model.NewPassword)
        {
            string resultIsSuccess = _sharLocalizer[Localization.CurrentAndNewPasswordIsTheSame];

            return new Response<AuthChangePassOfUserResponse>()
            {
                Data = mappedResponse,
                Error = resultIsSuccess,
                Message = resultIsSuccess
            };
        }

        string userId = _accessor!.HttpContext == null ? string.Empty : _accessor!.HttpContext!.User.GetUserId();
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.User], userId);

            return new Response<AuthChangePassOfUserResponse>()
            {
                Data = mappedResponse,
                Error = resultIsSuccess,
                Message = resultIsSuccess
            };
        }

        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        if (!result.Succeeded)
        {
            string resultIsSuccess = _sharLocalizer[Localization.CurrentPasswordIsIncorrect];

            return new Response<AuthChangePassOfUserResponse>()
            {
                Data = mappedResponse,
                Error = resultIsSuccess,
                Message = resultIsSuccess
            };
        }

        if (!user.IsActive)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.NotActive],
                _sharLocalizer[Localization.User], user.UserName);

            return new Response<AuthChangePassOfUserResponse>()
            {
                Data = mappedResponse,
                Error = resultIsSuccess,
                Message = resultIsSuccess
            };
        }

        return new Response<AuthChangePassOfUserResponse>()
        {
            Message = string.Format(_sharLocalizer[Localization.Updated]),
            IsSuccess = true,
            Data = mappedResponse
        };
    }

    public async Task<Response<AuthSetNewPasswordRequest>> SetNewPasswordToSpecificUserAsync(AuthSetNewPasswordRequest model)
    {
        string err = _sharLocalizer[Localization.Error];

        using var transaction = _unitOfWork.BeginTransaction();

        var user = await _userManager.FindByIdAsync(model.UserId);

        if (user is null)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.User], model.UserId);

            return new Response<AuthSetNewPasswordRequest>()
            {
                Data = model,
                Error = resultIsSuccess,
                Message = resultIsSuccess
            };
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

        _unitOfWork.Users.Update(user);
        await _unitOfWork.CompleteAsync();

        if (!result.Succeeded)
        {
            string resultIsSuccess = _sharLocalizer[Localization.CurrentPasswordIsIncorrect];

            return new Response<AuthSetNewPasswordRequest>()
            {
                Data = model,
                Error = resultIsSuccess,
                Message = resultIsSuccess
            };
        }
        await transaction.CommitAsync();
        return new Response<AuthSetNewPasswordRequest>()
        {
            Message = string.Format(_sharLocalizer[Localization.Updated]),
            IsSuccess = true,
            Data = model
        };
    }

    public async Task<Response<string>> SetNewPasswordToSuperAdminAsync(string newPassword)
    {
        string err = _sharLocalizer[Localization.Error];

        using var transaction = _unitOfWork.BeginTransaction();

        var user = await _userManager.FindByIdAsync(SuperAdmin.Id);

        if (user is null)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.User], SuperAdmin.Id);

            return new Response<string>()
            {
                Data = newPassword,
                Error = resultIsSuccess,
                Message = resultIsSuccess
            };
        }


        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        _unitOfWork.Users.Update(user);
        await _unitOfWork.CompleteAsync();

        if (!result.Succeeded)
        {
            string resultIsSuccess = _sharLocalizer[Localization.CurrentPasswordIsIncorrect];

            return new Response<string>()
            {
                Data = newPassword,
                Error = resultIsSuccess,
                Message = resultIsSuccess
            };
        }
        await transaction.CommitAsync();
        return new Response<string>()
        {
            Message = string.Format(_sharLocalizer[Localization.Updated]),
            IsSuccess = true,
            Data = "Password updated successfully."
        };

    }

    public async Task<Response<string>> LogOutUserAsync()
    {
        string userId = GetUserId();
        var lsitOfObjects = await _unitOfWork.UserDevices.GetAllAsync(x => x.UserId == userId);

        if (!lsitOfObjects.Any())
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.User], userId);

            return new Response<string>()
            {
                Data = string.Empty,
                Error = resultIsSuccess,
                Message = resultIsSuccess
            };
        }
        string err = _sharLocalizer[Localization.Error];
        try
        {
            _unitOfWork.UserDevices.DeleteRange(lsitOfObjects);

            bool result = await _unitOfWork.CompleteAsync() > 0;

            if (!result)
                return new Response<string>()
                {
                    IsSuccess = false,
                    Data = userId,
                    Error = err,
                    Message = err
                };
            return new Response<string>()
            {
                IsSuccess = true,
                Data = userId,
                Message = _sharLocalizer[Localization.Deleted]
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, err);

            return new Response<string>()
            {
                Error = err,
                Message = ex.Message + (ex.InnerException == null ? string.Empty : ex.InnerException.Message)
            };
        }
    }

    #endregion

    private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        List<Claim> roleClaims = [];

        var roles = (await _userManager.GetRolesAsync(user)).ToList();

        foreach (var role in roles)
        {
            var dd = await _roleManager.FindByNameAsync(role);

            var ddd = _roleManager.GetClaimsAsync(dd!).Result;
            roleClaims.AddRange(ddd);
        }
        for (int i = 0; i < roles.Count; i++)
            roleClaims.Add(new Claim(ClaimTypes.Role, roles[i]));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(RequestClaims.UserId, user.Id),
            new Claim(RequestClaims.TenantId, user.TenantId.ToString() )
        }
        .Union(roleClaims);
        //.Union(await _userManager.GetClaimsAsync(user));

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

        return new JwtSecurityToken(
            issuer: _jwt.Issuer,
            claims: claims,
            expires: new DateTime().NowEg().Add(_jwt.TokenLifetime),
            signingCredentials: signingCredentials);
    }
    private string GetUserId() =>
    _accessor!.HttpContext == null ? string.Empty : _accessor!.HttpContext!.User.GetUserId();
}
