namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialValues2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Prices", "LineType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Prices", "LineType", c => c.Int(nullable: false));
        }
    }
}
