namespace ErpManagement.Services.Services.Auth;

public class DbInitSeedsService(RoleManager<ApplicationRole> roleManager, IUnitOfWork unitOfWork) : IDbInitSeedsService
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    #region Claims

    public async Task SeedClaimsForSuperAdmin()
    {
        var superAdminRole = await _roleManager.FindByNameAsync(RolesEnums.Superadmin.ToString());

        List<string> modules = [];

        foreach (var item in Enum.GetValues(typeof(PermissionsModulesEnum)))
            modules.Add(item.ToString()!);

        var allClaims = await _roleManager.GetClaimsAsync(superAdminRole!);
        //List<GetPermissionsWithActions> allRoleClaims = [];
        List<string> allRoleClaims = [];


        foreach (string module in modules)
            allRoleClaims.AddRange(PermissionsStatic.GeneratePermissionsList(module));

        //List<ApplicationRoleClaim> allAddedRoleClaims = [];
        List<string> allAddedRoleClaims = [];


        foreach (var roleClaim in allRoleClaims)
            if (!allClaims.Any(c => c.Type == RequestClaims.Permission && c.Value == roleClaim))
            {
                await _roleManager.AddClaimAsync(superAdminRole!, new Claim(RequestClaims.Permission, roleClaim));

                //allAddedRoleClaims.Add(new ApplicationRoleClaim
                //{
                //    RoleId = superAdminRole!.Id,
                //    ClaimType = RequestClaims.Permission,
                //    ClaimValue = roleClaim.ClaimValue,
                //    NameAr = roleClaim.NameAr,
                //    NameEn = roleClaim.NameEn,
                //    NameTr = roleClaim.NameTr
                //});

                //await _unitOfWork.RoleClaims.AddRangeAsync(new ApplicationRoleClaim
                //{
                //    RoleId = superAdminRole!.Id,
                //    ClaimType = RequestClaims.Permission,
                //    ClaimValue = roleClaim.ClaimValue,
                //    NameAr = roleClaim.NameAr,
                //    NameEn = roleClaim.NameEn,
                //    NameTr = roleClaim.NameTr
                //});
                //await _unitOfWork.CompleteAsync();
            }
        //if (allAddedRoleClaims.Count > 0)
        //{
        //    await _unitOfWork.RoleClaims.CreateRangeAsync(allAddedRoleClaims);
        //    await _unitOfWork.CompleteAsync();
        //}
    }

    #endregion
}
