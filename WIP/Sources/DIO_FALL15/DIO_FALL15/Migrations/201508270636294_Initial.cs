namespace DIO_FALL15.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Back",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        BackedDate = c.DateTime(nullable: false),
                        BackAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: false)
                .ForeignKey("dbo.Project", t => t.ProjectId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50),
                        Status = c.Int(nullable: false),
                        Description = c.String(),
                        ImageLink = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        Deadline = c.DateTime(nullable: false),
                        TargetFund = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrentFund = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: false)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Status = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 20),
                        Password = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Gender = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        PhoneNumber = c.String(),
                        Address = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Back", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.Project", "UserId", "dbo.User");
            DropForeignKey("dbo.Back", "UserId", "dbo.User");
            DropForeignKey("dbo.Project", "CategoryId", "dbo.Category");
            DropIndex("dbo.Project", new[] { "CategoryId" });
            DropIndex("dbo.Project", new[] { "UserId" });
            DropIndex("dbo.Back", new[] { "ProjectId" });
            DropIndex("dbo.Back", new[] { "UserId" });
            DropTable("dbo.User");
            DropTable("dbo.Category");
            DropTable("dbo.Project");
            DropTable("dbo.Back");
        }
    }
}
