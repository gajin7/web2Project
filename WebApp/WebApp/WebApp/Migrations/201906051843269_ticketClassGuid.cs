namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ticketClassGuid : DbMigration
    {
        public override void Up()
        {
           
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Tickets");
            AlterColumn("dbo.Tickets", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Tickets", "Id");
        }
    }
}
