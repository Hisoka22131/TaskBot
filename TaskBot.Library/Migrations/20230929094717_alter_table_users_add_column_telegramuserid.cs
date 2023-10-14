using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskBot.Library.Migrations
{
    /// <inheritdoc />
    public partial class alter_table_users_add_column_telegramuserid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TelegramUserId",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelegramUserId",
                table: "Users");
        }
    }
}
