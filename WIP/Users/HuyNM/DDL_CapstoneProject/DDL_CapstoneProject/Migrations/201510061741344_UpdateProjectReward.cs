namespace DDL_CapstoneProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProjectReward : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RewardPkg", "Description", c => c.String());
            AddColumn("dbo.Project", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "CreatedDate");
            DropColumn("dbo.RewardPkg", "Description");
        }
    }
}
