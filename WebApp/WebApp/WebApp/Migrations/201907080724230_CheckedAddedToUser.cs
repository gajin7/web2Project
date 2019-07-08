namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CheckedAddedToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Checked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Checked");
        }
    }
}
