namespace 縱火0709.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class article : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Article_Title = c.String(nullable: false),
                        Article_name = c.String(),
                        Article_Content = c.String(),
                        Sticky = c.Boolean(),
                        AddUserId = c.Int(nullable: false),
                        AddTime = c.DateTime(),
                        EditUserId = c.Int(),
                        EditTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Replies",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ArtID = c.Int(nullable: false),
                        Reply_name = c.String(),
                        Reply_Content = c.String(),
                        Sticky = c.Boolean(),
                        AddUserId = c.Int(nullable: false),
                        AddTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Articles", t => t.ArtID, cascadeDelete: true)
                .Index(t => t.ArtID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Replies", "ArtID", "dbo.Articles");
            DropIndex("dbo.Replies", new[] { "ArtID" });
            DropTable("dbo.Replies");
            DropTable("dbo.Articles");
        }
    }
}
