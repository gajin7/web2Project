namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketUpdate1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tickets", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tickets", "UserId", c => c.Int(nullable: false));
        }
    }
}
