using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Orange.Services.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateAndSeedCouponTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CouponCode = table.Column<string>(type: "TEXT", nullable: false),
                    CouponAmount = table.Column<double>(type: "REAL", nullable: false),
                    MinAmount = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "CouponAmount", "CouponCode", "CreatedAt", "MinAmount" },
                values: new object[,]
                {
                    { 1, 12.4, "A123", null, 30 },
                    { 2, 5.0, "B123", null, 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
