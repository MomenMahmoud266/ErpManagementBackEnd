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
using ErpManagement.Domain.Models.Clinic;
using ErpManagement.Domain.Models.Gym;

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
    public DbSet<Currency> Currencies => Set<Currency>();
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
    public DbSet<InventoryPeriod> InventoryPeriods { get; set; }
    public DbSet<PhysicalCount> PhysicalCounts { get; set; }

    // Clinic / Salon
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<AppointmentItem> AppointmentItems => Set<AppointmentItem>();

    // Gym
    public DbSet<MembershipPlan> MembershipPlans => Set<MembershipPlan>();
    public DbSet<MemberSubscription> MemberSubscriptions => Set<MemberSubscription>();
    public DbSet<MemberCheckIn> MemberCheckIns => Set<MemberCheckIn>();

    // Cashbox
    public DbSet<Cashbox> Cashboxes => Set<Cashbox>();
    public DbSet<CashboxShift> CashboxShifts => Set<CashboxShift>();
    public DbSet<CashMovement> CashMovements => Set<CashMovement>();

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

        // Configure Tenant → Currency relationship
        modelBuilder.Entity<Tenant>()
            .HasOne(t => t.Currency)
            .WithMany(c => c.Tenants)
            .HasForeignKey(t => t.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

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

        // Clinic / Appointment relationships
        modelBuilder.Entity<Appointment>()
            .HasMany(a => a.Items)
            .WithOne(i => i.Appointment)
            .HasForeignKey(i => i.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Branch)
            .WithMany()
            .HasForeignKey(a => a.BranchId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Customer)
            .WithMany()
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Sale)
            .WithMany()
            .HasForeignKey(a => a.SaleId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<AppointmentItem>()
            .HasOne(ai => ai.Product)
            .WithMany()
            .HasForeignKey(ai => ai.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        // Gym relationships
        modelBuilder.Entity<MembershipPlan>()
            .HasOne(p => p.Branch)
            .WithMany()
            .HasForeignKey(p => p.BranchId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MembershipPlan>()
            .HasOne(p => p.Product)
            .WithMany()
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MemberSubscription>()
            .HasOne(s => s.Branch)
            .WithMany()
            .HasForeignKey(s => s.BranchId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MemberSubscription>()
            .HasOne(s => s.Customer)
            .WithMany()
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MemberSubscription>()
            .HasOne(s => s.MembershipPlan)
            .WithMany(p => p.Subscriptions)
            .HasForeignKey(s => s.MembershipPlanId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MemberSubscription>()
            .HasOne(s => s.LastSale)
            .WithMany()
            .HasForeignKey(s => s.LastSaleId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MemberSubscription>()
            .HasMany(s => s.CheckIns)
            .WithOne(c => c.MemberSubscription)
            .HasForeignKey(c => c.MemberSubscriptionId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MemberCheckIn>()
            .HasOne(c => c.Branch)
            .WithMany()
            .HasForeignKey(c => c.BranchId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MemberCheckIn>()
            .HasOne(c => c.Customer)
            .WithMany()
            .HasForeignKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);

        // Cashbox relationships
        modelBuilder.Entity<Cashbox>()
            .HasOne(c => c.Branch)
            .WithMany()
            .HasForeignKey(c => c.BranchId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CashboxShift>()
            .HasOne(s => s.Cashbox)
            .WithMany(c => c.Shifts)
            .HasForeignKey(s => s.CashboxId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CashMovement>()
            .HasOne(m => m.CashboxShift)
            .WithMany(s => s.Movements)
            .HasForeignKey(m => m.CashboxShiftId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        // ✅ Apply global filters ONLY once at the end
        ApplyGlobalFilters(modelBuilder);
    }

    // Per-request tenant id — evaluated at query time, not at model-build time
    public int CurrentTenantId => _currentTenant.TenantId;

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

            // 2) Tenant filter — reference DbContext.CurrentTenantId (per-request, not frozen constant)
            if (typeof(ITenantEntity).IsAssignableFrom(clrType))
            {
                var tenantProp = Expression.Property(param, nameof(ITenantEntity.TenantId));
                // Expression.Constant(this) binds to the live DbContext instance so the value
                // is read at query execution time rather than frozen at model-build time.
                var dbContextConstant = Expression.Constant(this);
                var currentTenantIdProp = Expression.Property(dbContextConstant, nameof(CurrentTenantId));
                var tenantMatch = Expression.Equal(tenantProp, currentTenantIdProp);

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

    public override int SaveChanges()
    {
        EnforceTenantOnAddedEntities();
        return base.SaveChanges();
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

        EnforceTenantOnAddedEntities();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void EnforceTenantOnAddedEntities()
    {
        foreach (var entry in ChangeTracker.Entries<ITenantEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Entity.TenantId == 0)
                {
                    if (CurrentTenantId == 0)
                        throw new UnauthorizedAccessException("Cannot save tenant-scoped entity: no authenticated tenant.");
                    entry.Entity.TenantId = CurrentTenantId;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                var originalTenantId = (int)entry.OriginalValues[nameof(ITenantEntity.TenantId)]!;
                if (entry.Entity.TenantId != originalTenantId)
                    throw new InvalidOperationException("Modifying TenantId on an existing entity is not allowed.");
            }
        }
    }
}