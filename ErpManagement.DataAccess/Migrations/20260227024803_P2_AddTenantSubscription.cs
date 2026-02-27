using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class P2_AddTenantSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSubscriptionActive",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: true);

            // Ensure all existing tenants (beyond the seeded default) are active by default
            migrationBuilder.Sql("UPDATE [Tenants] SET [IsSubscriptionActive] = 1 WHERE [IsActive] = 1 AND [IsDeleted] = 0;");

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscriptionEndsAt",
                table: "Tenants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TrialEndsAt",
                table: "Tenants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cb3c4e47-ef5a-48f5-8be3-dac4398024d9", "AQAAAAIAAYagAAAAEPhpfNG2CEAAGaydbcwp4q64xNiYvcx41YlT+WzN4C/WppD3LfAZ32bBULZGrMW5zQ==", "b390b897-d7c6-4731-a27a-f8edcc98c917" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "29ee2af3-2d00-4c6d-829d-edb7b7255c43", "AQAAAAIAAYagAAAAEK16VIULJGrSLftnOnO9M4L1wt59p3gOK3baOYHPxfYyK+u4q+qZMhJlu4PXHG3V+g==", "71ec4940-a5ee-4174-89d3-b2af00b511b2" });

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsSubscriptionActive", "SubscriptionEndsAt", "TrialEndsAt" },
                values: new object[] { true, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSubscriptionActive",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "SubscriptionEndsAt",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "TrialEndsAt",
                table: "Tenants");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8b8dfe2c-8684-4eed-8c0c-d15d45feadd1", "AQAAAAIAAYagAAAAEJswdSYmfqjeB+WA34jVcN9M3iRWZRDG6Y+WcQ6jOtlYbTN2daRj2HBujs5oQqmppQ==", "a75275a5-ae5c-4450-846e-af603f9e469d" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f87b7354-1cb5-4702-b7bf-c1357b0ecf10", "AQAAAAIAAYagAAAAECnUtP+zIEamTWxk1tJHq79H1sfc9+gyIhs6MIWyl4SjYewTtJgU2TmDCxD84lkPYg==", "10d57c71-ea70-497a-bd50-a9086593e0c0" });
        }
    }
}
