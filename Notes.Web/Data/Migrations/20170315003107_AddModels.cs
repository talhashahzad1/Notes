using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Notes.Web.Data.Migrations
{
    public partial class AddModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateTable(
                name: "Notebooks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notebooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notebooks_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notebooks_Notebooks_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Notebooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Body = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    NotebookId = table.Column<int>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Notebooks_NotebookId",
                        column: x => x.NotebookId,
                        principalTable: "Notebooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    ItemType = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagItems_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    NoteId = table.Column<int>(nullable: false),
                    Path = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_NoteId",
                table: "Assets",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_NotebookId",
                table: "Notes",
                column: "NotebookId");

            migrationBuilder.CreateIndex(
                name: "IX_Notebooks_ApplicationUserId",
                table: "Notebooks",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notebooks_ParentId",
                table: "Notebooks",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_TagItems_TagId",
                table: "TagItems",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_TagItems_ItemId_ItemType_TagId",
                table: "TagItems",
                columns: new[] { "ItemId", "ItemType", "TagId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "TagItems");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Notebooks");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");
        }
    }
}
