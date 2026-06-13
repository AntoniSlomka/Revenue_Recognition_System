using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Revenue_Recognition_System.Migrations
{
    /// <inheritdoc />
    public partial class Seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Software used for creating different types of graphic media.", "Graphic Design" },
                    { 2, "Software used for managing accounting in a company.", "Accounting" },
                    { 3, "Software used for creating 3D models.", "3D Design" },
                    { 4, "Software used for creating audio tracks.", "Audio Design" }
                });

            migrationBuilder.InsertData(
                table: "Softwares",
                columns: new[] { "SoftwareId", "CategoryId", "Description", "Name", "OneYearPrice" },
                values: new object[,]
                {
                    { 1, 1, "PhotoMarket is a picture editing software. (All the funcionalities of Photoshop included)", "PhotoMarket", 650.0m },
                    { 2, 2, "Accountant3000 is a super easy to use accounting software.", "Accountant3000", 840.0m },
                    { 3, 1, "IllustrationMaker allows to make beatiful illustrations in no time.", "IllustrationMaker", 780.0m },
                    { 4, 3, "Blonder is a versatile tool for 3D design.", "Blonder", 960.0m }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "DiscountId", "ActiveFrom", "ActiveTo", "DiscountName", "DiscountValue", "SoftwareId" },
                values: new object[,]
                {
                    { 1, new DateTime(2000, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2000, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "PhotoMarket summer discount", 0.10m, 1 },
                    { 2, new DateTime(2000, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2000, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Accountant3000 summer discount", 0.05m, 2 },
                    { 3, new DateTime(2000, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2000, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "IllustrationMaker summer discount", 0.10m, 3 },
                    { 4, new DateTime(2000, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2000, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "IllustrationMaker back to school discount", 0.15m, 3 }
                });

            migrationBuilder.InsertData(
                table: "SoftwareVersions",
                columns: new[] { "VersionId", "Description", "ReleaseDate", "SoftwareId", "VersionName" },
                values: new object[,]
                {
                    { 1, "Initial Release", new DateTime(2020, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "1.0.0" },
                    { 2, "Added filters and layer support", new DateTime(2021, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "1.5.0" },
                    { 3, "Major UI overhaul and performance improvements", new DateTime(2022, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "2.0.0" },
                    { 4, "Initial Release", new DateTime(2019, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "1.0.0" },
                    { 5, "Added tax report generation", new DateTime(2020, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "1.2.0" },
                    { 6, "Cloud sync and multi-currency support", new DateTime(2023, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "2.0.0" },
                    { 7, "Initial Release", new DateTime(2021, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "1.0.0" },
                    { 8, "Added vector tools and brush library", new DateTime(2022, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "1.3.0" },
                    { 9, "AI-assisted drawing and new export formats", new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "2.0.0" },
                    { 10, "Initial Release", new DateTime(2018, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "1.0.0" },
                    { 11, "Added sculpting tools and material editor", new DateTime(2020, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "1.4.0" },
                    { 12, "Real-time rendering and physics simulation", new DateTime(2024, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "3.0.0" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "DiscountId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "DiscountId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "DiscountId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "DiscountId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SoftwareVersions",
                keyColumn: "VersionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SoftwareVersions",
                keyColumn: "VersionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SoftwareVersions",
                keyColumn: "VersionId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SoftwareVersions",
                keyColumn: "VersionId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SoftwareVersions",
                keyColumn: "VersionId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "SoftwareVersions",
                keyColumn: "VersionId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "SoftwareVersions",
                keyColumn: "VersionId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "SoftwareVersions",
                keyColumn: "VersionId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "SoftwareVersions",
                keyColumn: "VersionId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "SoftwareVersions",
                keyColumn: "VersionId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "SoftwareVersions",
                keyColumn: "VersionId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "SoftwareVersions",
                keyColumn: "VersionId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Softwares",
                keyColumn: "SoftwareId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Softwares",
                keyColumn: "SoftwareId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Softwares",
                keyColumn: "SoftwareId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Softwares",
                keyColumn: "SoftwareId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);
        }
    }
}
