using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PizzeriaApp.Migrations
{
    /// <inheritdoc />
    public partial class AddDelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "acb7d6cd-f30b-4c78-8ad5-a4ece108c4b2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d6c90b8a-f374-4796-b6bf-cf5f1f787844");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e9da7436-f1cd-4e14-80ad-8004ec13d340");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryId",
                table: "UserOrders",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeliveryType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Delivery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeliveryTypeId = table.Column<int>(type: "integer", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "text", nullable: false),
                    ContactName = table.Column<string>(type: "text", nullable: false),
                    ContactPhone = table.Column<string>(type: "text", nullable: false),
                    Instructions = table.Column<string>(type: "text", nullable: false),
                    TrackingNumber = table.Column<string>(type: "text", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delivery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Delivery_DeliveryType_DeliveryTypeId",
                        column: x => x.DeliveryTypeId,
                        principalTable: "DeliveryType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "32b6009e-3cf5-4ae1-9d69-83f21b290d97", "46d8b2f9-14de-46f8-a554-9ec80aa9f4c9", "Admin", "ADMIN" },
                    { "f6daa3f4-f11d-4333-9993-d089cabee71f", "b8c29f8c-eaaa-46db-a55a-45487afefbf5", "Customer", "CUSTOMER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserOrders_DeliveryId",
                table: "UserOrders",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_DeliveryTypeId",
                table: "Delivery",
                column: "DeliveryTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrders_Delivery_DeliveryId",
                table: "UserOrders",
                column: "DeliveryId",
                principalTable: "Delivery",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserOrders_Delivery_DeliveryId",
                table: "UserOrders");

            migrationBuilder.DropTable(
                name: "Delivery");

            migrationBuilder.DropTable(
                name: "DeliveryType");

            migrationBuilder.DropIndex(
                name: "IX_UserOrders_DeliveryId",
                table: "UserOrders");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32b6009e-3cf5-4ae1-9d69-83f21b290d97");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6daa3f4-f11d-4333-9993-d089cabee71f");

            migrationBuilder.DropColumn(
                name: "DeliveryId",
                table: "UserOrders");

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserOrdersId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalPrice = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_UserOrders_UserOrdersId",
                        column: x => x.UserOrdersId,
                        principalTable: "UserOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "acb7d6cd-f30b-4c78-8ad5-a4ece108c4b2", "431daf77-5537-40c9-894a-edfcf049962f", "Guest", "GUEST" },
                    { "d6c90b8a-f374-4796-b6bf-cf5f1f787844", "452141fe-e686-4b5d-8cf9-4c0f5f1c7e93", "Admin", "ADMIN" },
                    { "e9da7436-f1cd-4e14-80ad-8004ec13d340", "da37c78e-12de-4174-a573-763c9152607a", "Customer", "CUSTOMER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserOrdersId",
                table: "Order",
                column: "UserOrdersId");
        }
    }
}
