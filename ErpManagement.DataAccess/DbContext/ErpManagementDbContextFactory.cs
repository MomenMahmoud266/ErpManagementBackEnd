using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ErpManagement.Domain.Interfaces;

namespace ErpManagement.DataAccess.DbContext;

/// <summary>
/// Design-time factory for EF Core migrations
/// </summary>
public class ErpManagementDbContextFactory : IDesignTimeDbContextFactory<ErpManagementDbContext>
{
    public ErpManagementDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ErpManagement.API"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ErpManagementDbContext>();
        var connectionString = configuration.GetConnectionString("ErpManagementConnection");
        
        optionsBuilder.UseSqlServer(connectionString,
            b => b.MigrationsAssembly(typeof(ErpManagementDbContext).Assembly.FullName));

        var fakeCurrentTenant = new DesignTimeCurrentTenant();
        var fakeHttpContextAccessor = new DesignTimeHttpContextAccessor();

           return new ErpManagementDbContext(optionsBuilder.Options, fakeCurrentTenant, fakeHttpContextAccessor);
       }

    // Fake implementation for design-time only
    private class DesignTimeCurrentTenant : ICurrentTenant
    {
        public int TenantId => 1; // Default tenant for migrations
    }

    private class DesignTimeHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext? HttpContext { get; set; }
    }
}