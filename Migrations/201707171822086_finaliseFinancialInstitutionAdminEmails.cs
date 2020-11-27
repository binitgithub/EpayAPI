namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class finaliseFinancialInstitutionAdminEmails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FinancialInstitutionAdminEmails",
                c => new
                    {
                        FinancialInstitutionAdminEmailID = c.Int(nullable: false, identity: true),
                        FinancialInstitutionID = c.Guid(nullable: false),
                        EmailAddress = c.String(),
                        ContactName = c.String(),
                    })
                .PrimaryKey(t => t.FinancialInstitutionAdminEmailID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FinancialInstitutionAdminEmails");
        }
    }
}
