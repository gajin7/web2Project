namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScheduleUPDATE : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schedules", "Depatures", c => c.DateTime(nullable: false));
            AddColumn("dbo.Schedules", "Duration", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schedules", "Duration");
            DropColumn("dbo.Schedules", "Depatures");
        }
    }
}
