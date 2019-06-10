namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Schedules", "Line_Id", "dbo.Lines");
            DropIndex("dbo.Schedules", new[] { "Line_Id" });
            RenameColumn(table: "dbo.Schedules", name: "Line_Id", newName: "LineId");
            AlterColumn("dbo.Schedules", "LineId", c => c.Int(nullable: false));
            CreateIndex("dbo.Schedules", "LineId");
            AddForeignKey("dbo.Schedules", "LineId", "dbo.Lines", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schedules", "LineId", "dbo.Lines");
            DropIndex("dbo.Schedules", new[] { "LineId" });
            AlterColumn("dbo.Schedules", "LineId", c => c.Int());
            RenameColumn(table: "dbo.Schedules", name: "LineId", newName: "Line_Id");
            CreateIndex("dbo.Schedules", "Line_Id");
            AddForeignKey("dbo.Schedules", "Line_Id", "dbo.Lines", "Id");
        }
    }
}
