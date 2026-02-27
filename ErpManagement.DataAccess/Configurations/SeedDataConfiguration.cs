using Microsoft.EntityFrameworkCore;
using ErpManagement.Domain.Models.Core;
using ErpManagement.Domain.Models.Auth;
using ErpManagement.Domain.Enums;

namespace ErpManagement.DataAccess.Configurations;

/// <summary>
/// Seed data configuration to ensure proper FK dependency order
/// </summary>
public static class SeedDataConfiguration
{
    public static void SeedData(this ModelBuilder modelBuilder)
    {
        // 1. Seed Tenants FIRST (no FK dependencies)
        modelBuilder.Entity<Tenant>().HasData(
            new Tenant 
            { 
                Id = 1,
                Name = "Default Tenant",
                Description = "Default tenant for development",
                ContactEmail = "admin@tenant.local",
                ContactPhone = "+1234567890",
                Address = "Default Address",
                IsActive = true,
                IsDeleted = false,
                InsertDate = null,
                UpdateDate = null,
                DeleteDate = null,
                InsertBy = null,
                UpdateBy = null,
                DeleteBy = null,
                BusinessType = BusinessType.Retail,
                EnableInventory = true,
                EnableAppointments = false,
                EnableMemberships = false,
                EnableTables = false,
                EnableKitchenRouting = false
            }
        );

        // 2. Seed Roles
        modelBuilder.Entity<ApplicationRole>().HasData(
            new ApplicationRole
            {
                Id = "fab4fac1-c546-41de-aebc-a14da68957ab1",
                Name = "Superadmin",
                NormalizedName = "SUPERADMIN",
                NameAr = "سوبر أدمن",
                NameTr = "süper yönetici",
                IsActive = true,
                IsDeleted = false,
                ConcurrencyStamp = "1"
            },
            new ApplicationRole
            {
                Id = "fab4fac1-c546-41de-aebc-a14da68957ab2",
                Name = "Admin",
                NormalizedName = "ADMIN",
                NameAr = "أدمن",
                NameTr = "yönetici",
                IsActive = true,
                IsDeleted = false,
                ConcurrencyStamp = "1"
            }
        );

        // 3. Seed Users (NOW with valid TenantId = 1)
        modelBuilder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = "b74ddd14-6340-4840-95c2-db12554843e5basb1",
                UserName = "Mr_DevSuperAdmin",
                NormalizedUserName = "MR_DEVSUPERADMIN",
                Email = "devasuperdmin90@gmail.com",
                NormalizedEmail = "DEVSUPERADMIN96@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAECTOP426hYRuAzkPvBPMl8xk5wit7umAHNykHMrhL+Xq67rlxbEBl1lB4otUdKM8lA==",
                SecurityStamp = "059284af-4e05-4ffb-9d70-4bc94e24a289",
                ConcurrencyStamp = "01ecd6e0-6e07-4ae0-b03f-6ee42fd52791",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                TenantId = 1, // Valid tenant!
                IsActive = true
            },
            new ApplicationUser
            {
                Id = "b74ddd14-6340-4840-95c2-db12554843e5basb2",
                UserName = "Mr_DevAdmin",
                NormalizedUserName = "MR_DEVADMIN",
                Email = "devadmin90@gmail.com",
                NormalizedEmail = "DEVADMIN96@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEEGh/woUR9lFnAZC+SZel7KMkrYtDV6yp56F7orNZEQiZiWwU8Pu7B6yrXvkCxy57w==",
                SecurityStamp = "32169564-92ea-4542-93ee-695e7303ea4b",
                ConcurrencyStamp = "cd2d5891-19b7-4345-89ea-53b8871be78c",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                TenantId = 1, // Valid tenant!
                IsActive = true
            }
        );

        // 4. Seed User Roles
        modelBuilder.Entity<ApplicationUserRole>().HasData(
            new ApplicationUserRole
            {
                UserId = "b74ddd14-6340-4840-95c2-db12554843e5basb1",
                RoleId = "fab4fac1-c546-41de-aebc-a14da68957ab1",
                IsActive = true,
                IsDeleted = false
            },
            new ApplicationUserRole
            {
                UserId = "b74ddd14-6340-4840-95c2-db12554843e5basb2",
                RoleId = "fab4fac1-c546-41de-aebc-a14da68957ab2",
                IsActive = true,
                IsDeleted = false
            }
        );

        // 5. Seed Gender data
        modelBuilder.Entity<HrGender>().HasData(
            new HrGender { Id = 1, NameEn = "Male", NameAr = "ذكر", NameTr = "Erkek" },
            new HrGender { Id = 2, NameEn = "Female", NameAr = "أنثى", NameTr = "Dişi" }
        );
    }
}