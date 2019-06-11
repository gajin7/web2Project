namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PicturesAdded1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pictures", "UserId", "dbo.Users");
            DropIndex("dbo.Pictures", new[] { "UserId" });
            AddColumn("dbo.Pictures", "AppUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Pictures", "AppUserId");
            AddForeignKey("dbo.Pictures", "AppUserId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Pictures", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pictures", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Pictures", "AppUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Pictures", new[] { "AppUserId" });
            DropColumn("dbo.Pictures", "AppUserId");
            CreateIndex("dbo.Pictures", "UserId");
            AddForeignKey("dbo.Pictures", "UserId", "dbo.Users", "id", cascadeDelete: true);
        }
    }
}
