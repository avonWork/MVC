namespace 縱火0709.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user_delete_setUp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Sticky", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "Error", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Error", c => c.String());
            DropColumn("dbo.Users", "Sticky");
        }
    }
}
