namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class approachchange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssociatedAccounts",
                c => new
                    {
                        AssociatedAccountID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        FinancialInstitutionID = c.Guid(nullable: false),
                        MemberID = c.String(),
                        AccountNumber = c.String(),
                        AccountType = c.String(),
                        Description = c.String(),
                        DefaultAccount = c.Boolean(nullable: false),
                        Verified = c.DateTime(nullable: false),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AssociatedAccountID);
            
            CreateTable(
                "dbo.COBSecurityResponses",
                c => new
                    {
                        COBSecurityResponseID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        QuestionText = c.String(),
                        ResponseText = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.COBSecurityResponseID);
            
            CreateTable(
                "dbo.COBSecurityReviews",
                c => new
                    {
                        COBSecurityReviewID = c.Int(nullable: false, identity: true),
                        ReviewerID = c.String(),
                        RevieweeID = c.String(),
                        Status = c.String(),
                        StatusDescription = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.COBSecurityReviewID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.COBSecurityReviews");
            DropTable("dbo.COBSecurityResponses");
            DropTable("dbo.AssociatedAccounts");
        }
    }
}
