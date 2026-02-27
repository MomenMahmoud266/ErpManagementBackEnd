using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A1_AddTenantBusinessTypeAndFeatureFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessType",
                table: "Tenants",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<bool>(
                name: "EnableAppointments",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableInventory",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableKitchenRouting",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableMemberships",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableTables",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // Defensive update: ensure all existing rows have correct defaults
            migrationBuilder.Sql(
                "UPDATE [Tenants] SET [BusinessType] = 1, [EnableInventory] = 1, " +
                "[EnableAppointments] = 0, [EnableMemberships] = 0, [EnableTables] = 0, [EnableKitchenRouting] = 0");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ab3c6876-6d64-47ee-88a7-f0b2cb90631b", "AQAAAAIAAYagAAAAEMlI7m5R7gG3KfN3L5sKBpenbaq9lbxNU3PRCeEJWSXVTMJU/mMC+Ise/WGEv45w/Q==", "63942b5b-a12a-427f-a53a-341872ca31d5" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ed10fdf1-a026-487f-8cfd-ba8ecdad3db1", "AQAAAAIAAYagAAAAEAegDdMO7rIexAudvVBZ62bA6iz8pIKMx5tk7F22UxfKSiL5rPosWXLXTpHoGCVG6g==", "5abcfc1a-3514-4416-9364-2107e03ca1c6" });

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BusinessType", "EnableAppointments", "EnableInventory", "EnableKitchenRouting", "EnableMemberships", "EnableTables" },
                values: new object[] { 1, false, true, false, false, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessType",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "EnableAppointments",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "EnableInventory",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "EnableKitchenRouting",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "EnableMemberships",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "EnableTables",
                table: "Tenants");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2c48d027-ff7a-4452-aebf-13be2be00084", "AQAAAAIAAYagAAAAEFNZ+RYYG666I0TFovRSx/2W4K+uMn8NwlsOtZm1fMaB1HlgZsYDSVuWwJ8VT3yTrQ==", "3a2b988a-7b3e-4567-86cd-1fc5eb890fa7" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5908834f-de53-4b10-a464-7c21be954b7f", "AQAAAAIAAYagAAAAEJKUrEVkcw/1A+rLf6wqxpH/PlS31oOb5ZCOAF7r4/gc4wVdwy5gLuz1DwLocXS71g==", "5aa0ef8b-a509-4784-900f-4881332702c0" });
        }
    }
}
