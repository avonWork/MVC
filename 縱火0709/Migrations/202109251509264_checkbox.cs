namespace 縱火0709.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class checkbox : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Members", "InternationalMembership", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Members", "InternationalMembership", c => c.Boolean());
        }
    }
}
