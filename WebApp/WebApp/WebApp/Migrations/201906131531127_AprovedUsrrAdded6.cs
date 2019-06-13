namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AprovedUsrrAdded6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stations", "LocationId", c => c.Int(nullable: false));
            AlterColumn("dbo.Locations", "Lon", c => c.Double(nullable: false));
            AlterColumn("dbo.Locations", "Lat", c => c.Double(nullable: false));
            CreateIndex("dbo.Stations", "LocationId");
            AddForeignKey("dbo.Stations", "LocationId", "dbo.Locations", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stations", "LocationId", "dbo.Locations");
            DropIndex("dbo.Stations", new[] { "LocationId" });
            AlterColumn("dbo.Locations", "Lat", c => c.String());
            AlterColumn("dbo.Locations", "Lon", c => c.String());
            DropColumn("dbo.Stations", "LocationId");
        }
    }
}
