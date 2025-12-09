namespace ErpManagement.Services.Services.Auth;

public class PermissionAuthorizationHandlerService(IHttpContextAccessor accessor) : AuthorizationHandler<PermissionRequirementService>
{
    private readonly IHttpContextAccessor _accessor = accessor;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirementService requirement)
    {
        if (context.User is null)
            return;

        bool canAccess = await Task.Run(() => context.User.Claims.Any(c => c.Type == RequestClaims.Permission && c.Value == requirement.Permission && c.Issuer == SDStatic.Shared.ErpManagement));
        if (canAccess)
        {
            context.Succeed(requirement);
            return;
        }
    }
}
