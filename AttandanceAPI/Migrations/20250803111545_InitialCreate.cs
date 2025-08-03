using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CSTUDENT_NIS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DATTENDANCE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CATTENDANCE_STATUS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DCREATE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DUPDATE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");
        }
    }
}
