namespace 縱火0709.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePwd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Members", "Password2", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Members", "Password2", c => c.String());
        }
    }
}
