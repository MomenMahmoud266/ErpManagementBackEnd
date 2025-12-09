namespace ErpManagement.API.Utilities.SignalRConfig;

public class ConfigureJwtBearerOptionsBase
{
    public void PostConfigure(JwtBearerOptions options)
    {
        var originalOnMessageReceived = options.Events.OnMessageReceived;
        options.Events.OnMessageReceived = async context =>
        {
            await originalOnMessageReceived(context);

            if (string.IsNullOrEmpty(context.Token))
            {
                var accessToken = context.Request.Query[Shared.AccessToken];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments(Shared.RealimeViewData))
                {
                    context.Request.Headers.Authorization = accessToken;
                    context.Token = accessToken;
                }
            }
        };
    }
}