using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class G1_AddGymModule : Migration
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
                values: new object[] { "9d90127b-2e4e-4898-baaf-761d5fe4468a", "AQAAAAIAAYagAAAAEPO0ulP8B0c0iyAXWuCmR53YR1/1spD1zZMVWfEDwgssSaQrSZyiXS9XWNH1twySvA==", "a46dd532-28f9-4e6d-ba36-0f10eec5a4b8" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6fa77497-95da-41aa-b8ee-f66bfd86e95c", "AQAAAAIAAYagAAAAEOz5xmcVP5h7UFBVdrLudl6vaIIcZNhiNPDxODwFDmwd0f4V0M4bnngpX+3WnuKcNw==", "c3cafe9d-f327-4758-8169-858dee1de7e5" });
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
                values: new object[] { "7f232b04-bc72-489e-af10-4584466e3ad3", "AQAAAAIAAYagAAAAEIIRRP7KduNo9prDbaY7DEsTLVRgUMwWY+45xQRB/WSMAIguX4uUva6wKzR4RdSilQ==", "48d389c5-1f39-426b-b47d-3e8856c73375" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8d028216-aeba-4560-9593-809244ac80eb", "AQAAAAIAAYagAAAAEMoDLMG3QWg6dwH9mYubj7ioq3wNOP4Y3RkSvTHJtpDhRtYKyDdAVAqWDwf3DwJgpw==", "f0051c80-1600-4627-8577-8a481cf688d5" });
        }
    }
}
