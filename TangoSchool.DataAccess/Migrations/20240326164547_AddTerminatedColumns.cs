using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TangoSchool.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTerminatedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Terminated",
                table: "Lessons",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Terminated",
                table: "LessonRequests",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Terminated",
                table: "Groups",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Terminated",
                table: "Classrooms",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Terminated",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Terminated",
                table: "LessonRequests");

            migrationBuilder.DropColumn(
                name: "Terminated",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Terminated",
                table: "Classrooms");
        }
    }
}
