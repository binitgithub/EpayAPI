namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResetPasswordRequest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResetPasswordRequests",
                c => new
                    {
                        ResetPasswordRequestID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        Clicked = c.DateTime(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ResetPasswordRequestID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ResetPasswordRequests");
        }
    }
}
