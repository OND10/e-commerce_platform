using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCart.API.Migrations
{
    public partial class ProductMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberofProduct = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "ImageUrl", "Name", "NumberofProduct", "Price" },
                values: new object[,]
                {
                    { 1, "Phones", "Description about products", "https://placehold.co/603x403", "Product1", 0, 15.0 },
                    { 2, "Phones", "Description about products", "https://placehold.co/603x403", "Product2", 0, 45.0 },
                    { 3, "Phones", "Description about products", "https://placehold.co/603x403", "Product3", 0, 60.0 },
                    { 4, "Phones", "Description about products", "https://placehold.co/603x403", "Product4", 0, 136.0 },
                    { 5, "Phones", "Description about products", "https://placehold.co/603x403", "Product5", 0, 6.0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
