namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAssociatedAccountStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssociatedAccounts", "Status", c => c.String());
            AddColumn("dbo.AssociatedAccounts", "Updated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssociatedAccounts", "Updated");
            DropColumn("dbo.AssociatedAccounts", "Status");
        }
    }
}
