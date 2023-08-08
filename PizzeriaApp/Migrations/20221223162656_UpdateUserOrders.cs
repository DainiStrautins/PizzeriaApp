using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PizzeriaApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderDetails_OrdersDetailsId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOrders_Order_OrderId",
                table: "UserOrders");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "19de330c-ac7d-4033-9ecd-d0703bbb71d2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45d878f1-733c-4761-8ddc-da609ef7eed3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89fc4e94-c315-4e83-81ee-50deebb6912a");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "UserOrders",
                newName: "OrderDetailsId");

            migrationBuilder.RenameIndex(
                name: "IX_UserOrders_OrderId",
                table: "UserOrders",
                newName: "IX_UserOrders_OrderDetailsId");

            migrationBuilder.RenameColumn(
                name: "OrdersDetailsId",
                table: "Order",
                newName: "UserOrdersId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_OrdersDetailsId",
                table: "Order",
                newName: "IX_Order_UserOrdersId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "acb7d6cd-f30b-4c78-8ad5-a4ece108c4b2", "431daf77-5537-40c9-894a-edfcf049962f", "Guest", "GUEST" },
                    { "d6c90b8a-f374-4796-b6bf-cf5f1f787844", "452141fe-e686-4b5d-8cf9-4c0f5f1c7e93", "Admin", "ADMIN" },
                    { "e9da7436-f1cd-4e14-80ad-8004ec13d340", "da37c78e-12de-4174-a573-763c9152607a", "Customer", "CUSTOMER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Order_UserOrders_UserOrdersId",
                table: "Order",
                column: "UserOrdersId",
                principalTable: "UserOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrders_OrderDetails_OrderDetailsId",
                table: "UserOrders",
                column: "OrderDetailsId",
                principalTable: "OrderDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_UserOrders_UserOrdersId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOrders_OrderDetails_OrderDetailsId",
                table: "UserOrders");

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

            migrationBuilder.RenameColumn(
                name: "OrderDetailsId",
                table: "UserOrders",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_UserOrders_OrderDetailsId",
                table: "UserOrders",
                newName: "IX_UserOrders_OrderId");

            migrationBuilder.RenameColumn(
                name: "UserOrdersId",
                table: "Order",
                newName: "OrdersDetailsId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_UserOrdersId",
                table: "Order",
                newName: "IX_Order_OrdersDetailsId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "19de330c-ac7d-4033-9ecd-d0703bbb71d2", "cfe0f5cc-b9d1-4478-8a89-feef6b757332", "Admin", "ADMIN" },
                    { "45d878f1-733c-4761-8ddc-da609ef7eed3", "f61d3312-0c22-4fb4-874a-61f3afe2d933", "Customer", "CUSTOMER" },
                    { "89fc4e94-c315-4e83-81ee-50deebb6912a", "98ae8e96-c506-460b-b60b-3b08d6cfe45b", "Guest", "GUEST" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderDetails_OrdersDetailsId",
                table: "Order",
                column: "OrdersDetailsId",
                principalTable: "OrderDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrders_Order_OrderId",
                table: "UserOrders",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");
        }
    }
}
