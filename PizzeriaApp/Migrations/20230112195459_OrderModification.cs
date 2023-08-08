using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaApp.Migrations
{
    /// <inheritdoc />
    public partial class OrderModification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "OrderDetails");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "UserOrders",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserOrders_OrderId",
                table: "UserOrders",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrders_Orders_OrderId",
                table: "UserOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserOrders_Orders_OrderId",
                table: "UserOrders");

            migrationBuilder.DropIndex(
                name: "IX_UserOrders_OrderId",
                table: "UserOrders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "UserOrders");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "OrderDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
