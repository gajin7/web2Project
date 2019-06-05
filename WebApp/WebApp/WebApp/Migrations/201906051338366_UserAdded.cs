namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Address = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        _appUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t._appUser_Id)
                .Index(t => t._appUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "_appUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Users", new[] { "_appUser_Id" });
            DropTable("dbo.Users");
        }
    }
}
