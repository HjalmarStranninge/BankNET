using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankNET.Migrations
{
    public partial class UpdateProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "SSN",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Accounts",
                newName: "AccountName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "SSN");

            migrationBuilder.RenameColumn(
                name: "AccountName",
                table: "Accounts",
                newName: "Name");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
