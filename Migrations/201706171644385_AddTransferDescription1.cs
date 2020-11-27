namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransferDescription1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transfers",
                c => new
                    {
                        TransferID = c.Int(nullable: false, identity: true),
                        SourceInstitutionID = c.Guid(nullable: false),
                        SourceAccountNumber = c.String(),
                        DestinationInstitutionID = c.Guid(nullable: false),
                        DestinationAccountNumber = c.String(),
                        TransferAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransferDescription = c.String(),
                        Status = c.String(),
                        StatusDescription = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TransferID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Transfers");
        }
    }
}
