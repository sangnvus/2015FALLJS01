namespace DDL_CapstoneProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Backing",
                c => new
                    {
                        BackingID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        RewardPkgID = c.Int(nullable: false),
                        BackedDate = c.DateTime(nullable: false),
                        PledgedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        Description = c.String(),
                        IsPublic = c.Boolean(nullable: false),
                        User_DDL_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.BackingID)
                .ForeignKey("dbo.RewardPkg", t => t.RewardPkgID)
                .ForeignKey("dbo.DDL_User", t => t.User_DDL_UserID)
                .Index(t => t.RewardPkgID)
                .Index(t => t.User_DDL_UserID);
            
            CreateTable(
                "dbo.RewardPkg",
                c => new
                    {
                        RewardPkgID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        Type = c.String(),
                        Quantity = c.Int(nullable: false),
                        EstimatedDelivery = c.DateTime(),
                        IsHide = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RewardPkgID)
                .ForeignKey("dbo.Project", t => t.ProjectID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        ProjectID = c.Int(nullable: false, identity: true),
                        ProjectCode = c.String(),
                        CategoryID = c.Int(nullable: false),
                        CreatorID = c.Int(nullable: false),
                        Title = c.String(),
                        Risk = c.String(),
                        ImageUrl = c.String(),
                        SubDescription = c.String(),
                        Location = c.String(),
                        IsExprired = c.Boolean(nullable: false),
                        CurrentFunded = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpireDate = c.DateTime(),
                        FundingGoal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                        VideoUrl = c.String(),
                        PopularPoint = c.Int(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ProjectID)
                .ForeignKey("dbo.Category", t => t.CategoryID)
                .ForeignKey("dbo.DDL_User", t => t.CreatorID)
                .Index(t => t.CategoryID)
                .Index(t => t.CreatorID);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        ProjectID = c.Int(nullable: false),
                        CommentContent = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        IsHide = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Project", t => t.ProjectID)
                .ForeignKey("dbo.DDL_User", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.DDL_User",
                c => new
                    {
                        DDL_UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        LoginType = c.String(),
                        UserType = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastLogin = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        IsVerify = c.Boolean(nullable: false),
                        VerifyCode = c.String(),
                    })
                .PrimaryKey(t => t.DDL_UserID);
            
            CreateTable(
                "dbo.Conversation",
                c => new
                    {
                        ConversationID = c.Int(nullable: false, identity: true),
                        CreatorID = c.Int(nullable: false),
                        ReceiverID = c.Int(nullable: false),
                        Subject = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                        DeleteStatus = c.String(),
                        ViewStatus = c.String(),
                    })
                .PrimaryKey(t => t.ConversationID)
                .ForeignKey("dbo.DDL_User", t => t.CreatorID)
                .ForeignKey("dbo.DDL_User", t => t.ReceiverID)
                .Index(t => t.CreatorID)
                .Index(t => t.ReceiverID);
            
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        MessageID = c.Int(nullable: false, identity: true),
                        ConversationID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        MessageContent = c.String(),
                        SentTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MessageID)
                .ForeignKey("dbo.Conversation", t => t.ConversationID)
                .ForeignKey("dbo.DDL_User", t => t.UserID)
                .Index(t => t.ConversationID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.ReportProject",
                c => new
                    {
                        ReportID = c.Int(nullable: false, identity: true),
                        ReporterID = c.Int(nullable: false),
                        ProjectID = c.Int(nullable: false),
                        Subject = c.String(),
                        ReportContent = c.String(),
                        ReportedDate = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ReportID)
                .ForeignKey("dbo.Project", t => t.ProjectID)
                .ForeignKey("dbo.DDL_User", t => t.ReporterID)
                .Index(t => t.ReporterID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.ReportUser",
                c => new
                    {
                        ReportID = c.Int(nullable: false, identity: true),
                        ReporterID = c.Int(nullable: false),
                        ReportedUserID = c.Int(nullable: false),
                        Subject = c.String(),
                        ReportContent = c.String(),
                        ReportedDate = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ReportID)
                .ForeignKey("dbo.DDL_User", t => t.ReportedUserID)
                .ForeignKey("dbo.DDL_User", t => t.ReporterID)
                .Index(t => t.ReporterID)
                .Index(t => t.ReportedUserID);
            
            CreateTable(
                "dbo.Remind",
                c => new
                    {
                        RemindID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        ProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RemindID)
                .ForeignKey("dbo.Project", t => t.ProjectID)
                .ForeignKey("dbo.DDL_User", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.UserInfo",
                c => new
                    {
                        DDL_UserID = c.Int(nullable: false),
                        FullName = c.String(),
                        ProfileImage = c.String(),
                        Gender = c.String(),
                        DateOfBirth = c.DateTime(),
                        Country = c.String(),
                        Address = c.String(),
                        Website = c.String(),
                        FacebookUrl = c.String(),
                        Biography = c.String(),
                    })
                .PrimaryKey(t => t.DDL_UserID)
                .ForeignKey("dbo.DDL_User", t => t.DDL_UserID)
                .Index(t => t.DDL_UserID);
            
            CreateTable(
                "dbo.Question",
                c => new
                    {
                        QuestionID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        QuestionContent = c.String(),
                        Answer = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionID)
                .ForeignKey("dbo.Project", t => t.ProjectID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.Timeline",
                c => new
                    {
                        TimelineID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        DueDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TimelineID)
                .ForeignKey("dbo.Project", t => t.ProjectID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.UpdateLog",
                c => new
                    {
                        UpdateLogID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UpdateLogID)
                .ForeignKey("dbo.Project", t => t.ProjectID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.Slide",
                c => new
                    {
                        SlideID = c.Int(nullable: false, identity: true),
                        Order = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        ImageUrl = c.String(),
                        SlideUrl = c.String(),
                        ButtonColor = c.String(),
                        ButtonText = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SlideID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UpdateLog", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.Timeline", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.RewardPkg", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.Question", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.Project", "CreatorID", "dbo.DDL_User");
            DropForeignKey("dbo.UserInfo", "DDL_UserID", "dbo.DDL_User");
            DropForeignKey("dbo.Remind", "UserID", "dbo.DDL_User");
            DropForeignKey("dbo.Remind", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.ReportUser", "ReporterID", "dbo.DDL_User");
            DropForeignKey("dbo.ReportUser", "ReportedUserID", "dbo.DDL_User");
            DropForeignKey("dbo.ReportProject", "ReporterID", "dbo.DDL_User");
            DropForeignKey("dbo.ReportProject", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.Conversation", "ReceiverID", "dbo.DDL_User");
            DropForeignKey("dbo.Message", "UserID", "dbo.DDL_User");
            DropForeignKey("dbo.Message", "ConversationID", "dbo.Conversation");
            DropForeignKey("dbo.Conversation", "CreatorID", "dbo.DDL_User");
            DropForeignKey("dbo.Comment", "UserID", "dbo.DDL_User");
            DropForeignKey("dbo.Backing", "User_DDL_UserID", "dbo.DDL_User");
            DropForeignKey("dbo.Comment", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.Project", "CategoryID", "dbo.Category");
            DropForeignKey("dbo.Backing", "RewardPkgID", "dbo.RewardPkg");
            DropIndex("dbo.UpdateLog", new[] { "ProjectID" });
            DropIndex("dbo.Timeline", new[] { "ProjectID" });
            DropIndex("dbo.Question", new[] { "ProjectID" });
            DropIndex("dbo.UserInfo", new[] { "DDL_UserID" });
            DropIndex("dbo.Remind", new[] { "ProjectID" });
            DropIndex("dbo.Remind", new[] { "UserID" });
            DropIndex("dbo.ReportUser", new[] { "ReportedUserID" });
            DropIndex("dbo.ReportUser", new[] { "ReporterID" });
            DropIndex("dbo.ReportProject", new[] { "ProjectID" });
            DropIndex("dbo.ReportProject", new[] { "ReporterID" });
            DropIndex("dbo.Message", new[] { "UserID" });
            DropIndex("dbo.Message", new[] { "ConversationID" });
            DropIndex("dbo.Conversation", new[] { "ReceiverID" });
            DropIndex("dbo.Conversation", new[] { "CreatorID" });
            DropIndex("dbo.Comment", new[] { "ProjectID" });
            DropIndex("dbo.Comment", new[] { "UserID" });
            DropIndex("dbo.Project", new[] { "CreatorID" });
            DropIndex("dbo.Project", new[] { "CategoryID" });
            DropIndex("dbo.RewardPkg", new[] { "ProjectID" });
            DropIndex("dbo.Backing", new[] { "User_DDL_UserID" });
            DropIndex("dbo.Backing", new[] { "RewardPkgID" });
            DropTable("dbo.Slide");
            DropTable("dbo.UpdateLog");
            DropTable("dbo.Timeline");
            DropTable("dbo.Question");
            DropTable("dbo.UserInfo");
            DropTable("dbo.Remind");
            DropTable("dbo.ReportUser");
            DropTable("dbo.ReportProject");
            DropTable("dbo.Message");
            DropTable("dbo.Conversation");
            DropTable("dbo.DDL_User");
            DropTable("dbo.Comment");
            DropTable("dbo.Category");
            DropTable("dbo.Project");
            DropTable("dbo.RewardPkg");
            DropTable("dbo.Backing");
        }
    }
}
