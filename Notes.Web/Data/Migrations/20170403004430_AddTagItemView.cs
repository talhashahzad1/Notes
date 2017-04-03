using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Notes.Web.Data.Migrations
{
    public partial class AddTagItemView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ISNULL used to combine names from notebooks and notes.
            // it replaces null with the second value if the first is null.
            migrationBuilder.Sql(@"
                CREATE VIEW TagItemViews AS
                SELECT ti.Id, ti.ItemType, ti.ItemId, ti.TagId, t.Name AS TagName,
                    ISNULL(n.Name, nb.Name) AS ItemName FROM TagItems ti
                JOIN Tags t
                ON (ti.TagId = t.Id)
                LEFT OUTER JOIN Notes n
                ON (ti.ItemId = n.Id
                AND ti.ItemType = 1
                )
                LEFT OUTER JOIN Notebooks nb
                ON (ti.ItemId = nb.Id
                AND ti.ItemType = 0
                );
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW TagItemViews");
        }
    }
}
