using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TangoSchool.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionTemplateIdToSubscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionTemplateId",
                table: "Subscriptions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SubscriptionTemplateId",
                table: "Subscriptions",
                column: "SubscriptionTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_SubscriptionTemplates_SubscriptionTemplateId",
                table: "Subscriptions",
                column: "SubscriptionTemplateId",
                principalTable: "SubscriptionTemplates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_SubscriptionTemplates_SubscriptionTemplateId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_SubscriptionTemplateId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "SubscriptionTemplateId",
                table: "Subscriptions");
        }
    }
}
