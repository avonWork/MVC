namespace 縱火0709.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_user_account : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Account", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Account");
        }
    }
}
