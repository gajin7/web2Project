namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserUpdate3 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Users", name: "AppUser_Id", newName: "AppUserId");
            RenameIndex(table: "dbo.Users", name: "IX_AppUser_Id", newName: "IX_AppUserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Users", name: "IX_AppUserId", newName: "IX_AppUser_Id");
            RenameColumn(table: "dbo.Users", name: "AppUserId", newName: "AppUser_Id");
        }
    }
}
