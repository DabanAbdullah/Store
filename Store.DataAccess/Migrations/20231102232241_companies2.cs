using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Store.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class companies2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "City", "Name", "Phonenumber", "PostalCode", "State", "StreetAdress" },
                values: new object[,]
                {
                    { 1, "Munich", "siemens", "12333333", "90321", "Bavaria", "Maria strasse 1" },
                    { 2, "Erlangen", "T&T", "123438y333333", "90111", "Bavaria", "Leipzig Strasse 12" },
                    { 3, "Nuremberg", "AirBNB", "123647333333", "90431", "Bavaria", "Eberdshardhof" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
