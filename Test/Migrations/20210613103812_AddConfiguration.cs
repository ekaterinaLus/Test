using Microsoft.EntityFrameworkCore.Migrations;

namespace Test.Migrations
{
    public partial class AddConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Drinks",
                columns: new[] { "Id", "Amount", "Name", "Quantity", "Volume" },
                values: new object[,]
                {
                    { 1, 33m, "Coca-Cola", 1, 0.5m },
                    { 2, 21m, "Fanta", 13, 1m },
                    { 3, 47m, "Yupi", 51, 0.5m },
                    { 4, 47m, "KateDrink", 51, 0.5m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
