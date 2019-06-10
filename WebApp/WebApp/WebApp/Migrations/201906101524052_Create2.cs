namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Schedules", "LineId", "dbo.Lines");
            DropIndex("dbo.Schedules", new[] { "LineId" });
            RenameColumn(table: "dbo.Schedules", name: "LineId", newName: "Line_Id");
            AlterColumn("dbo.Schedules", "Line_Id", c => c.Int());
            CreateIndex("dbo.Schedules", "Line_Id");
            AddForeignKey("dbo.Schedules", "Line_Id", "dbo.Lines", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schedules", "Line_Id", "dbo.Lines");
            DropIndex("dbo.Schedules", new[] { "Line_Id" });
            AlterColumn("dbo.Schedules", "Line_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Schedules", name: "Line_Id", newName: "LineId");
            CreateIndex("dbo.Schedules", "LineId");
            AddForeignKey("dbo.Schedules", "LineId", "dbo.Lines", "Id", cascadeDelete: true);
        }
    }
}
