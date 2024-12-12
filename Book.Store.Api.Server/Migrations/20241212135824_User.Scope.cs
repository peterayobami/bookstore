using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book.Store.Api.Server.Migrations
{
    /// <inheritdoc />
    public partial class UserScope : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Scope",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Scope",
                table: "AspNetUsers");
        }
    }
}
