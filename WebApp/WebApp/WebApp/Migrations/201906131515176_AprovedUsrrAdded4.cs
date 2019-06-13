namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AprovedUsrrAdded4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Stations", "Location_Id", "dbo.Locations");
            DropIndex("dbo.Stations", new[] { "Location_Id" });
            RenameColumn(table: "dbo.Stations", name: "Location_Id", newName: "LocationId");
            AlterColumn("dbo.Stations", "LocationId", c => c.Int(nullable: false));
            CreateIndex("dbo.Stations", "LocationId");
            AddForeignKey("dbo.Stations", "LocationId", "dbo.Locations", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stations", "LocationId", "dbo.Locations");
            DropIndex("dbo.Stations", new[] { "LocationId" });
            AlterColumn("dbo.Stations", "LocationId", c => c.Int());
            RenameColumn(table: "dbo.Stations", name: "LocationId", newName: "Location_Id");
            CreateIndex("dbo.Stations", "Location_Id");
            AddForeignKey("dbo.Stations", "Location_Id", "dbo.Locations", "Id");
        }
    }
}
