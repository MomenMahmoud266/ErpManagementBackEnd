using ErpManagement.Domain.Interfaces.Repositories.Core;
using ErpManagement.Domain.Interfaces.Repositories.Inventory;
using ErpManagement.Domain.Interfaces.Repositories.Organization;
using ErpManagement.Domain.Interfaces.Repositories.People;
using ErpManagement.Domain.Interfaces.Repositories.Products;
using ErpManagement.Domain.Interfaces.Repositories.Shared;
using ErpManagement.Domain.Interfaces.Repositories.Transaction;
using ErpManagement.Domain.Interfaces.Repositories.Transactions;
using ErpManagement.Domain.Interfaces.Shared;

namespace ErpManagement.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IDatabaseTransaction BeginTransaction();

    IUserRepository Users { get; }
    IRoleClaimRepository RoleClaims { get; }
    IUserDeviceRepository UserDevices { get; }
    IRoleRepository Roles { get; }
    IUserRoleRepository UserRoles { get; }
    ICountryRepository Countries { get; }
    //ICountryRepository Countries { get; }
    //IStateRepository States { get; }
    //IBranchRepository Branches { get; }
    //IDefinitionRepository Definitions { get; }
    //IDepartmentRepository Departments { get; }
    //IStockRepository Stocks { get; }
    // ISupplierRepository Suppliers { get; }
    //IProductRepository Products { get; }
    ICategoryRepository Categories { get; }
    IBrandRepository Brands { get; }
    IUnitRepository Units { get; }
    ITaxRepository Taxes { get; }
    IVariantRepository Variants { get; }
    IProductTypeRepository ProductTypes { get; }
    IProductRepository Product { get; }
    ICustomerRepository Customer { get; }
    IBillerRepository Biller { get; }
    ISupplierRepository Supplier { get; }
    IWarehouseRepository Warehouses { get; }
    IBranchRepository Branches { get; }
    ICompanyRepository Companies { get; }
    IPurchaseRepository Purchases { get; }
    IPurchaseItemRepository PurchaseItems { get; }
    IWarehouseProductRepository WarehouseProducts { get; }
    IStockMovementRepository StockMovements { get; }

    // NEW: PurchaseReturn repositories
    IPurchaseReturnRepository PurchaseReturns { get; }
    IPurchaseReturnItemRepository PurchaseReturnItems { get; }
    ISaleRepository Sales { get; }
    ISaleItemRepository SaleItems { get; }

    IPaymentRepository Payments { get; }                           // <-- added
    IExpenseRepository Expenses { get; }                           // <-- added
    IExpenseCategoryRepository ExpenseCategories { get; }          // <-- added

    IStockAdjustmentRepository StockAdjustments { get; }           // <-- added
    IStockAdjustmentItemRepository StockAdjustmentItems { get; }   // <-- added

    IStockTransferRepository StockTransfers { get; }               // <-- added
    IStockTransferItemRepository StockTransferItems { get; }       // <-- added

    // Sale returns
    ISaleReturnRepository SaleReturns { get; }                     // <-- added
    ISaleReturnItemRepository SaleReturnItems { get; }             // <-- added

    // Clinic / Salon
    IAppointmentRepository Appointments { get; }
    IAppointmentItemRepository AppointmentItems { get; }

    // Gym
    IMembershipPlanRepository MembershipPlans { get; }
    IMemberSubscriptionRepository MemberSubscriptions { get; }
    IMemberCheckInRepository MemberCheckIns { get; }

    // Cashbox
    ICashboxRepository Cashboxes { get; }
    ICashboxShiftRepository CashboxShifts { get; }
    ICashMovementRepository CashMovements { get; }

    // Inventory periods (periodic costing)
    IInventoryPeriodRepository InventoryPeriods { get; }
    IPhysicalCountRepository PhysicalCounts { get; }

    // Core
    ITenantRepository Tenants { get; }

    Task<int> CompleteAsync();
}
