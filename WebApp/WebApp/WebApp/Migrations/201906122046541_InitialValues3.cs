namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialValues3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Depatures", "Schedule_Id", "dbo.Schedules");
            DropIndex("dbo.Depatures", new[] { "Schedule_Id" });
            RenameColumn(table: "dbo.Depatures", name: "Schedule_Id", newName: "ScheduleId");
            AlterColumn("dbo.Depatures", "ScheduleId", c => c.Int(nullable: false));
            CreateIndex("dbo.Depatures", "ScheduleId");
            AddForeignKey("dbo.Depatures", "ScheduleId", "dbo.Schedules", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Depatures", "ScheduleId", "dbo.Schedules");
            DropIndex("dbo.Depatures", new[] { "ScheduleId" });
            AlterColumn("dbo.Depatures", "ScheduleId", c => c.Int());
            RenameColumn(table: "dbo.Depatures", name: "ScheduleId", newName: "Schedule_Id");
            CreateIndex("dbo.Depatures", "Schedule_Id");
            AddForeignKey("dbo.Depatures", "Schedule_Id", "dbo.Schedules", "Id");
        }
    }
}
