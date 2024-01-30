using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumUsers.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    EmailAddress = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PasswordSalt = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Photo = table.Column<byte[]>(type: "image", nullable: true),
                    Banner = table.Column<byte[]>(type: "image", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "GETDATE()"),
                    ConfirmedEmail = table.Column<bool>(type: "bit", nullable: false),
                    ConfirmedEmailDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    _2FA = table.Column<bool>(name: "2FA", type: "bit", nullable: false),
                    _2FADate = table.Column<DateTime>(name: "2FADate", type: "datetime", nullable: true),
                    Role = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true, defaultValue: "User")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
