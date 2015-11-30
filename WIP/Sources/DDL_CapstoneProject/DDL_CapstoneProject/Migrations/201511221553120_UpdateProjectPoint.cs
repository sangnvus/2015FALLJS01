namespace DDL_CapstoneProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProjectPoint : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "PointOfTheDay", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "PointOfTheDay");
        }
    }
}
