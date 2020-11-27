namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modeilupdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transfers", "SourceTransactionID", c => c.String());
            AddColumn("dbo.Transfers", "DestinationTransactionID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transfers", "DestinationTransactionID");
            DropColumn("dbo.Transfers", "SourceTransactionID");
        }
    }
}
