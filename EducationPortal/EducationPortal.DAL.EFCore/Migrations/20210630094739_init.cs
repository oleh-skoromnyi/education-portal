using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationPortal.DAL.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sch");

            migrationBuilder.CreateTable(
                name: "Courses",
                schema: "sch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                schema: "sch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                schema: "sch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "sch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Login = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.UniqueConstraint("UK_Login", x => x.Login);
                });

            migrationBuilder.CreateTable(
                name: "CourseMaterial",
                schema: "sch",
                columns: table => new
                {
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseMaterial", x => new { x.CourseId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_CourseMaterial_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "sch",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseMaterial_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalSchema: "sch",
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DigitalBookMaterial",
                schema: "sch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Authors = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pages = table.Column<int>(type: "int", nullable: false),
                    Format = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearOfPublication = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalBookMaterial", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_DigitalBookMaterial_Materials_Id",
                        column: x => x.Id,
                        principalSchema: "sch",
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InternetArticleMaterial",
                schema: "sch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DateOfPublication = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LinqToResource = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternetArticleMaterial", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_InternetArticleMaterial_Materials_Id",
                        column: x => x.Id,
                        principalSchema: "sch",
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestMaterial",
                schema: "sch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestMaterial", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_TestMaterial_Materials_Id",
                        column: x => x.Id,
                        principalSchema: "sch",
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VideoMaterial",
                schema: "sch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quality = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoMaterial", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_VideoMaterial_Materials_Id",
                        column: x => x.Id,
                        principalSchema: "sch",
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseGivenSkill",
                schema: "sch",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseGivenSkill", x => new { x.SkillId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_CourseGivenSkill_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "sch",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseGivenSkill_Skills_SkillId",
                        column: x => x.SkillId,
                        principalSchema: "sch",
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequirenmentSkill",
                schema: "sch",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequirenmentSkill", x => new { x.SkillId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_RequirenmentSkill_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "sch",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequirenmentSkill_Skills_SkillId",
                        column: x => x.SkillId,
                        principalSchema: "sch",
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LearnedMaterial",
                schema: "sch",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearnedMaterial", x => new { x.MaterialId, x.UserId });
                    table.ForeignKey(
                        name: "FK_LearnedMaterial_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalSchema: "sch",
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearnedMaterial_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "sch",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserCourse",
                schema: "sch",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Progress = table.Column<int>(type: "int", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCourse", x => new { x.CourseId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserCourse_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "sch",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCourse_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "sch",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSkill",
                schema: "sch",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkill", x => new { x.SkillId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserSkill_Skills_SkillId",
                        column: x => x.SkillId,
                        principalSchema: "sch",
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkill_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "sch",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestItems",
                schema: "sch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectAnswerIndex = table.Column<int>(type: "int", nullable: false),
                    TestMaterialId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestItems_TestMaterial_TestMaterialId",
                        column: x => x.TestMaterialId,
                        principalSchema: "sch",
                        principalTable: "TestMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                schema: "sch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_TestItems_TestItemId",
                        column: x => x.TestItemId,
                        principalSchema: "sch",
                        principalTable: "TestItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_TestItemId",
                schema: "sch",
                table: "Answers",
                column: "TestItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseGivenSkill_CourseId",
                schema: "sch",
                table: "CourseGivenSkill",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseMaterial_MaterialId",
                schema: "sch",
                table: "CourseMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_LearnedMaterial_UserId",
                schema: "sch",
                table: "LearnedMaterial",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RequirenmentSkill_CourseId",
                schema: "sch",
                table: "RequirenmentSkill",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TestItems_TestMaterialId",
                schema: "sch",
                table: "TestItems",
                column: "TestMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourse_UserId",
                schema: "sch",
                table: "UserCourse",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserId",
                schema: "sch",
                table: "UserSkill",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers",
                schema: "sch");

            migrationBuilder.DropTable(
                name: "CourseGivenSkill",
                schema: "sch");

            migrationBuilder.DropTable(
                name: "CourseMaterial",
                schema: "sch");

            migrationBuilder.DropTable(
                name: "DigitalBookMaterial",
                schema: "sch");

            migrationBuilder.DropTable(
                name: "InternetArticleMaterial",
                schema: "sch");

            migrationBuilder.DropTable(
                name: "LearnedMaterial",
                schema: "sch");

            migrationBuilder.DropTable(
                name: "RequirenmentSkill",
                schema: "sch");

            migrationBuilder.DropTable(
                name: "UserCourse",
                schema: "sch");

            migrationBuilder.DropTable(
                name: "UserSkill",
                schema: "sch");

            migrationBuilder.DropTable(
                name: "VideoMaterial",
                schema: "sch");

            migrationBuilder.DropTable(
                name: "TestItems",
                schema: "sch");

            migrationBuilder.DropTable(
                name: "Courses",
                schema: "sch");

            migrationBuilder.DropTable(
                name: "Skills",
                schema: "sch");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "sch");

            migrationBuilder.DropTable(
                name: "TestMaterial",
                schema: "sch");

            migrationBuilder.DropTable(
                name: "Materials",
                schema: "sch");
        }
    }
}
