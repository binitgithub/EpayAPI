namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConvertBillerIdToGuid : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transfers", "BillerID", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transfers", "BillerID", c => c.String());
        }
    }
}
