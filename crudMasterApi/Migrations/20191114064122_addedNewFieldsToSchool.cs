using Microsoft.EntityFrameworkCore.Migrations;

namespace CrudMasterApi.Migrations
{
    public partial class addedNewFieldsToSchool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2,
                column: "NekiBool",
                value: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2,
                column: "NekiBool",
                value: true);
        }
    }
}
