using ErpManagement.DataAccess.Repositories.Organization;
using ErpManagement.DataAccess.Repositories.People;
using ErpManagement.DataAccess.Repositories.Shared;
using ErpManagement.DataAccess.Services;
using ErpManagement.Domain.Interfaces.Repositories.Organization;
using ErpManagement.Domain.Interfaces.Repositories.People;
using ErpManagement.Services.IServices;
using ErpManagement.Services.IServices.Organization;
using ErpManagement.Services.IServices.People;
using ErpManagement.Services.IServices.Products;
using ErpManagement.Services.IServices.Transactions;
using ErpManagement.Services.Services.Organization;
using ErpManagement.Services.Services.People;
using ErpManagement.Services.Services.Products;
using ErpManagement.Services.Services.Transactions;

namespace ErpManagement.API.Utilities;

public static class InjectedDependenciesExtensions
{
    public static IServiceCollection InjectedDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandlerService>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProviderService>();
        //services.AddSingleton<IStaticDataRepository, StaticDataRepository>();
        services.AddSingleton<JwtSecurityTokenHandler, CustomJwtSecurityTokenHandler>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        //services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<IDbInitSeedsService, DbInitSeedsService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
       // services.AddScoped<ILoggingRepository, LoggingRepository>();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<IUnitService, UnitService>();
        services.AddScoped<ITaxService, TaxService>();
        services.AddScoped<IVariantService, VariantService>();
        services.AddScoped<IProductTypeService, ProductTypeService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IBillerService, BillerService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IPurchaseService, PurchaseService>();
        services.AddScoped<IPurchaseReturnService, PurchaseReturnService>();
        services.AddScoped<ISaleService, SaleService>();
        services.AddScoped<ISaleReturnService, SaleReturnService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IExpenseCategoryService, ExpenseCategoryService>();
        services.AddScoped<IExpenseService, ExpenseService>();
        services.AddScoped<IStockTransferService, StockTransferService>();
        services.AddScoped<IStockAdjustmentService, StockAdjustmentService>();
        services.AddScoped<ISaleInvoiceService, SaleInvoiceService>();
        services.AddScoped<IPurchaseInvoiceService, PurchaseInvoiceService>();
        services.AddScoped<IExpenseInvoiceService, ExpenseInvoiceService>();
        return services;
    }
}
