using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ErpManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Q1_AddCurrenciesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Create the Currencies table first
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DecimalDigits = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InsertBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            // Step 2: Seed currencies
            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "DecimalDigits", "DeleteBy", "DeleteDate", "InsertBy", "InsertDate", "IsActive", "IsDeleted", "Symbol", "UpdateBy", "UpdateDate" },
                values: new object[,]
                {
                    { 1, "EGP", 2, null, null, null, null, true, false, "E£", null, null },
                    { 2, "USD", 2, null, null, null, null, true, false, "$", null, null },
                    { 3, "EUR", 2, null, null, null, null, true, false, "€", null, null },
                    { 4, "SAR", 2, null, null, null, null, true, false, "﷼", null, null }
                });

            // Step 3: Add CurrencyId to Tenants with a safe temporary default
            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Tenants",
                type: "int",
                nullable: false,
                defaultValue: 1);

            // Step 4: Backfill CurrencyId from the existing CurrencyCode BEFORE we drop it
            migrationBuilder.Sql(
                "UPDATE [Tenants] SET [CurrencyId] = CASE [CurrencyCode] " +
                "WHEN 'USD' THEN 2 " +
                "WHEN 'EUR' THEN 3 " +
                "WHEN 'SAR' THEN 4 " +
                "ELSE 1 " +  // EGP or any other unknown defaults to EGP (Id=1)
                "END;");

            // Step 5: Now it is safe to drop CurrencyCode
            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "Tenants");

            // Step 6: Update seed data user hashes
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9282c7b6-920b-41ff-8cb1-3b34c229e189", "AQAAAAIAAYagAAAAEFrqIMVcFBNq07/GDZ4AXLbpayKvg2OYn0PDDZxR0ECNdRAxsTWu8l1sVi/EZeKZfQ==", "82ef5d80-de2b-4698-8c4f-7b19308345f5" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "242e0be4-52c0-4f76-b333-695673ddee5d", "AQAAAAIAAYagAAAAEKUzADrKxFTA2l4eXZFxV7hDFfda+xcqHKOT7WLISRJhDa0NOqd3Z8JQsRu/xMKQIg==", "e6601c70-a3ca-4d76-b615-6b3dbe41ee75" });

            // Step 7: The seeded tenant (Id=1) was already handled by the backfill above.
            //         This explicit update ensures the EF seed state is consistent.
            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: 1,
                column: "CurrencyId",
                value: 1);

            // Step 8: Index + FK now that all rows have valid CurrencyId
            migrationBuilder.CreateIndex(
                name: "IX_Tenants_CurrencyId",
                table: "Tenants",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_Currencies_CurrencyId",
                table: "Tenants",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_Currencies_CurrencyId",
                table: "Tenants");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_CurrencyId",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Tenants");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "Tenants",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

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
                column: "CurrencyCode",
                value: "EGP");
        }
    }
}
