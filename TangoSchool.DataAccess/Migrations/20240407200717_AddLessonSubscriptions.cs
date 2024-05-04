using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TangoSchool.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonSubscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonSubscription");

            migrationBuilder.CreateTable(
                name: "LessonSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriptionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonSubscriptions_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonSubscriptions_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonSubscriptions_LessonId",
                table: "LessonSubscriptions",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonSubscriptions_SubscriptionId",
                table: "LessonSubscriptions",
                column: "SubscriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonSubscriptions");

            migrationBuilder.CreateTable(
                name: "LessonSubscription",
                columns: table => new
                {
                    AttendedLessonsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsedSubscriptionsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonSubscription", x => new { x.AttendedLessonsId, x.UsedSubscriptionsId });
                    table.ForeignKey(
                        name: "FK_LessonSubscription_Lessons_AttendedLessonsId",
                        column: x => x.AttendedLessonsId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonSubscription_Subscriptions_UsedSubscriptionsId",
                        column: x => x.UsedSubscriptionsId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonSubscription_UsedSubscriptionsId",
                table: "LessonSubscription",
                column: "UsedSubscriptionsId");
        }
    }
}
