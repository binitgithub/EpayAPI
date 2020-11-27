namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedJoinTable : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.AssociatedAccountCOBSecurityReviews");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AssociatedAccountCOBSecurityReviews",
                c => new
                    {
                        AssociatedAccountID = c.Int(nullable: false),
                        COBSecurityReviewID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AssociatedAccountID, t.COBSecurityReviewID });
            
        }
    }
}
