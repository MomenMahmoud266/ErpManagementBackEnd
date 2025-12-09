using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameExpenseCategoryIdInExpenses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleReturns_Tenants_TenantId",
                table: "SaleReturns");

            migrationBuilder.DropIndex(
                name: "IX_SaleReturns_TenantId",
                table: "SaleReturns");

            migrationBuilder.AddColumn<int>(
                name: "SaleId",
                table: "SaleReturns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseId",
                table: "PurchaseReturns",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Expenses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ExpenseCategoryId",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ExpenseCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameTr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InsertBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCategories", x => x.Id);
                });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8ca9568c-95e4-40c9-8681-a8a5d8a78bf7", "AQAAAAIAAYagAAAAEHEYiLABpDu8nWxGK0SqiGrFzSvL1Oqz4ZF3YvKT2en3C1zHWnuxaaUkJdSl/iF0yQ==", "a995361b-36ee-401d-a57d-c654d618f49b" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "690a9cae-5821-49fa-81f9-1286b825b8ed", "AQAAAAIAAYagAAAAEPUliOA3TPorwAbmX6+qcmlWI7epuTK9i2XzT6woOxSSlcUuFMhCi8jzmu4TQG6Cnw==", "f518003a-fc4a-4f07-8999-ac9e249dfacd" });

            migrationBuilder.CreateIndex(
                name: "IX_SaleReturns_SaleId",
                table: "SaleReturns",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturns_PurchaseId",
                table: "PurchaseReturns",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ExpenseCategoryId",
                table: "Expenses",
                column: "ExpenseCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_ExpenseCategories_ExpenseCategoryId",
                table: "Expenses",
                column: "ExpenseCategoryId",
                principalTable: "ExpenseCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReturns_Purchases_PurchaseId",
                table: "PurchaseReturns",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleReturns_Sales_SaleId",
                table: "SaleReturns",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpenseCategories_ExpenseCategoryId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReturns_Purchases_PurchaseId",
                table: "PurchaseReturns");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleReturns_Sales_SaleId",
                table: "SaleReturns");

            migrationBuilder.DropTable(
                name: "ExpenseCategories");

            migrationBuilder.DropIndex(
                name: "IX_SaleReturns_SaleId",
                table: "SaleReturns");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseReturns_PurchaseId",
                table: "PurchaseReturns");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_ExpenseCategoryId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "SaleId",
                table: "SaleReturns");

            migrationBuilder.DropColumn(
                name: "PurchaseId",
                table: "PurchaseReturns");

            migrationBuilder.DropColumn(
                name: "ExpenseCategoryId",
                table: "Expenses");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2541bad3-ed27-4d43-8f49-0c967c395202", "AQAAAAIAAYagAAAAEGZYf/XTZ0eCBhJwE1UdnnnpkQ8jl8L4ljazxMTlxywi58u5Fv9pdONmSaqxxrgPLQ==", "cb2b8875-c94b-4890-aaa3-687dd11bebdb" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "45ef538f-7966-4a04-93b5-d4e7b001c154", "AQAAAAIAAYagAAAAEB56kclz5oXYNH/A/+AnLcZ1VDzJ3QsEZamr1BS+S4yLj6uBDkj0TYQIs0xk+lgtJA==", "2cf7e258-0106-4641-9ee6-dd19e4adbf4b" });

            migrationBuilder.CreateIndex(
                name: "IX_SaleReturns_TenantId",
                table: "SaleReturns",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleReturns_Tenants_TenantId",
                table: "SaleReturns",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");
        }
    }
}
