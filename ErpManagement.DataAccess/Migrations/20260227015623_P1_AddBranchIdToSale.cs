using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class P1_AddBranchIdToSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add BranchId as NULLABLE to avoid default-value FK violations
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Sales",
                type: "int",
                nullable: true);

            // Step 2: Backfill BranchId from Warehouse.BranchId where WarehouseId is known
            migrationBuilder.Sql(@"
                UPDATE s
                SET s.BranchId = w.BranchId
                FROM Sales s
                INNER JOIN Warehouses w ON w.Id = s.WarehouseId
                WHERE s.WarehouseId IS NOT NULL AND s.BranchId IS NULL;
            ");

            // Step 3: Backfill remaining rows using the tenant's first active branch
            migrationBuilder.Sql(@"
                UPDATE s
                SET s.BranchId = (
                    SELECT TOP 1 b.Id
                    FROM Branches b
                    WHERE b.TenantId = s.TenantId
                      AND b.IsActive = 1
                    ORDER BY b.Id ASC
                )
                FROM Sales s
                WHERE s.BranchId IS NULL;
            ");

            // Step 4: For tenants that still have NULL BranchId (sales exist but no active branch),
            //         insert a 'Default Branch' per tenant so the FK can be satisfied.
            migrationBuilder.Sql(@"
                INSERT INTO [Branches]
                    ([TenantId],[NameEn],[NameAr],[NameTr],[CountryId],[City],[ZipCode],
                     [Address],[Phone],[IsActive],[IsDeleted],[InsertDate],[UpdateDate],
                     [DeleteDate],[InsertBy],[UpdateBy],[DeleteBy])
                SELECT DISTINCT
                    s.[TenantId],
                    N'Default Branch', NULL, NULL,
                    COALESCE(
                        (SELECT TOP 1 [Id] FROM [Countries] WHERE [IsActive] = 1 ORDER BY [Id] ASC),
                        (SELECT TOP 1 [Id] FROM [Countries] ORDER BY [Id] ASC),
                        1
                    ),
                    NULL, NULL, NULL, NULL,
                    1, 0, GETUTCDATE(), NULL, NULL, NULL, NULL, NULL
                FROM [Sales] s
                WHERE s.[BranchId] IS NULL
                  AND NOT EXISTS (
                      SELECT 1 FROM [Branches] b
                      WHERE b.[TenantId] = s.[TenantId]
                  );
            ");

            // Step 5: Final backfill — pick any branch for tenants that still have NULL
            migrationBuilder.Sql(@"
                UPDATE s
                SET s.BranchId = (
                    SELECT TOP 1 b.Id
                    FROM Branches b
                    WHERE b.TenantId = s.TenantId
                    ORDER BY b.Id ASC
                )
                FROM Sales s
                WHERE s.BranchId IS NULL;
            ");

            // Step 6: Now that every row has a valid BranchId, make the column NOT NULL
            migrationBuilder.Sql("ALTER TABLE [Sales] ALTER COLUMN [BranchId] int NOT NULL;");

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

            migrationBuilder.CreateIndex(
                name: "IX_Sales_BranchId",
                table: "Sales",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_TenantId_BranchId_SaleDate",
                table: "Sales",
                columns: new[] { "TenantId", "BranchId", "SaleDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Branches_BranchId",
                table: "Sales",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Branches_BranchId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_TenantId_BranchId_SaleDate",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_BranchId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Sales");

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
        }
    }
}
