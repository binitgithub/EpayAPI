namespace E_Pay_Web_API.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using E_Pay_Web_API.Models;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<E_Pay_Web_API.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "E_Pay_Web_API.Models.ApplicationDbContext";
        }

        protected override void Seed(E_Pay_Web_API.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.FinancialInstitutions.AddOrUpdate(
                p => p.FinancialInstitutionID,
                new Models.FinancialInstitution {
                    FinancialInstitutionID =Guid.Parse("8DB34F14-A886-4BDB-AC85-2351CDD0F715"),
                    Name = "City Of Bridgetown Cooperative Credit Union Ltd.",
                    Created = DateTime.Now }
                );

            context.COBSecurityQuestions.AddOrUpdate(
                p => p.COBSecurityQuestionID,
                new COBSecurityQuestion
                {
                    COBSecurityQuestionID = 1,
                    SecurityQuestionText = "What is the name of your favourite Secondary School teacher?",
                    Active = true
                },
                new COBSecurityQuestion
                {
                    COBSecurityQuestionID = 2,
                    SecurityQuestionText = "What is the name of the first company you worked for?",
                    Active = true
                },
                new COBSecurityQuestion
                {
                    COBSecurityQuestionID = 3,
                    SecurityQuestionText = "What is your favourite vacation destination?",
                    Active = true
                });
            //Guid institutionId = context.FinancialInstitutions.First().FinancialInstitutionID;
            //UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            //ApplicationUser user = manager.FindByName("kevin.gilkes@live.com");
            //if (user != null)
            //{
            //    context.AssociatedAccounts.AddOrUpdate(
            //        p => p.AssociatedAccountID,
            //        new AssociatedAccount
            //        {
            //            AssociatedAccountID = 1,
            //            FinancialInstitutionID = institutionId,
            //            UserID = user.Id,
            //            AccountNumber = "11234456",
            //            AccountType = "Savings",
            //            Description = "Test Savings Account",
            //            MemberID = "KGilkes",
            //            DefaultAccount = true,
            //            Verified = DateTime.Now,
            //            Created = DateTime.Now,
            //        },
            //        new AssociatedAccount
            //        {
            //            AssociatedAccountID = 2,
            //            FinancialInstitutionID = institutionId,
            //            UserID = user.Id,
            //            AccountNumber = "9987665",
            //            AccountType = "Chequing",
            //            Description = "Test Chequing Account",
            //            MemberID = "KGilkes",
            //            DefaultAccount = true,
            //            Verified = DateTime.Now,
            //            Created = DateTime.Now,
            //        });

            //    context.Transfers.AddOrUpdate(
            //        p => p.TransferID,
            //        new Transfer
            //        {
            //            TransferID = 1,
            //            SourceInstitutionID = institutionId,
            //            SourceAccountNumber = "11234456",
            //            DestinationInstitutionID = institutionId.ToString(),
            //            DestinationAccountNumber = "9987665",
            //            TransferAmount = 500.00m,
            //            TransferDescription = "Grocery Expenses",
            //            Status = "Successful",
            //            StatusDescription = "",
            //            Created = DateTime.Now
            //        },
            //        new Transfer
            //        {
            //            TransferID = 2,
            //            SourceInstitutionID = institutionId,
            //            SourceAccountNumber = "11234456",
            //            DestinationInstitutionID = institutionId.ToString(),
            //            DestinationAccountNumber = "9987665",
            //            TransferAmount = 325.00m,
            //            TransferDescription = "Utility Bills",
            //            Status = "Successful",
            //            StatusDescription = "",
            //            Created = DateTime.Now
            //        },
            //        new Transfer
            //        {
            //            TransferID = 3,
            //            SourceInstitutionID = institutionId,
            //            SourceAccountNumber = "9987665",
            //            DestinationInstitutionID = "Digicel-Bds",
            //            DestinationAccountNumber = "2468325502",
            //            TransferAmount = 50.00m,
            //            TransferDescription = "Digicel-Bds: Mom's Cell TopUp",
            //            Status = "Successful",
            //            StatusDescription = "",
            //            Created = DateTime.Now
            //        });
            //}
        }
    }
}
