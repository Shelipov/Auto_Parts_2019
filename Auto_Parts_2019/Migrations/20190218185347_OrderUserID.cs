using Microsoft.EntityFrameworkCore.Migrations;

namespace Auto_Parts_2019.Migrations
{
    public partial class OrderUserID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "_Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "_Orders");
        }
    }
}
