namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AprovedUsrrAdded3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Locations", "Line_Id", "dbo.Lines");
            DropIndex("dbo.Locations", new[] { "Line_Id" });
            DropColumn("dbo.Locations", "Line_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Locations", "Line_Id", c => c.Int());
            CreateIndex("dbo.Locations", "Line_Id");
            AddForeignKey("dbo.Locations", "Line_Id", "dbo.Lines", "Id");
        }
    }
}
