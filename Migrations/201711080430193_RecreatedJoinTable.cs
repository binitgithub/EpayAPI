namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecreatedJoinTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.COBSecurityReviewAssociatedAccounts",
                c => new
                    {
                        COBSecurityReview_COBSecurityReviewID = c.Int(nullable: false),
                        AssociatedAccount_AssociatedAccountID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.COBSecurityReview_COBSecurityReviewID, t.AssociatedAccount_AssociatedAccountID })
                .ForeignKey("dbo.COBSecurityReviews", t => t.COBSecurityReview_COBSecurityReviewID, cascadeDelete: true)
                .ForeignKey("dbo.AssociatedAccounts", t => t.AssociatedAccount_AssociatedAccountID, cascadeDelete: true)
                .Index(t => t.COBSecurityReview_COBSecurityReviewID)
                .Index(t => t.AssociatedAccount_AssociatedAccountID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.COBSecurityReviewAssociatedAccounts", "AssociatedAccount_AssociatedAccountID", "dbo.AssociatedAccounts");
            DropForeignKey("dbo.COBSecurityReviewAssociatedAccounts", "COBSecurityReview_COBSecurityReviewID", "dbo.COBSecurityReviews");
            DropIndex("dbo.COBSecurityReviewAssociatedAccounts", new[] { "AssociatedAccount_AssociatedAccountID" });
            DropIndex("dbo.COBSecurityReviewAssociatedAccounts", new[] { "COBSecurityReview_COBSecurityReviewID" });
            DropTable("dbo.COBSecurityReviewAssociatedAccounts");
        }
    }
}
