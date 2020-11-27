namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMinorAccountNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transfers", "MinorAccountNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transfers", "MinorAccountNumber");
        }
    }
}
