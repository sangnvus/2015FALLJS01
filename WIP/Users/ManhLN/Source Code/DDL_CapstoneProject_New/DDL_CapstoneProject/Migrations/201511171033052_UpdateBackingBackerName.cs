namespace DDL_CapstoneProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBackingBackerName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BackingDetail", "BackerName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BackingDetail", "BackerName");
        }
    }
}
