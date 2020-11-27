namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSettlementRequiredField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transfers", "SettlementRequired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transfers", "SettlementRequired");
        }
    }
}
