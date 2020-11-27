namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_BillerUsers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BillerUsers",
                c => new
                    {
                        BillerID = c.Guid(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 128),
                        IsAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.BillerID, t.UserID });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BillerUsers");
        }
    }
}
