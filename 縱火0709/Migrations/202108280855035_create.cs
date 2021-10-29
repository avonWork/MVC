namespace 縱火0709.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hobbys",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HobbyName = c.String(nullable: false),
                        InsertAdminID = c.Int(),
                        InsertTime = c.DateTime(),
                        EditAdminID = c.Int(),
                        EditTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeId = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 50),
                        UserImg = c.String(),
                        UserPassword = c.String(nullable: false, maxLength: 50),
                        PasswordSalt = c.String(maxLength: 100),
                        Authority = c.String(),
                        Error = c.String(),
                        UserPhone = c.String(nullable: false),
                        UserEmail = c.String(nullable: false),
                        Sex = c.Int(nullable: false),
                        Hobby = c.String(),
                        Ramark = c.String(),
                        InsertAdminID = c.Int(),
                        InsertTime = c.DateTime(),
                        EditAdminID = c.Int(),
                        EditTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserTypes", t => t.TypeId, cascadeDelete: true)
                .Index(t => t.TypeId);
            
            CreateTable(
                "dbo.UserTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        New_Title = c.String(nullable: false),
                        New_Message = c.String(),
                        New_img = c.String(),
                        New_Content = c.String(),
                        Sticky = c.Boolean(),
                        AddUserId = c.Int(nullable: false),
                        AddTime = c.DateTime(),
                        EditUserId = c.Int(),
                        EditTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Premissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Pid = c.Int(),
                        PValue = c.String(),
                        Url = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Premissions", t => t.Pid)
                .Index(t => t.Pid);
            
            CreateTable(
                "dbo.UserHobbys",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Hobbys_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Hobbys_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Hobbys", t => t.Hobbys_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Hobbys_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Premissions", "Pid", "dbo.Premissions");
            DropForeignKey("dbo.Users", "TypeId", "dbo.UserTypes");
            DropForeignKey("dbo.UserHobbys", "Hobbys_Id", "dbo.Hobbys");
            DropForeignKey("dbo.UserHobbys", "User_Id", "dbo.Users");
            DropIndex("dbo.UserHobbys", new[] { "Hobbys_Id" });
            DropIndex("dbo.UserHobbys", new[] { "User_Id" });
            DropIndex("dbo.Premissions", new[] { "Pid" });
            DropIndex("dbo.Users", new[] { "TypeId" });
            DropTable("dbo.UserHobbys");
            DropTable("dbo.Premissions");
            DropTable("dbo.News");
            DropTable("dbo.UserTypes");
            DropTable("dbo.Users");
            DropTable("dbo.Hobbys");
        }
    }
}
