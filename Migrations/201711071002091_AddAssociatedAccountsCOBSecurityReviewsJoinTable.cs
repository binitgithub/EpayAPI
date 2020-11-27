namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAssociatedAccountsCOBSecurityReviewsJoinTable : DbMigration
    {
        public override void Up()
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
        
        public override void Down()
        {
            DropTable("dbo.AssociatedAccountCOBSecurityReviews");
        }
    }
}
