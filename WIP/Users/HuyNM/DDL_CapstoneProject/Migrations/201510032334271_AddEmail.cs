namespace DDL_CapstoneProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DDL_User", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DDL_User", "Email");
        }
    }
}
