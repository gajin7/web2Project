namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prices_lineType_added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prices", "LineType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prices", "LineType");
        }
    }
}
