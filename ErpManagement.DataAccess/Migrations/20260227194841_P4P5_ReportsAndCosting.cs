using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class P4P5_ReportsAndCosting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AverageCost",
                table: "WarehouseProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CostingMethod",
                table: "Tenants",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InventoryMode",
                table: "Tenants",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "CostAtSale",
                table: "SaleItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "InventoryPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BeginningValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchasesValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EndingValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CogsValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InsertBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryPeriods_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PhysicalCounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryPeriodId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CountQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostUsed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InsertBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalCounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhysicalCounts_InventoryPeriods_InventoryPeriodId",
                        column: x => x.InventoryPeriodId,
                        principalTable: "InventoryPeriods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PhysicalCounts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PhysicalCounts_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5cdd2c38-da64-48c4-bb32-763ec20713eb", "AQAAAAIAAYagAAAAEEB/iKYXN2qm3Y1PvbLgTfue9DAKPJA+AlUfoCx2YzNc8UIBSuxkAswUWc+UrAsyVQ==", "8675d572-3915-4da9-9597-19376d324cb5" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7fdcc909-56ee-4651-9f22-9a02e988f435", "AQAAAAIAAYagAAAAEEX/TZvwNQlwSPMLCV4OqIi7cVarFsZ3BNCrR7btz3Ido0Vq7BGaICKQNTVnnox1kw==", "9f21af1c-2a81-4bd4-a814-2005dbc23168" });

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CostingMethod", "InventoryMode" },
                values: new object[] { "Average", "Perpetual" });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryPeriods_BranchId",
                table: "InventoryPeriods",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalCounts_InventoryPeriodId",
                table: "PhysicalCounts",
                column: "InventoryPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalCounts_ProductId",
                table: "PhysicalCounts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalCounts_WarehouseId",
                table: "PhysicalCounts",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhysicalCounts");

            migrationBuilder.DropTable(
                name: "InventoryPeriods");

            migrationBuilder.DropColumn(
                name: "AverageCost",
                table: "WarehouseProducts");

            migrationBuilder.DropColumn(
                name: "CostingMethod",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "InventoryMode",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "CostAtSale",
                table: "SaleItems");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "48cebed0-81be-4fbe-8573-50ae519c7e02", "AQAAAAIAAYagAAAAEF5y9oBjXZ9z12pUrsvED4MyHwlbnyWN+4AmwVHOx1ZrRr/krRa/ze0GCJzgvqa7Xg==", "5b1e5088-145c-45ff-b561-0caa344d7cc6" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fa49744c-eaca-4d52-8c24-54680ee9b9bc", "AQAAAAIAAYagAAAAEFfnxJrKoYP0PJlsCI24ZP8BhyrPqjOdYxWqebcSHg5236Hlsup/k1qXfP9RmomcLA==", "85697434-9f7d-4515-8968-f9533327cb65" });
        }
    }
}
