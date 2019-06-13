namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AprovedUsrrAdded8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stations", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stations", "Address");
        }
    }
}
