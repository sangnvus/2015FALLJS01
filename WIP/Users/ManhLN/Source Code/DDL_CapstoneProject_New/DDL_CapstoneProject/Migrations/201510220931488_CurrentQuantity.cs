namespace DDL_CapstoneProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CurrentQuantity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RewardPkg", "CurrentQuantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RewardPkg", "CurrentQuantity");
        }
    }
}
