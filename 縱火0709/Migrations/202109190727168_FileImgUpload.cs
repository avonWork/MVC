namespace 縱火0709.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileImgUpload : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileClasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        fileName = c.String(nullable: false),
                        fileUrl = c.String(),
                        fileText = c.String(),
                        fileorder = c.Int(nullable: false),
                        Sticky = c.Boolean(),
                        AddUserId = c.Int(nullable: false),
                        AddTime = c.DateTime(),
                        EditUserId = c.Int(),
                        EditTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ImgClasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        imgName = c.String(),
                        imgUrl = c.String(),
                        imgText = c.String(),
                        imgorder = c.Int(nullable: false),
                        Sticky = c.Boolean(),
                        AddUserId = c.Int(nullable: false),
                        AddTime = c.DateTime(),
                        EditUserId = c.Int(),
                        EditTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ImgClasses");
            DropTable("dbo.FileClasses");
        }
    }
}
