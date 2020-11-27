namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAssociatedAccountDescription2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssociatedAccounts", "Description2", c => c.String());
            AddColumn("dbo.AssociatedAccounts", "Description3", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssociatedAccounts", "Description3");
            DropColumn("dbo.AssociatedAccounts", "Description2");
        }
    }
}
