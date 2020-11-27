namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_initial_create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FinancialInstitutions",
                c => new
                    {
                        FinancialInstitutionID = c.Guid(nullable: false),
                        Name = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FinancialInstitutionID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FinancialInstitutions");
        }
    }
}
