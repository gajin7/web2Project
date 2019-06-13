namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AprovedUsrrAdded5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Stations", "LocationId", "dbo.Locations");
            DropIndex("dbo.Stations", new[] { "LocationId" });
            DropColumn("dbo.Stations", "LocationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stations", "LocationId", c => c.Int(nullable: false));
            CreateIndex("dbo.Stations", "LocationId");
            AddForeignKey("dbo.Stations", "LocationId", "dbo.Locations", "Id", cascadeDelete: true);
        }
    }
}
