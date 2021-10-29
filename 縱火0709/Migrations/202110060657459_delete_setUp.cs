namespace 縱火0709.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delete_setUp : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "Sticky", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.News", "Sticky", c => c.Boolean());
        }
    }
}
