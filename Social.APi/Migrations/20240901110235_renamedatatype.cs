using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Social.APi.Migrations
{
    /// <inheritdoc />
    public partial class renamedatatype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop existing foreign key constraints
            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_Users_ReceiverId",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_Users_SenderId",
                table: "FriendRequests");

            // Drop existing indexes
            migrationBuilder.DropIndex(
                name: "IX_FriendRequests_ReceiverId",
                table: "FriendRequests");

            migrationBuilder.DropIndex(
                name: "IX_FriendRequests_SenderId_ReceiverId",
                table: "FriendRequests");

            // Alter column types from int to string (nvarchar)
            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "FriendRequests",
                type: "nvarchar(256)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverId",
                table: "FriendRequests",
                type: "nvarchar(256)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            // Re-create indexes
            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_ReceiverId",
                table: "FriendRequests",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_SenderId_ReceiverId",
                table: "FriendRequests",
                columns: new[] { "SenderId", "ReceiverId" },
                unique: true);

            // Re-create foreign key constraints with updated column types
            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_Users_ReceiverId",
                table: "FriendRequests",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_Users_SenderId",
                table: "FriendRequests",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert changes by dropping the modified foreign key constraints and indexes
            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_Users_ReceiverId",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_Users_SenderId",
                table: "FriendRequests");

            migrationBuilder.DropIndex(
                name: "IX_FriendRequests_ReceiverId",
                table: "FriendRequests");

            migrationBuilder.DropIndex(
                name: "IX_FriendRequests_SenderId_ReceiverId",
                table: "FriendRequests");

            // Alter column types back to int
            migrationBuilder.AlterColumn<int>(
                name: "SenderId",
                table: "FriendRequests",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)");

            migrationBuilder.AlterColumn<int>(
                name: "ReceiverId",
                table: "FriendRequests",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)");

            // Re-create indexes with original types
            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_ReceiverId",
                table: "FriendRequests",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_SenderId_ReceiverId",
                table: "FriendRequests",
                columns: new[] { "SenderId", "ReceiverId" },
                unique: true);

            // Re-create original foreign key constraints
            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_Users_ReceiverId",
                table: "FriendRequests",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_Users_SenderId",
                table: "FriendRequests",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
