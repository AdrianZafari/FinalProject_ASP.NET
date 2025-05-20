using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class MembersMusthavereferencetoUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserEntityId",
                table: "Members",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_UserEntityId",
                table: "Members",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_AspNetUsers_UserEntityId",
                table: "Members",
                column: "UserEntityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_AspNetUsers_UserEntityId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_UserEntityId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "Members");
        }
    }
}
