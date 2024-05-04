using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TangoSchool.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddAdditionalTermainatedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Terminated",
                table: "Teachers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Terminated",
                table: "Students",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Terminated",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Terminated",
                table: "Students");
        }
    }
}
