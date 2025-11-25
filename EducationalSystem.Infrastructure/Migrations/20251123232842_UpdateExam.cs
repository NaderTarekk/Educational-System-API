using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationalSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TbExams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    TotalMarks = table.Column<decimal>(type: "decimal(10,2)", precision: 5, scale: 2, nullable: false),
                    PassingMarks = table.Column<decimal>(type: "decimal(10,2)", precision: 5, scale: 2, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbExams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbExams_TbGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "TbGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Marks = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbQuestions_TbExams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "TbExams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbStudentExams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Score = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbStudentExams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbStudentExams_TbExams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "TbExams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbQuestionOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbQuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbQuestionOptions_TbQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "TbQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbStudentAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentExamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SelectedOptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: true),
                    MarksObtained = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbStudentAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbStudentAnswers_TbQuestionOptions_SelectedOptionId",
                        column: x => x.SelectedOptionId,
                        principalTable: "TbQuestionOptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbStudentAnswers_TbQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "TbQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbStudentAnswers_TbStudentExams_StudentExamId",
                        column: x => x.StudentExamId,
                        principalTable: "TbStudentExams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbExams_GroupId",
                table: "TbExams",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuestionOptions_QuestionId",
                table: "TbQuestionOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuestions_ExamId",
                table: "TbQuestions",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_TbStudentAnswers_QuestionId",
                table: "TbStudentAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TbStudentAnswers_SelectedOptionId",
                table: "TbStudentAnswers",
                column: "SelectedOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TbStudentAnswers_StudentExamId",
                table: "TbStudentAnswers",
                column: "StudentExamId");

            migrationBuilder.CreateIndex(
                name: "IX_TbStudentExams_ExamId",
                table: "TbStudentExams",
                column: "ExamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbStudentAnswers");

            migrationBuilder.DropTable(
                name: "TbQuestionOptions");

            migrationBuilder.DropTable(
                name: "TbStudentExams");

            migrationBuilder.DropTable(
                name: "TbQuestions");

            migrationBuilder.DropTable(
                name: "TbExams");
        }
    }
}
