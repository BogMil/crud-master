using Microsoft.EntityFrameworkCore.Migrations;

namespace CrudMasterApi.Migrations
{
    public partial class addedNewFieldsToCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NekiBool",
                table: "Schools",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "NekiDecimal",
                table: "Schools",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "NekiDouble",
                table: "Schools",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "NekiFloat",
                table: "Schools",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "NekiInt",
                table: "Schools",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "NekiLong",
                table: "Schools",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "NekiBool", "NekiDecimal", "NekiDouble", "NekiFloat", "NekiInt", "NekiLong" },
                values: new object[] { true, 1.1m, 0.11, 0.1f, 1, 1111111111L });

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "NekiBool", "NekiDecimal", "NekiDouble", "NekiFloat", "NekiInt", "NekiLong" },
                values: new object[] { true, 2.2m, 0.22, 0.2f, 2, 2222222222L });

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "NekiBool", "NekiDecimal", "NekiDouble", "NekiFloat", "NekiInt", "NekiLong" },
                values: new object[] { true, 3.3m, 0.33000000000000002, 0.3f, 3, 3333333333L });

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "NekiBool", "NekiDecimal", "NekiDouble", "NekiFloat", "NekiInt", "NekiLong" },
                values: new object[] { true, 4.4m, 0.44, 0.4f, 4, 4444444444L });

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "NekiBool", "NekiDecimal", "NekiDouble", "NekiFloat", "NekiInt", "NekiLong" },
                values: new object[] { true, 5.5m, 0.55000000000000004, 0.5f, 5, 5555555555L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NekiBool",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "NekiDecimal",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "NekiDouble",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "NekiFloat",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "NekiInt",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "NekiLong",
                table: "Schools");
        }
    }
}
