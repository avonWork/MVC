namespace 縱火0709.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_contact : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contacts", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contacts", "Email", c => c.String());
        }
    }
}
