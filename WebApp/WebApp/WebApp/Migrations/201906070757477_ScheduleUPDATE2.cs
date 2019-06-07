namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScheduleUPDATE2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Schedules", name: "LineName", newName: "Line_Name");
            RenameIndex(table: "dbo.Schedules", name: "IX_LineName", newName: "IX_Line_Name");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Schedules", name: "IX_Line_Name", newName: "IX_LineName");
            RenameColumn(table: "dbo.Schedules", name: "Line_Name", newName: "LineName");
        }
    }
}
