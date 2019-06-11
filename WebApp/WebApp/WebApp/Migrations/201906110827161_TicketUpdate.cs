namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "CheckedTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tickets", "UserId", c => c.Int(nullable: true));
            AddColumn("dbo.Tickets", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Tickets", "User_Id");
            AddForeignKey("dbo.Tickets", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Tickets", new[] { "User_Id" });
            DropColumn("dbo.Tickets", "User_Id");
            DropColumn("dbo.Tickets", "UserId");
            DropColumn("dbo.Tickets", "CheckedTime");
        }
    }
}
