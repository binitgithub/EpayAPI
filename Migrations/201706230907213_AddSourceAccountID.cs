namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSourceAccountID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transfers", "SourceAccountID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transfers", "SourceAccountID");
        }
    }
}
