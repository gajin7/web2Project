namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScheduleUPDATE1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Schedules", "Depatures");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schedules", "Depatures", c => c.DateTime(nullable: false));
        }
    }
}
