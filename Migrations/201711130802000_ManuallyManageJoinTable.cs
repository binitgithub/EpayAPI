namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManuallyManageJoinTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.COBSecurityReviewAssociatedAccounts", "COBSecurityReview_COBSecurityReviewID", "dbo.COBSecurityReviews");
            DropForeignKey("dbo.COBSecurityReviewAssociatedAccounts", "AssociatedAccount_AssociatedAccountID", "dbo.AssociatedAccounts");
            DropIndex("dbo.COBSecurityReviewAssociatedAccounts", new[] { "COBSecurityReview_COBSecurityReviewID" });
            DropIndex("dbo.COBSecurityReviewAssociatedAccounts", new[] { "AssociatedAccount_AssociatedAccountID" });
            CreateTable(
                "dbo.AssociatedAccountCOBSecurityReviews",
                c => new
                    {
                        AssociatedAccountID = c.Int(nullable: false),
                        COBSecurityReviewID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AssociatedAccountID, t.COBSecurityReviewID });
            
            DropTable("dbo.COBSecurityReviewAssociatedAccounts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.COBSecurityReviewAssociatedAccounts",
                c => new
                    {
                        COBSecurityReview_COBSecurityReviewID = c.Int(nullable: false),
                        AssociatedAccount_AssociatedAccountID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.COBSecurityReview_COBSecurityReviewID, t.AssociatedAccount_AssociatedAccountID });
            
            DropTable("dbo.AssociatedAccountCOBSecurityReviews");
            CreateIndex("dbo.COBSecurityReviewAssociatedAccounts", "AssociatedAccount_AssociatedAccountID");
            CreateIndex("dbo.COBSecurityReviewAssociatedAccounts", "COBSecurityReview_COBSecurityReviewID");
            AddForeignKey("dbo.COBSecurityReviewAssociatedAccounts", "AssociatedAccount_AssociatedAccountID", "dbo.AssociatedAccounts", "AssociatedAccountID", cascadeDelete: true);
            AddForeignKey("dbo.COBSecurityReviewAssociatedAccounts", "COBSecurityReview_COBSecurityReviewID", "dbo.COBSecurityReviews", "COBSecurityReviewID", cascadeDelete: true);
        }
    }
}
