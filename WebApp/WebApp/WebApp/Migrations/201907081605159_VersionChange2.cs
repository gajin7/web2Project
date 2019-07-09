namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VersionChange2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schedules", "Version", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schedules", "Version");
        }
    }
}
