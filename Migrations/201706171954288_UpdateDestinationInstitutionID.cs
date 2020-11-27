namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDestinationInstitutionID : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transfers", "DestinationInstitutionID", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transfers", "DestinationInstitutionID", c => c.Guid(nullable: false));
        }
    }
}
