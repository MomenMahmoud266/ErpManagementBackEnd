using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumnsInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeleteBy",
                schema: "dbo",
                table: "Auth_Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                schema: "dbo",
                table: "Auth_Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailConfirmToken",
                schema: "dbo",
                table: "Auth_Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsertBy",
                schema: "dbo",
                table: "Auth_Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDate",
                schema: "dbo",
                table: "Auth_Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "dbo",
                table: "Auth_Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdateBy",
                schema: "dbo",
                table: "Auth_Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "dbo",
                table: "Auth_Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb1",
                columns: new[] { "ConcurrencyStamp", "DeleteBy", "DeleteDate", "EmailConfirmToken", "InsertBy", "InsertDate", "IsDeleted", "PasswordHash", "SecurityStamp", "UpdateBy", "UpdateDate" },
                values: new object[] { "305b6d34-2156-4362-91d1-32a264f6478c", null, null, null, null, null, false, "AQAAAAIAAYagAAAAEKX0JwzJVdqIbMCjnN+pfyaRgbn1tfqviE0jB7gaqeFwXzs8Y/TIafIRO44LnPGMUA==", "dfce0f08-99b6-4006-abc5-250a34c91d49", null, null });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb2",
                columns: new[] { "ConcurrencyStamp", "DeleteBy", "DeleteDate", "EmailConfirmToken", "InsertBy", "InsertDate", "IsDeleted", "PasswordHash", "SecurityStamp", "UpdateBy", "UpdateDate" },
                values: new object[] { "8f205b72-2fb7-4738-8f47-e969bb5c451a", null, null, null, null, null, false, "AQAAAAIAAYagAAAAEHy8p81jZWdd4YfM8sNT9CDoIJu4Z2EeziJysLJP5bvGbHTZmexpKKxVbcX1c9VXzg==", "ac4db3ed-5bed-4887-817d-9a3ecb697c6f", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteBy",
                schema: "dbo",
                table: "Auth_Users");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                schema: "dbo",
                table: "Auth_Users");

            migrationBuilder.DropColumn(
                name: "EmailConfirmToken",
                schema: "dbo",
                table: "Auth_Users");

            migrationBuilder.DropColumn(
                name: "InsertBy",
                schema: "dbo",
                table: "Auth_Users");

            migrationBuilder.DropColumn(
                name: "InsertDate",
                schema: "dbo",
                table: "Auth_Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "dbo",
                table: "Auth_Users");

            migrationBuilder.DropColumn(
                name: "UpdateBy",
                schema: "dbo",
                table: "Auth_Users");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "dbo",
                table: "Auth_Users");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "55266841-33ed-4b02-9930-840d56d21346", "AQAAAAIAAYagAAAAEHcZ+nuDtk8CHA19TVfdH4xt0d7MSXX2C/ViaOeNe7MMPbe2kJxxwcKe4AUfUOoCqQ==", "0bd80c70-d87a-4bbf-ba0e-f4ea54cd1200" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Auth_Users",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5basb2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "61013d74-a97b-4fcf-b35b-622b60277460", "AQAAAAIAAYagAAAAEGk3zcDpHKoNZwTpUft76nCF/X++zwMLXGQt914a6Fn6qF7n8luwC7IqzUP+FZ05WQ==", "be1eb304-09c5-4268-b073-4d631562efae" });
        }
    }
}
