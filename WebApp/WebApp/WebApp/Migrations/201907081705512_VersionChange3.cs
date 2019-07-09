namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VersionChange3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Discounts", "Version", c => c.Double(nullable: false));
            AddColumn("dbo.Prices", "Version", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prices", "Version");
            DropColumn("dbo.Discounts", "Version");
        }
    }
}
