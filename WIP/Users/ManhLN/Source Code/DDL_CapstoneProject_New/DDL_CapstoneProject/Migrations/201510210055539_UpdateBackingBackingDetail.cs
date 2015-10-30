namespace DDL_CapstoneProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBackingBackingDetail : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Backing", new[] { "User_DDL_UserID" });
            DropColumn("dbo.Backing", "UserID");
            RenameColumn(table: "dbo.Backing", name: "User_DDL_UserID", newName: "UserID");
            AddColumn("dbo.Backing", "IsDelivery", c => c.Boolean(nullable: false));
            AddColumn("dbo.BackingDetail", "PhoneNumber", c => c.String());
            AddColumn("dbo.BackingDetail", "Email", c => c.String());
            AddColumn("dbo.Project", "IsFunded", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Backing", "UserID", c => c.Int(nullable: false));
            CreateIndex("dbo.Backing", "UserID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Backing", new[] { "UserID" });
            AlterColumn("dbo.Backing", "UserID", c => c.Int());
            DropColumn("dbo.Project", "IsFunded");
            DropColumn("dbo.BackingDetail", "Email");
            DropColumn("dbo.BackingDetail", "PhoneNumber");
            DropColumn("dbo.Backing", "IsDelivery");
            RenameColumn(table: "dbo.Backing", name: "UserID", newName: "User_DDL_UserID");
            AddColumn("dbo.Backing", "UserID", c => c.Int(nullable: false));
            CreateIndex("dbo.Backing", "User_DDL_UserID");
        }
    }
}
