namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDiscount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Discounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Discount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Lines",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        LineType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Lon = c.String(),
                        Lat = c.String(),
                        Line_Name = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lines", t => t.Line_Name)
                .Index(t => t.Line_Name);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Day = c.Int(nullable: false),
                        LineName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lines", t => t.LineName)
                .Index(t => t.LineName);
            
            CreateTable(
                "dbo.Stations",
                c => new
                    {
                        StationNum = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location_Id = c.Int(),
                    })
                .PrimaryKey(t => t.StationNum)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .Index(t => t.Location_Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Double(nullable: false),
                        Type = c.Int(nullable: false),
                        RemainingTime = c.Time(nullable: false, precision: 7),
                        Checked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StationLines",
                c => new
                    {
                        Station_StationNum = c.Int(nullable: false),
                        Line_Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Station_StationNum, t.Line_Name })
                .ForeignKey("dbo.Stations", t => t.Station_StationNum, cascadeDelete: true)
                .ForeignKey("dbo.Lines", t => t.Line_Name, cascadeDelete: true)
                .Index(t => t.Station_StationNum)
                .Index(t => t.Line_Name);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stations", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.StationLines", "Line_Name", "dbo.Lines");
            DropForeignKey("dbo.StationLines", "Station_StationNum", "dbo.Stations");
            DropForeignKey("dbo.Schedules", "LineName", "dbo.Lines");
            DropForeignKey("dbo.Locations", "Line_Name", "dbo.Lines");
            DropIndex("dbo.StationLines", new[] { "Line_Name" });
            DropIndex("dbo.StationLines", new[] { "Station_StationNum" });
            DropIndex("dbo.Stations", new[] { "Location_Id" });
            DropIndex("dbo.Schedules", new[] { "LineName" });
            DropIndex("dbo.Locations", new[] { "Line_Name" });
            DropTable("dbo.StationLines");
            DropTable("dbo.Tickets");
            DropTable("dbo.Stations");
            DropTable("dbo.Schedules");
            DropTable("dbo.Locations");
            DropTable("dbo.Lines");
            DropTable("dbo.Discounts");
        }
    }
}
