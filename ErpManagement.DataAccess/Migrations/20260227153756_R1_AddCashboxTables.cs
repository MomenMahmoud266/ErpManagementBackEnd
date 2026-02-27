using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class R1_AddCashboxTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4293435d-5382-4325-898b-b201b7a3d883", "AQAAAAIAAYagAAAAEBjldK2N+ZO2Pg6gl3SeMaRGWvd6rNrUzstvKxsptGWH8kW6W833AEKHqaiD6lMfsw==", "85156f4c-4169-4fb1-b091-ca84aaff6160" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f2c0374d-b758-4be8-adb4-b8289142c99a", "AQAAAAIAAYagAAAAEBI61Bee6Zlxz6zoy40D6Vq10cXdmJ5Ix6em2zqegIcKdJUKRiVr6fSjZATuwQTAhw==", "4794929e-a8f4-4842-ba5f-f28ffdd209cb" });
        }
    }
}
