namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketUpdate4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tickets", "CheckedTime", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tickets", "CheckedTime", c => c.DateTime(nullable: false));
        }
    }
}
