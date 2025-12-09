namespace ErpManagement.API.Utilities.SignalRConfig;

public class DomainRestrictedRequirement : AuthorizationHandler<DomainRestrictedRequirement, HubInvocationContext>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DomainRestrictedRequirement requirement,
                                                   HubInvocationContext resource)
    {
        if (true)
            context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
