namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "_appUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Users", new[] { "_appUser_Id" });
            AddColumn("dbo.Users", "Email", c => c.String());
            AddColumn("dbo.Users", "UserType", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "_appUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "_appUser_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.Users", "UserType");
            DropColumn("dbo.Users", "Email");
            CreateIndex("dbo.Users", "_appUser_Id");
            AddForeignKey("dbo.Users", "_appUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
