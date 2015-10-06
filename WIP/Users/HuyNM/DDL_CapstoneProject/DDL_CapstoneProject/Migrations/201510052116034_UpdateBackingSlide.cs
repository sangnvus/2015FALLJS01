namespace DDL_CapstoneProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBackingSlide : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Backing", "RewardPkgID", "dbo.RewardPkg");
            DropIndex("dbo.Backing", new[] { "RewardPkgID" });
            CreateTable(
                "dbo.BackingDetail",
                c => new
                    {
                        BackingID = c.Int(nullable: false),
                        RewardPkgID = c.Int(nullable: false),
                        PledgedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        Description = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.BackingID)
                .ForeignKey("dbo.Backing", t => t.BackingID)
                .ForeignKey("dbo.RewardPkg", t => t.RewardPkgID)
                .Index(t => t.BackingID)
                .Index(t => t.RewardPkgID);
            
            AddColumn("dbo.Backing", "ProjectID", c => c.Int(nullable: false));
            AddColumn("dbo.Timeline", "ImageUrl", c => c.String());
            AddColumn("dbo.Slide", "TextColor", c => c.String());
            AddColumn("dbo.Slide", "VideoUrl", c => c.String());
            CreateIndex("dbo.Backing", "ProjectID");
            AddForeignKey("dbo.Backing", "ProjectID", "dbo.Project", "ProjectID");
            DropColumn("dbo.Backing", "RewardPkgID");
            DropColumn("dbo.Backing", "PledgedAmount");
            DropColumn("dbo.Backing", "Quantity");
            DropColumn("dbo.Backing", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Backing", "Description", c => c.String());
            AddColumn("dbo.Backing", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.Backing", "PledgedAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Backing", "RewardPkgID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Backing", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.BackingDetail", "RewardPkgID", "dbo.RewardPkg");
            DropForeignKey("dbo.BackingDetail", "BackingID", "dbo.Backing");
            DropIndex("dbo.BackingDetail", new[] { "RewardPkgID" });
            DropIndex("dbo.BackingDetail", new[] { "BackingID" });
            DropIndex("dbo.Backing", new[] { "ProjectID" });
            DropColumn("dbo.Slide", "VideoUrl");
            DropColumn("dbo.Slide", "TextColor");
            DropColumn("dbo.Timeline", "ImageUrl");
            DropColumn("dbo.Backing", "ProjectID");
            DropTable("dbo.BackingDetail");
            CreateIndex("dbo.Backing", "RewardPkgID");
            AddForeignKey("dbo.Backing", "RewardPkgID", "dbo.RewardPkg", "RewardPkgID");
        }
    }
}
