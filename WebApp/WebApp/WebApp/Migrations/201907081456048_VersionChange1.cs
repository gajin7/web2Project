namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VersionChange1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stations", "Version", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stations", "Version");
        }
    }
}
