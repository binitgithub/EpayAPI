namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSecurityQuestions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.COBSecurityQuestions",
                c => new
                    {
                        COBSecurityQuestionID = c.Int(nullable: false, identity: true),
                        SecurityQuestionText = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.COBSecurityQuestionID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.COBSecurityQuestions");
        }
    }
}
