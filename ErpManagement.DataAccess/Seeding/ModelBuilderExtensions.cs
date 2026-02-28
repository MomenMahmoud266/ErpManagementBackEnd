using ErpManagement.Domain.Models.Core;
using ErpManagement.Domain.Models.Hr;
using ErpManagement.Domain.Enums;

namespace ErpManagement.DataAccess.Seeding;

public static class ModelBuilderExtensions
{
    public static void SeedData(this ModelBuilder modelBuilder)
    {
        #region Currencies (MUST BE FIRST - Tenant has FK to Currency)

        modelBuilder.Entity<Currency>()
            .HasData(
            new Currency { Id = 1, Code = "EGP", Symbol = "E£", DecimalDigits = 2, IsActive = true, IsDeleted = false },
            new Currency { Id = 2, Code = "USD", Symbol = "$",  DecimalDigits = 2, IsActive = true, IsDeleted = false },
            new Currency { Id = 3, Code = "EUR", Symbol = "€",  DecimalDigits = 2, IsActive = true, IsDeleted = false },
            new Currency { Id = 4, Code = "SAR", Symbol = "﷼",  DecimalDigits = 2, IsActive = true, IsDeleted = false }
        );

        #endregion

        #region Tenants (depends on Currencies)

        modelBuilder.Entity<Tenant>()
            .HasData(
            new Tenant()
            {
                Id = 1,
                Name = "Default Tenant",
                Description = "Default tenant for development",
                ContactEmail = "admin@tenant.local",
                ContactPhone = "+1234567890",
                Address = "Default Address",
                IsActive = true,
                IsDeleted = false,
                BusinessType = BusinessType.Retail,
                EnableInventory = true,
                EnableAppointments = false,
                EnableMemberships = false,
                EnableTables = false,
                EnableKitchenRouting = false,
                CurrencyId = 1,
                CountryCode = "EG",
                TimeZoneId = "Africa/Cairo",
                TaxLabel = "VAT",
                CostingMethod = "Average",
                InventoryMode = "Perpetual",
                IsSubscriptionActive = true
            });

        #endregion

        #region Users and their roles

        modelBuilder.Entity<ApplicationRole>()
            .HasData(
            new ApplicationRole()
            {
                Id = SuperAdmin.RoleId,
                Name = nameof(RolesEnums.Superadmin),
                NameAr = SuperAdmin.RoleNameInAr,
                NameTr = SuperAdmin.RoleNameInTr,
                ConcurrencyStamp = "1",
                NormalizedName = "SUPERADMIN"
            });

        modelBuilder.Entity<ApplicationRole>()
            .HasData(
            new ApplicationRole()
            {
                Id = Admin.RoleId,
                Name = nameof(RolesEnums.Admin),
                NameAr = Admin.RoleNameInAr,
                NameTr = Admin.RoleNameInTr,
                ConcurrencyStamp = "1",
                NormalizedName = "ADMIN"
            });

        modelBuilder.Entity<ApplicationUser>()
            .HasData(
            new ApplicationUser()
            {
                Id = SuperAdmin.Id,
                UserName = "Mr_DevSuperAdmin",
                NormalizedUserName = "MR_DEVSUPERADMIN",
                Email = "devasuperdmin90@gmail.com",
                NormalizedEmail = "DEVSUPERADMIN96@GMAIL.COM",
                EmailConfirmed = true,
                IsActive = true,
                TenantId = 1,  // ✅ VALID tenant exists
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null!, SuperAdmin.Password)
            });

        modelBuilder.Entity<ApplicationUser>()
            .HasData(
            new ApplicationUser()
            {
                Id = Admin.Id,
                UserName = "Mr_DevAdmin",
                NormalizedUserName = "MR_DEVADMIN",
                Email = "devadmin90@gmail.com",
                NormalizedEmail = "DEVADMIN96@GMAIL.COM",
                EmailConfirmed = true,
                IsActive = true,
                TenantId = 1,  // ✅ ADDED - was missing!
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null!, Admin.Password)
            });

        modelBuilder.Entity<ApplicationUserRole>()
            .HasData(
            new ApplicationUserRole()
            {
                RoleId = SuperAdmin.RoleId,
                UserId = SuperAdmin.Id
            });

        modelBuilder.Entity<ApplicationUserRole>()
            .HasData(
            new ApplicationUserRole()
            {
                RoleId = Admin.RoleId,
                UserId = Admin.Id
            });

        #endregion

        #region Genders

        modelBuilder.Entity<HrGender>()
            .HasData(
                new() { Id = 1, NameAr = "ذكر", NameEn = "Male", NameTr = "Erkek" },
                new() { Id = 2, NameAr = "أنثى", NameEn = "Female", NameTr = "Dişi" }
            );

        #endregion
    }

    public static void AddQueryFilterToAllEntitiesAssignableFrom<T>(this ModelBuilder modelBuilder,
      Expression<Func<T, bool>> expression)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(T).IsAssignableFrom(entityType.ClrType))
                continue;

            var parameterType = Expression.Parameter(entityType.ClrType);
            var expressionFilter = ReplacingExpressionVisitor.Replace(
                expression.Parameters.Single(), parameterType, expression.Body);

            var currentQueryFilter = entityType.GetQueryFilter();
            if (currentQueryFilter != null)
            {
                var currentExpressionFilter = ReplacingExpressionVisitor.Replace(
                    currentQueryFilter.Parameters.Single(), parameterType, currentQueryFilter.Body);
                expressionFilter = Expression.AndAlso(currentExpressionFilter, expressionFilter);
            }

            var lambdaExpression = Expression.Lambda(expressionFilter, parameterType);
            entityType.SetQueryFilter(lambdaExpression);
        }
    }
}
