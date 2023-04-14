using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantAppOOP.Migrations
{
    /// <inheritdoc />
    public partial class RestM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    cost = table.Column<decimal>(type: "decimal(10,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Menu__3213E83F23E41A07", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Waiters",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nameWaiter = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Waiters__3213E83F495371ED", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idWaiter = table.Column<int>(type: "int", nullable: false),
                    dateOrder = table.Column<DateTime>(type: "date", nullable: false),
                    numberOfTable = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tmp_ms_x__3213E83F8FBF43D6", x => x.id);
                    table.ForeignKey(
                        name: "FK_Waiter",
                        column: x => x.idWaiter,
                        principalTable: "Waiters",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "OrderedDish",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idOrder = table.Column<int>(type: "int", nullable: false),
                    idMenu = table.Column<int>(type: "int", nullable: false),
                    number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderedD__3213E83F5BBA48BA", x => x.id);
                    table.ForeignKey(
                        name: "FK_Menu",
                        column: x => x.idMenu,
                        principalTable: "Menu",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order",
                        column: x => x.idOrder,
                        principalTable: "Orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderedDish_idMenu",
                table: "OrderedDish",
                column: "idMenu");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedDish_idOrder",
                table: "OrderedDish",
                column: "idOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_idWaiter",
                table: "Orders",
                column: "idWaiter");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderedDish");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Waiters");
        }
    }
}
