using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreWebApiDemo.DataAccess.migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "userModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNo = table.Column<long>(type: "bigint", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "feedbackModels",
                columns: table => new
                {
                    LawyerId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rate = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedbackModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_feedbackModels_userModels_UserId",
                        column: x => x.UserId,
                        principalTable: "userModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "questionaryModels",
                columns: table => new
                {
                    LawyerId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Media = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isPicked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questionaryModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_questionaryModels_userModels_UserId",
                        column: x => x.UserId,
                        principalTable: "userModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "answerModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    LawyerId = table.Column<int>(type: "int", nullable: false),
                    QuestionaryId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answerModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_answerModels_questionaryModels_QuestionaryId",
                        column: x => x.QuestionaryId,
                        principalTable: "questionaryModels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_answerModels_userModels_UserId",
                        column: x => x.UserId,
                        principalTable: "userModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "conversationModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    AnswerId = table.Column<int>(type: "int", nullable: false),
                    QuestionaryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conversationModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_conversationModels_answerModels_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "answerModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_conversationModels_questionaryModels_QuestionaryId",
                        column: x => x.QuestionaryId,
                        principalTable: "questionaryModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_answerModels_QuestionaryId",
                table: "answerModels",
                column: "QuestionaryId");

            migrationBuilder.CreateIndex(
                name: "IX_answerModels_UserId",
                table: "answerModels",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_conversationModels_AnswerId",
                table: "conversationModels",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_conversationModels_QuestionaryId",
                table: "conversationModels",
                column: "QuestionaryId");

            migrationBuilder.CreateIndex(
                name: "IX_feedbackModels_UserId",
                table: "feedbackModels",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_questionaryModels_UserId",
                table: "questionaryModels",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "conversationModels");

            migrationBuilder.DropTable(
                name: "feedbackModels");

            migrationBuilder.DropTable(
                name: "answerModels");

            migrationBuilder.DropTable(
                name: "questionaryModels");

            migrationBuilder.DropTable(
                name: "userModels");
        }
    }
}
