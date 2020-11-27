namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPIN : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PINs",
                c => new
                    {
                        PINID = c.Guid(nullable: false),
                        FinancialInstitutionID = c.Guid(nullable: false),
                        UserID = c.String(),
                        Pin = c.String(),
                        Updated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PINID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PINs");
        }
    }
}
