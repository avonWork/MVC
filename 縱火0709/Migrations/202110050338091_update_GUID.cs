namespace 縱火0709.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_GUID : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Experiences", "MemId", "dbo.Members");
            DropIndex("dbo.Experiences", new[] { "MemId" });
            RenameColumn(table: "dbo.Experiences", name: "MemId", newName: "MemUserId");
            DropPrimaryKey("dbo.Members");
            AddColumn("dbo.Members", "UserId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Members", "CheckAccount", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Experiences", "MemUserId", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.Members", "UserId");
            CreateIndex("dbo.Experiences", "MemUserId");
            AddForeignKey("dbo.Experiences", "MemUserId", "dbo.Members", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Experiences", "MemUserId", "dbo.Members");
            DropIndex("dbo.Experiences", new[] { "MemUserId" });
            DropPrimaryKey("dbo.Members");
            AlterColumn("dbo.Experiences", "MemUserId", c => c.Int(nullable: false));
            DropColumn("dbo.Members", "CheckAccount");
            DropColumn("dbo.Members", "UserId");
            AddPrimaryKey("dbo.Members", "Id");
            RenameColumn(table: "dbo.Experiences", name: "MemUserId", newName: "MemId");
            CreateIndex("dbo.Experiences", "MemId");
            AddForeignKey("dbo.Experiences", "MemId", "dbo.Members", "Id", cascadeDelete: true);
        }
    }
}
