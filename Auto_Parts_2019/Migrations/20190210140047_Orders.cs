using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auto_Parts_2019.Migrations
{
    public partial class Orders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "OrdersDTO");

            migrationBuilder.DropTable(
                name: "CartLine");

            migrationBuilder.CreateTable(
                name: "_Orders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SessionID = table.Column<string>(nullable: true),
                    OrderID = table.Column<string>(nullable: true),
                    PartID = table.Column<int>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    Brand = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Group_Parts = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    AddressID = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Sity = table.Column<string>(nullable: true),
                    Avenue = table.Column<string>(nullable: true),
                    IP = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    OneCCreate = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Orders", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "_OrdersDTO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrderID = table.Column<string>(nullable: true),
                    PartID = table.Column<int>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    Brand = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Foto_link = table.Column<string>(nullable: true),
                    Group_Parts = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Analogues = table.Column<string>(nullable: true),
                    Group_Auto = table.Column<string>(nullable: true),
                    AddressID = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Sity = table.Column<string>(nullable: true),
                    Avenue = table.Column<string>(nullable: true),
                    IP = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrdersDTO", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_Orders");

            migrationBuilder.DropTable(
                name: "_OrdersDTO");

            migrationBuilder.CreateTable(
                name: "CartLine",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateLastModified = table.Column<DateTime>(nullable: false),
                    PartID = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartLine", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CartLine_Parts_PartID",
                        column: x => x.PartID,
                        principalTable: "Parts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrdersDTO",
                columns: table => new
                {
                    OrderID = table.Column<string>(nullable: false),
                    AddressID = table.Column<string>(nullable: true),
                    Analogues = table.Column<string>(nullable: true),
                    Avenue = table.Column<string>(nullable: true),
                    Brand = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Foto_link = table.Column<string>(nullable: true),
                    Group_Auto = table.Column<string>(nullable: true),
                    Group_Parts = table.Column<string>(nullable: true),
                    IP = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    PartID = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Sity = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersDTO", x => x.OrderID);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressId = table.Column<string>(nullable: true),
                    CartLineID = table.Column<int>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    OneCCreate = table.Column<bool>(nullable: false),
                    PartID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_AddressId",
                        column: x => x.AddressId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_CartLine_CartLineID",
                        column: x => x.CartLineID,
                        principalTable: "CartLine",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Parts_PartID",
                        column: x => x.PartID,
                        principalTable: "Parts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartLine_PartID",
                table: "CartLine",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AddressId",
                table: "Orders",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CartLineID",
                table: "Orders",
                column: "CartLineID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PartID",
                table: "Orders",
                column: "PartID");
        }
    }
}
