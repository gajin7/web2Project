namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateSchedule4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Depatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DepatureTime = c.String(),
                        Schedule_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schedules", t => t.Schedule_Id)
                .Index(t => t.Schedule_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Depatures", "Schedule_Id", "dbo.Schedules");
            DropIndex("dbo.Depatures", new[] { "Schedule_Id" });
            DropTable("dbo.Depatures");
        }
    }
}
