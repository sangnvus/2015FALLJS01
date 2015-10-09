namespace DDL_CapstoneProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditUserInfoRewardPkg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RewardPkg", "PledgeAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.UserInfo", "PhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserInfo", "PhoneNumber");
            DropColumn("dbo.RewardPkg", "PledgeAmount");
        }
    }
}
