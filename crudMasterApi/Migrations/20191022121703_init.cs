using Microsoft.EntityFrameworkCore.Migrations;

namespace crudMasterApi.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Mail = table.Column<string>(maxLength: 100, nullable: true),
                    CityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schools_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Principal = table.Column<string>(maxLength: 100, nullable: true),
                    SchoolId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modules_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModuleSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSubject = table.Column<int>(maxLength: 200, nullable: false),
                    SubjectId = table.Column<int>(nullable: true),
                    IdModule = table.Column<int>(nullable: false),
                    ModuleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleSubjects_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name", "PostalCode" },
                values: new object[,]
                {
                    { 1, "Kovin", "26220" },
                    { 2, "Beograd", "11000" },
                    { 3, "Pančevo", "26000" },
                    { 4, "Novi Sad", "21000" },
                    { 5, "Beograd", "11000" }
                });

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "CityId", "Mail", "Name" },
                values: new object[,]
                {
                    { 1, 1, "gimeko@yahoo.com", "Gimnazija i ekonomska škola Branko Radičević" },
                    { 2, 1, "gimeko@yahoo.com", "Gimnazija i ekonomska škola Branko Radičević" },
                    { 5, 1, null, "Srednja stručna škola Mihajlo Pupin" },
                    { 3, 2, "gimeko@yahoo.com", "Gimnazija i ekonomska škola Branko Radičević" },
                    { 4, 3, "gimeko@yahoo.com", "Gimnazija i ekonomska škola Branko Radičević" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Modules_SchoolId",
                table: "Modules",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleSubjects_ModuleId",
                table: "ModuleSubjects",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleSubjects_SubjectId",
                table: "ModuleSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_CityId",
                table: "Schools",
                column: "CityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModuleSubjects");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Schools");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
