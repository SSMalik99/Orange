using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orange.Services.RewardAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTableRewardsToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RewardDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RewardPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rewards", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rewards");
        }
    }
}
