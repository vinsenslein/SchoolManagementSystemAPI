using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CSTUDENT_NIS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CSTUDENT_NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CSTUDENT_EMAIL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CSTUDENT_PHONE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CSTUDENT_GENDER = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LSTUDENT_ISACTIVE = table.Column<bool>(type: "bit", nullable: false),
                    DCREATE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DUPDATE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
