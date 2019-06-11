namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserUpdate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "AppUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Users", "AppUser_Id");
            AddForeignKey("dbo.Users", "AppUser_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Users", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Email", c => c.String());
            DropForeignKey("dbo.Users", "AppUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Users", new[] { "AppUser_Id" });
            DropColumn("dbo.Users", "AppUser_Id");
        }
    }
}
