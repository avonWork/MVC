namespace 縱火0709.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class member_sticky_set : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "Sticky", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "Sticky");
        }
    }
}
