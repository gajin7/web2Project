namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketUpdate5 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Tickets", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.Tickets", name: "IX_User_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Tickets", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Tickets", name: "UserId", newName: "User_Id");
        }
    }
}
