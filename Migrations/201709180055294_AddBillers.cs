namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBillers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Billers",
                c => new
                    {
                        BillerID = c.Guid(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                        MinorAccountDescription = c.String(),
                        AdminRole = c.String(),
                        Status = c.String(),
                        StatusDescription = c.String(),
                        Updated = c.DateTime(nullable: false),
                        UpdatedBy = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BillerID);
            
            AddColumn("dbo.Transfers", "BillerID", c => c.String());
            AddColumn("dbo.Transfers", "BillerAccountNumber", c => c.String());
            AddColumn("dbo.Transfers", "BillerTransactionID", c => c.String());
            DropColumn("dbo.Transfers", "DestinationInstitutionID");
            DropColumn("dbo.Transfers", "DestinationAccountNumber");
            DropColumn("dbo.Transfers", "DestinationTransactionID");
            DropColumn("dbo.Transfers", "SettlementRequired");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transfers", "SettlementRequired", c => c.Boolean(nullable: false));
            AddColumn("dbo.Transfers", "DestinationTransactionID", c => c.String());
            AddColumn("dbo.Transfers", "DestinationAccountNumber", c => c.String());
            AddColumn("dbo.Transfers", "DestinationInstitutionID", c => c.String());
            DropColumn("dbo.Transfers", "BillerTransactionID");
            DropColumn("dbo.Transfers", "BillerAccountNumber");
            DropColumn("dbo.Transfers", "BillerID");
            DropTable("dbo.Billers");
        }
    }
}
