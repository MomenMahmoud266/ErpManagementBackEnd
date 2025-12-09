namespace ErpManagement.Domain.Customization.Middleware;

public static class StaticFilesMiddlewareExtensionsMiddleware
{
    public static void ConfigureStaticFilesHandler(this WebApplication app)
    {
        string directory = Path.Combine(Directory.GetCurrentDirectory(), "FilesServer");
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "FilesServer")),
            RequestPath = "/FilesServer"
        });
    }
}
