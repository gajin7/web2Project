namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInitialValues5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Locations", "Line_Name", "dbo.Lines");
            DropForeignKey("dbo.StationLines", "Line_Name", "dbo.Lines");
            DropIndex("dbo.Locations", new[] { "Line_Name" });
            DropIndex("dbo.StationLines", new[] { "Line_Name" });
            RenameColumn(table: "dbo.Locations", name: "Line_Name", newName: "Line_Id");
            RenameColumn(table: "dbo.StationLines", name: "Line_Name", newName: "Line_Id");
            DropPrimaryKey("dbo.Lines");
            DropPrimaryKey("dbo.StationLines");
            AddColumn("dbo.Schedules", "LineId", c => c.Int(nullable: false));
            AddColumn("dbo.Lines", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Lines", "Name", c => c.String());
            AlterColumn("dbo.Locations", "Line_Id", c => c.Int());
            AlterColumn("dbo.StationLines", "Line_Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Lines", "Id");
            AddPrimaryKey("dbo.StationLines", new[] { "Station_StationNum", "Line_Id" });
            CreateIndex("dbo.Schedules", "LineId");
            CreateIndex("dbo.Locations", "Line_Id");
            CreateIndex("dbo.StationLines", "Line_Id");
            AddForeignKey("dbo.Schedules", "LineId", "dbo.Lines", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Locations", "Line_Id", "dbo.Lines", "Id");
            AddForeignKey("dbo.StationLines", "Line_Id", "dbo.Lines", "Id", cascadeDelete: true);
            DropColumn("dbo.Schedules", "LineName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schedules", "LineName", c => c.String());
            DropForeignKey("dbo.StationLines", "Line_Id", "dbo.Lines");
            DropForeignKey("dbo.Locations", "Line_Id", "dbo.Lines");
            DropForeignKey("dbo.Schedules", "LineId", "dbo.Lines");
            DropIndex("dbo.StationLines", new[] { "Line_Id" });
            DropIndex("dbo.Locations", new[] { "Line_Id" });
            DropIndex("dbo.Schedules", new[] { "LineId" });
            DropPrimaryKey("dbo.StationLines");
            DropPrimaryKey("dbo.Lines");
            AlterColumn("dbo.StationLines", "Line_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Locations", "Line_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Lines", "Name", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Lines", "Id");
            DropColumn("dbo.Schedules", "LineId");
            AddPrimaryKey("dbo.StationLines", new[] { "Station_StationNum", "Line_Name" });
            AddPrimaryKey("dbo.Lines", "Name");
            RenameColumn(table: "dbo.StationLines", name: "Line_Id", newName: "Line_Name");
            RenameColumn(table: "dbo.Locations", name: "Line_Id", newName: "Line_Name");
            CreateIndex("dbo.StationLines", "Line_Name");
            CreateIndex("dbo.Locations", "Line_Name");
            AddForeignKey("dbo.StationLines", "Line_Name", "dbo.Lines", "Name", cascadeDelete: true);
            AddForeignKey("dbo.Locations", "Line_Name", "dbo.Lines", "Name");
        }
    }
}
