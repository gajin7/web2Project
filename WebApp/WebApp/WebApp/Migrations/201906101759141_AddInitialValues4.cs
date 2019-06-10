namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInitialValues4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Schedules", "LineName", "dbo.Lines");
            DropIndex("dbo.Schedules", new[] { "LineName" });
            AlterColumn("dbo.Schedules", "LineName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Schedules", "LineName", c => c.String(maxLength: 128));
            CreateIndex("dbo.Schedules", "LineName");
            AddForeignKey("dbo.Schedules", "LineName", "dbo.Lines", "Name");
        }
    }
}
