namespace 縱火0709.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class upAddTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Replies", "AddTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Replies", "AddTime", c => c.DateTime());
        }
    }
}
