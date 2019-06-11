namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PicturesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageSource = c.Binary(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            DropColumn("dbo.Users", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Image", c => c.Binary());
            DropForeignKey("dbo.Pictures", "UserId", "dbo.Users");
            DropIndex("dbo.Pictures", new[] { "UserId" });
            DropTable("dbo.Pictures");
        }
    }
}
