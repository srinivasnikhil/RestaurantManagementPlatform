using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategoriesandMenudata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "MenuItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DisplayOrder", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, 1, true, "Dosa" },
                    { 2, 2, true, "Biryani & Pulao" },
                    { 3, 3, true, "Idly" },
                    { 4, 4, true, "Uttapam Spcls" },
                    { 5, 5, true, "Curries" },
                    { 6, 6, true, "Indian Bread" },
                    { 7, 7, true, "Appetizers - Veg" },
                    { 8, 8, true, "Appetizers - Non Veg" },
                    { 9, 9, true, "Fried Rice / Noodles" },
                    { 10, 10, true, "Frankie's" },
                    { 11, 11, true, "South Indian Spcl Snacks" },
                    { 12, 12, true, "Mandi" },
                    { 13, 13, true, "Drinks" }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "CategoryId", "Description", "DisplayOrder", "ImageUrl", "IsAvailable", "IsVeg", "Name", "Price", "SpiceLevel" },
                values: new object[,]
                {
                    { 1, 1, "Crisp rice and lentil crepe.", 1, "/images/menu/plain-dosa.jpg", true, true, "Plain Dosa", 6.99m, 0 },
                    { 2, 1, "Dosa filled with spiced potato masala.", 2, "/images/menu/masala-dosa.jpg", true, true, "Masala Dosa", 6.99m, 0 },
                    { 3, 1, "Dosa roasted in clarified butter.", 3, "/images/menu/ghee-dosa.jpg", true, true, "Ghee Dosa", 7.49m, 0 },
                    { 4, 1, "Crisp semolina dosa with onions and green chili.", 4, "/images/menu/rava-dosa.jpg", true, true, "Onion Rava Dosa", 8.99m, 0 },
                    { 5, 2, "Spicy Vijayawada style chicken biryani.", 1, "/images/menu/vijayawada-biryani.jpg", true, false, "Vijayawada Biryani", 17.99m, 0 },
                    { 6, 2, "Basmati rice with seasonal vegetables and spices.", 2, "/images/menu/veg-biryani.jpg", true, true, "Veg Dum Biryani", 14.99m, 0 },
                    { 7, 2, "Slow-cooked layered chicken biryani.", 3, "/images/menu/chicken-biryani.jpg", true, false, "Chicken Dum Biryani", 16.99m, 0 },
                    { 8, 2, "Fragrant rice tossed with paneer and spices.", 4, "/images/menu/paneer-pulao.jpg", true, true, "Paneer Pulao", 14.49m, 0 },
                    { 9, 3, "Two steamed rice cakes with chutney and sambar.", 1, "/images/menu/plain-idly.jpg", true, true, "Plain Idly", 4.99m, 0 },
                    { 10, 3, "Idly tossed in ghee and spicy podi.", 2, "/images/menu/karam-idly.jpg", true, true, "Ghee Karam Idly", 6.99m, 0 },
                    { 11, 4, "Thick savory pancake.", 1, "/images/menu/plain-uttapam.jpg", true, true, "Plain Uttapam", 6.99m, 0 },
                    { 12, 4, "Uttapam topped with onions and chili.", 2, "/images/menu/onion-uttapam.jpg", true, true, "Onion Uttapam", 7.99m, 0 },
                    { 13, 5, "Paneer in a rich tomato butter gravy.", 1, "/images/menu/paneer-butter-masala.jpg", true, true, "Paneer Butter Masala", 13.99m, 0 },
                    { 14, 5, "Home-style chicken curry.", 2, "/images/menu/chicken-curry.jpg", true, false, "Chicken Curry", 14.99m, 0 },
                    { 15, 6, "Thin handkerchief bread served with curry.", 1, "/images/menu/rumali-roti.jpg", true, true, "Rumali Roti with Curry", 9.99m, 0 },
                    { 16, 6, "Tandoor-baked naan brushed with butter.", 2, "/images/menu/butter-naan.jpg", true, true, "Butter Naan", 3.99m, 0 },
                    { 17, 7, "Crispy cauliflower in tangy Manchurian sauce.", 1, "/images/menu/gobi-manchurian.jpg", true, true, "Gobi Manchurian", 11.99m, 0 },
                    { 18, 7, "Two pastry pockets with spiced potato.", 2, "/images/menu/samosa.jpg", true, true, "Punjabi Samosa", 3.99m, 0 },
                    { 19, 8, "Spicy deep-fried chicken bites.", 1, "/images/menu/chicken-65.jpg", true, false, "Chicken 65", 13.99m, 0 },
                    { 20, 8, "Crispy fried fish tossed in chili and curry leaf.", 2, "/images/menu/apollo-fish.jpg", true, false, "Apollo Fish", 15.99m, 0 },
                    { 21, 9, "Wok-tossed street style noodles.", 1, "/images/menu/fried-noodles.jpg", true, true, "Street Style Fried Noodles", 14.99m, 0 },
                    { 22, 9, "Indian street style chicken fried rice.", 2, "/images/menu/chicken-fried-rice.jpg", true, false, "Chicken Fried Rice", 14.99m, 0 },
                    { 23, 10, "Roll stuffed with spiced paneer.", 1, "/images/menu/paneer-frankie.jpg", true, true, "Paneer Frankie", 9.99m, 0 },
                    { 24, 10, "Roll stuffed with spiced chicken.", 2, "/images/menu/chicken-frankie.jpg", true, false, "Chicken Frankie", 10.99m, 0 },
                    { 25, 11, "Ghee and spice tossed fried fritters.", 1, "/images/menu/punugulu.jpg", true, true, "Neyyi Karam Punugulu", 10.99m, 0 },
                    { 26, 11, "Soft fried lentil dumplings.", 2, "/images/menu/mysore-bonda.jpg", true, true, "Mysore Bonda", 9.99m, 0 },
                    { 27, 12, "Smoky spiced rice with chicken, Yemeni style.", 1, "/images/menu/chicken-mandi.jpg", true, false, "Chicken Mandi", 18.99m, 0 },
                    { 28, 13, "Signature sweet milky tea.", 1, "/images/menu/irani-chai.jpg", true, true, "Irani Chai", 2.99m, 0 },
                    { 29, 13, "Chilled yogurt and mango drink.", 2, "/images/menu/mango-lassi.jpg", true, true, "Mango Lassi", 4.99m, 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "MenuItems");
        }
    }
}
