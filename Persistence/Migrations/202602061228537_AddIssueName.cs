namespace Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIssueName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Issues", "Name", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Issues", "Name");
        }
    }
}
