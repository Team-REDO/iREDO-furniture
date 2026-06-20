using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace user.Migrations
{
    /// <inheritdoc />
    public partial class RefactorUserDatabaseStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SavedListPosts",
                table: "SavedListPosts");

            migrationBuilder.DropIndex(
                name: "IX_SavedListPosts_SavedListId",
                table: "SavedListPosts");

            migrationBuilder.DropIndex(
                name: "IX_PersonDetails_Email",
                table: "PersonDetails");

            migrationBuilder.DropIndex(
                name: "IX_PersonDetails_PersonId",
                table: "PersonDetails");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_PersonId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SavedListPosts");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "SavedListPosts");

            migrationBuilder.RenameColumn(
                name: "ModifiedDate",
                table: "SavedLists",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "SalesPostId",
                table: "SavedListPosts",
                newName: "SalesPostGuid");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "Persons",
                newName: "PersonGuid");

            migrationBuilder.RenameColumn(
                name: "RemovedDate",
                table: "PersonRemoved",
                newName: "RemovedAt");

            migrationBuilder.RenameColumn(
                name: "ModifiedDate",
                table: "PersonDetails",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "ModifiedDate",
                table: "Addresses",
                newName: "ModifiedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Persons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "PersonDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SavedListPosts",
                table: "SavedListPosts",
                columns: new[] { "SavedListId", "SalesPostGuid" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "User");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Admin");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_PersonGuid",
                table: "Persons",
                column: "PersonGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonDetails_PersonId",
                table: "PersonDetails",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PersonId",
                table: "Addresses",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SavedListPosts",
                table: "SavedListPosts");

            migrationBuilder.DropIndex(
                name: "IX_Persons_PersonGuid",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_PersonDetails_PersonId",
                table: "PersonDetails");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_PersonId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Persons");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "SavedLists",
                newName: "ModifiedDate");

            migrationBuilder.RenameColumn(
                name: "SalesPostGuid",
                table: "SavedListPosts",
                newName: "SalesPostId");

            migrationBuilder.RenameColumn(
                name: "PersonGuid",
                table: "Persons",
                newName: "Guid");

            migrationBuilder.RenameColumn(
                name: "RemovedAt",
                table: "PersonRemoved",
                newName: "RemovedDate");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "PersonDetails",
                newName: "ModifiedDate");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "Addresses",
                newName: "ModifiedDate");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SavedListPosts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "SavedListPosts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "PersonDetails",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SavedListPosts",
                table: "SavedListPosts",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "user");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "admin");

            migrationBuilder.CreateIndex(
                name: "IX_SavedListPosts_SavedListId",
                table: "SavedListPosts",
                column: "SavedListId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonDetails_Email",
                table: "PersonDetails",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonDetails_PersonId",
                table: "PersonDetails",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PersonId",
                table: "Addresses",
                column: "PersonId",
                unique: true);
        }
    }
}
