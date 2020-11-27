namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNullableDatetimeToVerifiedField : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AssociatedAccounts", "Verified", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AssociatedAccounts", "Verified", c => c.DateTime(nullable: false));
        }
    }
}
