namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AprovedUsrrAdded1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "postedImage", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "postedImage");
        }
    }
}
