namespace PhoneBook.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class InitStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BookEntryStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("9eabc1db-7b45-43e8-854b-a13e7383a745"), "Актуально" });

            migrationBuilder.InsertData(
                table: "BookEntryStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("42f0a156-330e-4aa8-9605-8a171903796e"), "Требует подтверждения" });

            migrationBuilder.InsertData(
                table: "BookEntryStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("48b09bb2-3145-4e38-9174-cac1485f9ed5"), "Нектуально" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookEntryStatuses",
                keyColumn: "Id",
                keyValue: new Guid("42f0a156-330e-4aa8-9605-8a171903796e"));

            migrationBuilder.DeleteData(
                table: "BookEntryStatuses",
                keyColumn: "Id",
                keyValue: new Guid("48b09bb2-3145-4e38-9174-cac1485f9ed5"));

            migrationBuilder.DeleteData(
                table: "BookEntryStatuses",
                keyColumn: "Id",
                keyValue: new Guid("9eabc1db-7b45-43e8-854b-a13e7383a745"));
        }
    }
}
