using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SearchCount.Contexts.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SearchCountHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchTerm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchEngine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Indices = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfExcecution = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchCountHistory", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchCountHistory");
        }
    }
}
