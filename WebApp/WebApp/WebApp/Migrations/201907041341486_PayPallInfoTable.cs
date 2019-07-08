namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PayPallInfoTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PayPalInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionId = c.String(),
                        TicketId = c.Int(nullable: false),
                        PayerEmail = c.String(),
                        PayerId = c.String(),
                        CreateTime = c.String(),
                        UpdateTime = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PayPalInfoes");
        }
    }
}
