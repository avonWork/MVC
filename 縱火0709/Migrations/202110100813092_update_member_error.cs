namespace 縱火0709.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_member_error : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Members", "Error", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Members", "Error", c => c.String());
        }
    }
}
