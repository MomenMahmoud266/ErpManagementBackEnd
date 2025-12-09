using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Interfaces;
using Net.YSolution.Sac.Recruitment.Domain.Models.Shared;
using ErpManagement.Domain.Models.Core;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.People;
using ErpManagement.Domain.Models.Products;
using ErpManagement.Domain.Models.Transactions;
using ErpManagement.Domain.Models.Inventory;

namespace ErpManagement.DataAccess.DbContext;

public class ErpManagementDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
                                    ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
                                    ApplicationRoleClaim, ApplicationUserToken>
{
    private readonly ICurrentTenant _currentTenant;
    private readonly IHttpContextAccessor _accessor;

    public ErpManagementDbContext(
        DbContextOptions<ErpManagementDbContext> options,
        ICurrentTenant currentTenant,
        IHttpContextAccessor accessor) : base(options)
    {
        _currentTenant = currentTenant ?? throw new ArgumentNullException(nameof(currentTenant));
        _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
    }

    #region Data Sets

    // Auth
    public DbSet<ApplicationUserDevice> UserDevices { get; set; }
    public DbSet<ComLog> Logs { get; set; }

    // Core
    public DbSet<HrGender> Genders { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<State> States { get; set; }

    // Organization
    public DbSet<Branch> Branches { get; set; }
    public DbSet<SharDepartment> Departments { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }

    // People
    public DbSet<SharSupplier> Suppliers { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Biller> Billers { get; set; }

    // Products
    public DbSet<Product> Products { get; set; }
    public DbSet<WarehouseProduct> WarehouseProducts { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<Tax> Taxes { get; set; }
    public DbSet<Variant> Variants { get; set; }

    // Shared
    public DbSet<SharDefinition> Definitions { get; set; }

    // Transactions
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
    public DbSet<PurchaseReturn> PurchaseReturns { get; set; }
    public DbSet<SaleReturn> SaleReturns { get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }

    // Inventory
    public DbSet<StockMovement> StockMovements { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Call the seed data AFTER base.OnModelCreating and before configurations
        modelBuilder.SeedData();

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()).ToList().ForEach(x => x.DeleteBehavior = DeleteBehavior.NoAction);

        modelBuilder.Entity<ApplicationUser>().ToTable("Auth_Users", "dbo");
        modelBuilder.Entity<ApplicationRole>().ToTable("Auth_Roles", "dbo");
        modelBuilder.Entity<ApplicationUserRole>().ToTable("Auth_UserRoles", "dbo");
        modelBuilder.Entity<ApplicationUserClaim>().ToTable("Auth_UserClaims", "dbo");
        modelBuilder.Entity<ApplicationUserLogin>().ToTable("Auth_UserLogins", "dbo");
        modelBuilder.Entity<ApplicationRoleClaim>().ToTable("Auth_RoleClaims", "dbo");
        modelBuilder.Entity<ApplicationUserToken>().ToTable("Auth_UserTokens", "dbo");

        #region Fluent Api

        // ✅ NEW: Configure Branch → Country relationship
        modelBuilder.Entity<Branch>()
            .HasOne(b => b.Country)
            .WithMany()
            .HasForeignKey(b => b.CountryId)
            .OnDelete(DeleteBehavior.NoAction);

        // ✅ NEW: Configure Branch → Warehouses relationship
        modelBuilder.Entity<Warehouse>()
            .HasOne(w => w.Branch)
            .WithMany(b => b.Warehouses)
            .HasForeignKey(w => w.BranchId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Expense>()
            .HasOne(e => e.ExpenseCategory)
            .WithMany(c => c.Expenses)
            .HasForeignKey(e => e.ExpenseCategoryId);

        #endregion

        // ✅ Apply global filters ONLY once at the end
        ApplyGlobalFilters(modelBuilder);
    }

    // ✅ Keep this method - it correctly applies both filters
    private void ApplyGlobalFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            if (clrType == null || clrType.IsAbstract)
                continue;

            // Skip Tenant itself
            if (clrType == typeof(Tenant))
                continue;

            var param = Expression.Parameter(clrType, "e");
            Expression? filter = null;

            // 1) Soft delete for all BaseEntity
            if (typeof(BaseEntity).IsAssignableFrom(clrType))
            {
                var isDeletedProp = Expression.Property(param, nameof(BaseEntity.IsDeleted));
                var notDeleted = Expression.Equal(isDeletedProp, Expression.Constant(false));
                filter = notDeleted;
            }

            // 2) Tenant filter for all ITenantEntity
            if (typeof(ITenantEntity).IsAssignableFrom(clrType))
            {
                var tenantProp = Expression.Property(param, nameof(ITenantEntity.TenantId));
                var tenantValue = Expression.Constant(_currentTenant.TenantId);
                var tenantMatch = Expression.Equal(tenantProp, tenantValue);

                filter = filter == null
                    ? tenantMatch
                    : Expression.AndAlso(filter, tenantMatch);
            }

            if (filter is null)
                continue;

            var lambda = Expression.Lambda(filter, param);
            entityType.SetQueryFilter(lambda);
        }
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        DateTime dateNow = new DateTime().NowEg();
        string userId = _accessor?.HttpContext?.User.GetUserId() ?? string.Empty;

        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified
                    || e.State == EntityState.Deleted)
        );
        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).UpdateDate = dateNow;
            ((BaseEntity)entityEntry.Entity).UpdateBy = userId;

            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity)entityEntry.Entity).InsertDate = dateNow;
                ((BaseEntity)entityEntry.Entity).InsertBy = userId;
            }
            if (entityEntry.State == EntityState.Deleted)
            {
                entityEntry.State = EntityState.Modified;
                ((BaseEntity)entityEntry.Entity).DeleteDate = dateNow;
                ((BaseEntity)entityEntry.Entity).DeleteBy = userId;
                ((BaseEntity)entityEntry.Entity).IsDeleted = true;
            }
        }
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
