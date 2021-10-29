namespace 縱火0709.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class member : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account = c.String(nullable: false, maxLength: 50),
                        TypeId = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Password1 = c.String(nullable: false, maxLength: 50),
                        Password2 = c.String(),
                        PasswordSalt = c.String(maxLength: 100),
                        Birthday = c.DateTime(nullable: false),
                        Authority = c.String(),
                        Error = c.String(),
                        Email = c.String(nullable: false),
                        Sex = c.Int(nullable: false),
                        Address = c.String(nullable: false),
                        InternationalMembership = c.Boolean(),
                        CurrentEmployer = c.String(nullable: false),
                        JobTitle = c.String(nullable: false),
                        HighestEducation = c.String(nullable: false),
                        TotalRelevantYears = c.String(nullable: false),
                        TotalRelevantMonths = c.String(nullable: false),
                        InsertAdminID = c.Int(),
                        InsertTime = c.DateTime(),
                        EditAdminID = c.Int(),
                        EditTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserTypes", t => t.TypeId, cascadeDelete: true)
                .Index(t => t.TypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Members", "TypeId", "dbo.UserTypes");
            DropIndex("dbo.Members", new[] { "TypeId" });
            DropTable("dbo.Members");
        }
    }
}
