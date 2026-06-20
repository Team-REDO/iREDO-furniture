using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace user.Migrations
{
    /// <inheritdoc />
    public partial class AddProcessedEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Persons_PersonId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonDetails_Persons_PersonId",
                table: "PersonDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonRemoved_Persons_PersonId",
                table: "PersonRemoved");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedListPosts_SavedLists_SavedListId",
                table: "SavedListPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedLists_Persons_PersonId",
                table: "SavedLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SavedLists",
                table: "SavedLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SavedListPosts",
                table: "SavedListPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonRemoved",
                table: "PersonRemoved");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonDetails",
                table: "PersonDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.RenameTable(
                name: "SavedLists",
                newName: "Saved_Lists");

            migrationBuilder.RenameTable(
                name: "SavedListPosts",
                newName: "Saved_List_Posts");

            migrationBuilder.RenameTable(
                name: "PersonRemoved",
                newName: "Person_Removed");

            migrationBuilder.RenameTable(
                name: "PersonDetails",
                newName: "Person_Details");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Address");

            migrationBuilder.RenameIndex(
                name: "IX_SavedLists_PersonId",
                table: "Saved_Lists",
                newName: "IX_Saved_Lists_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonRemoved_PersonId",
                table: "Person_Removed",
                newName: "IX_Person_Removed_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonDetails_PersonId",
                table: "Person_Details",
                newName: "IX_Person_Details_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_PersonId",
                table: "Address",
                newName: "IX_Address_PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Saved_Lists",
                table: "Saved_Lists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Saved_List_Posts",
                table: "Saved_List_Posts",
                columns: new[] { "SavedListId", "SalesPostGuid" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Person_Removed",
                table: "Person_Removed",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Person_Details",
                table: "Person_Details",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Processed_Events",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processed_Events", x => x.EventId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Persons_PersonId",
                table: "Address",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Details_Persons_PersonId",
                table: "Person_Details",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Removed_Persons_PersonId",
                table: "Person_Removed",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Saved_List_Posts_Saved_Lists_SavedListId",
                table: "Saved_List_Posts",
                column: "SavedListId",
                principalTable: "Saved_Lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Saved_Lists_Persons_PersonId",
                table: "Saved_Lists",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Persons_PersonId",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Person_Details_Persons_PersonId",
                table: "Person_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Person_Removed_Persons_PersonId",
                table: "Person_Removed");

            migrationBuilder.DropForeignKey(
                name: "FK_Saved_List_Posts_Saved_Lists_SavedListId",
                table: "Saved_List_Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Saved_Lists_Persons_PersonId",
                table: "Saved_Lists");

            migrationBuilder.DropTable(
                name: "Processed_Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Saved_Lists",
                table: "Saved_Lists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Saved_List_Posts",
                table: "Saved_List_Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Person_Removed",
                table: "Person_Removed");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Person_Details",
                table: "Person_Details");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.RenameTable(
                name: "Saved_Lists",
                newName: "SavedLists");

            migrationBuilder.RenameTable(
                name: "Saved_List_Posts",
                newName: "SavedListPosts");

            migrationBuilder.RenameTable(
                name: "Person_Removed",
                newName: "PersonRemoved");

            migrationBuilder.RenameTable(
                name: "Person_Details",
                newName: "PersonDetails");

            migrationBuilder.RenameTable(
                name: "Address",
                newName: "Addresses");

            migrationBuilder.RenameIndex(
                name: "IX_Saved_Lists_PersonId",
                table: "SavedLists",
                newName: "IX_SavedLists_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Person_Removed_PersonId",
                table: "PersonRemoved",
                newName: "IX_PersonRemoved_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Person_Details_PersonId",
                table: "PersonDetails",
                newName: "IX_PersonDetails_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Address_PersonId",
                table: "Addresses",
                newName: "IX_Addresses_PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SavedLists",
                table: "SavedLists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SavedListPosts",
                table: "SavedListPosts",
                columns: new[] { "SavedListId", "SalesPostGuid" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonRemoved",
                table: "PersonRemoved",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonDetails",
                table: "PersonDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Persons_PersonId",
                table: "Addresses",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonDetails_Persons_PersonId",
                table: "PersonDetails",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonRemoved_Persons_PersonId",
                table: "PersonRemoved",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedListPosts_SavedLists_SavedListId",
                table: "SavedListPosts",
                column: "SavedListId",
                principalTable: "SavedLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedLists_Persons_PersonId",
                table: "SavedLists",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
