namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeSchedule : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Schedules", "LineId", "dbo.Lines");
            DropForeignKey("dbo.Locations", "Line_Id", "dbo.Lines");
            DropForeignKey("dbo.StationLines", "Line_Id", "dbo.Lines");
            DropIndex("dbo.Schedules", new[] { "LineId" });
            DropIndex("dbo.Locations", new[] { "Line_Id" });
            DropIndex("dbo.StationLines", new[] { "Line_Id" });
            RenameColumn(table: "dbo.Schedules", name: "LineId", newName: "LineName");
            RenameColumn(table: "dbo.Locations", name: "Line_Id", newName: "Line_Name");
            RenameColumn(table: "dbo.StationLines", name: "Line_Id", newName: "Line_Name");
            DropPrimaryKey("dbo.Lines");
            DropPrimaryKey("dbo.StationLines");
            AlterColumn("dbo.Schedules", "LineName", c => c.String(maxLength: 128));
            AlterColumn("dbo.Lines", "Name", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Locations", "Line_Name", c => c.String(maxLength: 128));
            AlterColumn("dbo.StationLines", "Line_Name", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Lines", "Name");
            AddPrimaryKey("dbo.StationLines", new[] { "Station_StationNum", "Line_Name" });
            CreateIndex("dbo.Schedules", "LineName");
            CreateIndex("dbo.Locations", "Line_Name");
            CreateIndex("dbo.StationLines", "Line_Name");
            AddForeignKey("dbo.Schedules", "LineName", "dbo.Lines", "Name");
            AddForeignKey("dbo.Locations", "Line_Name", "dbo.Lines", "Name");
            AddForeignKey("dbo.StationLines", "Line_Name", "dbo.Lines", "Name", cascadeDelete: true);
            DropColumn("dbo.Lines", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Lines", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.StationLines", "Line_Name", "dbo.Lines");
            DropForeignKey("dbo.Locations", "Line_Name", "dbo.Lines");
            DropForeignKey("dbo.Schedules", "LineName", "dbo.Lines");
            DropIndex("dbo.StationLines", new[] { "Line_Name" });
            DropIndex("dbo.Locations", new[] { "Line_Name" });
            DropIndex("dbo.Schedules", new[] { "LineName" });
            DropPrimaryKey("dbo.StationLines");
            DropPrimaryKey("dbo.Lines");
            AlterColumn("dbo.StationLines", "Line_Name", c => c.Int(nullable: false));
            AlterColumn("dbo.Locations", "Line_Name", c => c.Int());
            AlterColumn("dbo.Lines", "Name", c => c.String());
            AlterColumn("dbo.Schedules", "LineName", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.StationLines", new[] { "Station_StationNum", "Line_Id" });
            AddPrimaryKey("dbo.Lines", "Id");
            RenameColumn(table: "dbo.StationLines", name: "Line_Name", newName: "Line_Id");
            RenameColumn(table: "dbo.Locations", name: "Line_Name", newName: "Line_Id");
            RenameColumn(table: "dbo.Schedules", name: "LineName", newName: "LineId");
            CreateIndex("dbo.StationLines", "Line_Id");
            CreateIndex("dbo.Locations", "Line_Id");
            CreateIndex("dbo.Schedules", "LineId");
            AddForeignKey("dbo.StationLines", "Line_Id", "dbo.Lines", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Locations", "Line_Id", "dbo.Lines", "Id");
            AddForeignKey("dbo.Schedules", "LineId", "dbo.Lines", "Id", cascadeDelete: true);
        }
    }
}
