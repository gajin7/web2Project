namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketUpdate2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tickets", "CheckedTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tickets", "CheckedTime", c => c.DateTime(nullable: false));
        }
    }
}
