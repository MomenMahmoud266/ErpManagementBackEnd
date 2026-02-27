using ErpManagement.DataAccess.DbContext;
using ErpManagement.DataAccess.Repositories.Auth;
using ErpManagement.DataAccess.Repositories.Core;
using ErpManagement.DataAccess.Repositories.Inventory;
using ErpManagement.DataAccess.Repositories.Organization;
using ErpManagement.DataAccess.Repositories.People;
using ErpManagement.DataAccess.Repositories.Products;
using ErpManagement.DataAccess.Repositories.Shared;
using ErpManagement.DataAccess.Repositories.Transaction;
using ErpManagement.DataAccess.Repositories.Transactions;
using ErpManagement.Domain.Interfaces;
using ErpManagement.Domain.Interfaces.Repositories.Core;
using ErpManagement.Domain.Interfaces.Repositories.Inventory;
using ErpManagement.Domain.Interfaces.Repositories.Organization;
using ErpManagement.Domain.Interfaces.Repositories.People;
using ErpManagement.Domain.Interfaces.Repositories.Products;
using ErpManagement.Domain.Interfaces.Repositories.Shared;
using ErpManagement.Domain.Interfaces.Repositories.Transaction;
using ErpManagement.Domain.Interfaces.Repositories.Transactions;

namespace ErpManagement.DataAccess.Repositories;

public class UnitOfWork(ErpManagementDbContext context, ICurrentTenant currentTenant) : IUnitOfWork
{
    private readonly ErpManagementDbContext _context = context;
    private readonly ICurrentTenant _currentTenant = currentTenant;

    public IUserRepository Users { get; private set; } = new UserRepository(context, currentTenant);
    public IRoleClaimRepository RoleClaims { get; private set; } = new RoleClaimRepository(context, currentTenant);
    public IUserDeviceRepository UserDevices { get; private set; } = new UserDeviceRepository(context, currentTenant);
    public IRoleRepository Roles { get; private set; } = new RoleRepository(context, currentTenant);
    public IUserRoleRepository UserRoles { get; private set; } = new UserRoleRepository(context, currentTenant);
    public ICategoryRepository Categories { get; private set; } = new CategoryRepository(context, currentTenant);
    public IBrandRepository Brands { get; private set; } = new BrandRepository(context, currentTenant);
    public IUnitRepository Units { get; private set; } = new UnitRepository(context, currentTenant);
    public ITaxRepository Taxes { get; private set; } = new TaxRepository(context, currentTenant);
    public IVariantRepository Variants { get; private set; } = new VariantRepository(context, currentTenant);
    public IProductTypeRepository ProductTypes { get; private set; } = new ProductTypeRepository(context, currentTenant);
    public IProductRepository Product { get; private set; } = new ProductRepository(context, currentTenant);
    public ICustomerRepository Customer { get; private set; } = new CustomerRepository(context, currentTenant);
    public ISupplierRepository Supplier { get; private set; } = new SupplierRepository(context, currentTenant);
    public IBillerRepository Biller { get; private set; } = new BillerRepository(context, currentTenant);
    public IWarehouseRepository Warehouses { get; private set; } = new WarehouseRepository(context, currentTenant);
    public IBranchRepository Branches { get; private set; } = new BranchRepository(context, currentTenant);
    public ICompanyRepository Companies { get; private set; } = new CompanyRepository(context, currentTenant);
    public IPurchaseRepository Purchases { get; private set; } = new PurchaseRepository(context, currentTenant);
    public IPurchaseItemRepository PurchaseItems { get; private set; } = new PurchaseItemRepository(context, currentTenant);
    public IWarehouseProductRepository WarehouseProducts { get; private set; } = new WarehouseProductRepository(context, currentTenant);
    public IStockMovementRepository StockMovements { get; private set; } = new StockMovementRepository(context, currentTenant);
    public IPurchaseReturnRepository PurchaseReturns { get; private set; } = new PurchaseReturnRepository(context, currentTenant);
    public IPurchaseReturnItemRepository PurchaseReturnItems { get; private set; } = new PurchaseReturnItemRepository(context, currentTenant);
    public ISaleRepository Sales { get; private set; } = new SaleRepository(context, currentTenant);
    public ISaleItemRepository SaleItems { get; private set; } = new SaleItemRepository(context, currentTenant);
    // Transactions / Payments / Expenses / Stock
    public IPaymentRepository Payments { get; private set; } = new PaymentRepository(context, currentTenant);                      // <-- added
    public IExpenseRepository Expenses { get; private set; } = new ExpenseRepository(context, currentTenant);                      // <-- added
    public IExpenseCategoryRepository ExpenseCategories { get; private set; } = new ExpenseCategoryRepository(context, currentTenant); // <-- added

    public IStockAdjustmentRepository StockAdjustments { get; private set; } = new StockAdjustmentRepository(context, currentTenant);           // <-- added
    public IStockAdjustmentItemRepository StockAdjustmentItems { get; private set; } = new StockAdjustmentItemRepository(context, currentTenant); // <-- added

    public IStockTransferRepository StockTransfers { get; private set; } = new StockTransferRepository(context, currentTenant);               // <-- added
    public IStockTransferItemRepository StockTransferItems { get; private set; } = new StockTransferItemRepository(context, currentTenant);     // <-- added

    // Sale returns
    public ISaleReturnRepository SaleReturns { get; private set; } = new SaleReturnRepository(context, currentTenant);               // <-- added
    public ISaleReturnItemRepository SaleReturnItems { get; private set; } = new SaleReturnItemRepository(context, currentTenant);     // <-- added
    public ICountryRepository Countries { get; private set; } = new CountryRepository(context, currentTenant);     // <-- added

    // Clinic / Salon
    public IAppointmentRepository Appointments { get; private set; } = new AppointmentRepository(context, currentTenant);
    public IAppointmentItemRepository AppointmentItems { get; private set; } = new AppointmentItemRepository(context, currentTenant);

    // Gym
    public IMembershipPlanRepository MembershipPlans { get; private set; } = new MembershipPlanRepository(context, currentTenant);
    public IMemberSubscriptionRepository MemberSubscriptions { get; private set; } = new MemberSubscriptionRepository(context, currentTenant);
    public IMemberCheckInRepository MemberCheckIns { get; private set; } = new MemberCheckInRepository(context, currentTenant);

    // Cashbox
    public ICashboxRepository Cashboxes { get; private set; } = new CashboxRepository(context, currentTenant);
    public ICashboxShiftRepository CashboxShifts { get; private set; } = new CashboxShiftRepository(context, currentTenant);
    public ICashMovementRepository CashMovements { get; private set; } = new CashMovementRepository(context, currentTenant);

    // Inventory periods (periodic costing)
    public IInventoryPeriodRepository InventoryPeriods { get; private set; } = new InventoryPeriodRepository(context, currentTenant);
    public IPhysicalCountRepository PhysicalCounts { get; private set; } = new PhysicalCountRepository(context, currentTenant);

    // Core
    public ITenantRepository Tenants { get; private set; } = new TenantRepository(context, currentTenant);


    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    public IDatabaseTransaction BeginTransaction() => new DatabaseTransaction(_context.Database.BeginTransaction());

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
