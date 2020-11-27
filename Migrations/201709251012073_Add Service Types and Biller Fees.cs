namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddServiceTypesandBillerFees : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppliedFees",
                c => new
                    {
                        AppliedFeeID = c.Int(nullable: false, identity: true),
                        TransfrerID = c.Int(nullable: false),
                        ServiceTypeID = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.AppliedFeeID);
            
            CreateTable(
                "dbo.BillerFees",
                c => new
                    {
                        BillerFeeID = c.Int(nullable: false, identity: true),
                        BillerID = c.Guid(nullable: false),
                        ServiceTypeID = c.Int(nullable: false),
                        Description = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ApplyAsPercentage = c.Boolean(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        UpdatedBy = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BillerFeeID);
            
            CreateTable(
                "dbo.BillerServices",
                c => new
                    {
                        BillerID = c.Guid(nullable: false),
                        ServiceTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BillerID, t.ServiceTypeID });
            
            CreateTable(
                "dbo.ServiceTypes",
                c => new
                    {
                        ServiceTypeID = c.Int(nullable: false, identity: true),
                        ServiceName = c.String(),
                        ServiceDescription = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ServiceTypeID);
            
            AddColumn("dbo.Transfers", "ServiceTypeID", c => c.Int(nullable: false));
            AddColumn("dbo.Transfers", "TransferTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transfers", "TransferTotal");
            DropColumn("dbo.Transfers", "ServiceTypeID");
            DropTable("dbo.ServiceTypes");
            DropTable("dbo.BillerServices");
            DropTable("dbo.BillerFees");
            DropTable("dbo.AppliedFees");
        }
    }
}
