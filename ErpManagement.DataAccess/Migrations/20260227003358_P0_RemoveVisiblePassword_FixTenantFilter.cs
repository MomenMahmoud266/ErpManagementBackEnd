using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class P0_RemoveVisiblePassword_FixTenantFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branch_Tenants_TenantId",
                table: "Branch");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Branch_BranchId",
                table: "Warehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Tenants_TenantId",
                table: "Warehouses");

            migrationBuilder.DropTable(
                name: "Shar_Branches");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_TenantId",
                table: "Warehouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Branch",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "VisiblePassword",
                schema: "dbo",
                table: "Auth_Users");

            migrationBuilder.RenameTable(
                name: "Branch",
                newName: "Branches");

            migrationBuilder.RenameIndex(
                name: "IX_Branch_TenantId",
                table: "Branches",
                newName: "IX_Branches_TenantId");

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Warehouses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Warehouses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Branches",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Branches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Branches",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Branches",
                table: "Branches",
                column: "Id");

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

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CountryId",
                table: "Branches",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Countries_CountryId",
                table: "Branches",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Tenants_TenantId",
                table: "Branches",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Branches_BranchId",
                table: "Warehouses",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Countries_CountryId",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Tenants_TenantId",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Branches_BranchId",
                table: "Warehouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Branches",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_CountryId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Branches");

            migrationBuilder.RenameTable(
                name: "Branches",
                newName: "Branch");

            migrationBuilder.RenameIndex(
                name: "IX_Branches_TenantId",
                table: "Branch",
                newName: "IX_Branch_TenantId");

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Warehouses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Warehouses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Warehouses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Warehouses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Warehouses",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisiblePassword",
                schema: "dbo",
                table: "Auth_Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Branch",
                table: "Branch",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Shar_Branches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BackAccount = table.Column<int>(type: "int", nullable: false),
                    BoxAccount = table.Column<int>(type: "int", nullable: false),
                    CenterPriceNumber = table.Column<int>(type: "int", nullable: false),
                    Chekat = table.Column<int>(type: "int", nullable: false),
                    ClientsAccount = table.Column<int>(type: "int", nullable: false),
                    DeleteBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpensesAccount = table.Column<int>(type: "int", nullable: false),
                    InsertBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    KeyNetAccount = table.Column<int>(type: "int", nullable: false),
                    Master = table.Column<int>(type: "int", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameTr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseAccount = table.Column<int>(type: "int", nullable: false),
                    PurchasingExpenses = table.Column<int>(type: "int", nullable: false),
                    ReturnedPurchase = table.Column<int>(type: "int", nullable: false),
                    ReturnedSales = table.Column<int>(type: "int", nullable: false),
                    SalesAccount = table.Column<int>(type: "int", nullable: false),
                    SalesDiscountWithSub = table.Column<int>(type: "int", nullable: false),
                    SalesWithAccount = table.Column<int>(type: "int", nullable: false),
                    SuppliersAccount = table.Column<int>(type: "int", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shar_Branches", x => x.Id);
                });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "VisiblePassword" },
                values: new object[] { "8ca9568c-95e4-40c9-8681-a8a5d8a78bf7", "AQAAAAIAAYagAAAAEHEYiLABpDu8nWxGK0SqiGrFzSvL1Oqz4ZF3YvKT2en3C1zHWnuxaaUkJdSl/iF0yQ==", "a995361b-36ee-401d-a57d-c654d618f49b", "devsuperadmin96" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "VisiblePassword" },
                values: new object[] { "690a9cae-5821-49fa-81f9-1286b825b8ed", "AQAAAAIAAYagAAAAEPUliOA3TPorwAbmX6+qcmlWI7epuTK9i2XzT6woOxSSlcUuFMhCi8jzmu4TQG6Cnw==", "f518003a-fc4a-4f07-8999-ac9e249dfacd", "devadmin96" });

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_TenantId",
                table: "Warehouses",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branch_Tenants_TenantId",
                table: "Branch",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Branch_BranchId",
                table: "Warehouses",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Tenants_TenantId",
                table: "Warehouses",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");
        }
    }
}
