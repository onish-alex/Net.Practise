namespace EF.Practice.CodeFirst.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonType",
                schema: "Person",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonType",
                schema: "Person",
                table: "Person");
        }
    }
}
