namespace 縱火0709.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Experience : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserHobbys", newName: "HobbysUsers");
            DropPrimaryKey("dbo.HobbysUsers");
            CreateTable(
                "dbo.Experiences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentEmployer = c.String(),
                        JobTitle = c.String(),
                        WorkStartYear = c.String(),
                        WorkStartMonth = c.String(),
                        WorkEndYear = c.String(),
                        WorkEndMonth = c.String(),
                        MemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.MemId, cascadeDelete: true)
                .Index(t => t.MemId);
            
            AddPrimaryKey("dbo.HobbysUsers", new[] { "Hobbys_Id", "User_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Experiences", "MemId", "dbo.Members");
            DropIndex("dbo.Experiences", new[] { "MemId" });
            DropPrimaryKey("dbo.HobbysUsers");
            DropTable("dbo.Experiences");
            AddPrimaryKey("dbo.HobbysUsers", new[] { "User_Id", "Hobbys_Id" });
            RenameTable(name: "dbo.HobbysUsers", newName: "UserHobbys");
        }
    }
}
