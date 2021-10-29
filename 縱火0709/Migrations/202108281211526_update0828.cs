namespace 縱火0709.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update0828 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "Hobby");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Hobby", c => c.String());
        }
    }
}
