namespace DDL_CapstoneProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUpdateTimeCommentProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "UpdatedDate", c => c.DateTime());
            DropColumn("dbo.Project", "UpdateDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Project", "UpdateDate", c => c.DateTime());
            DropColumn("dbo.Project", "UpdatedDate");
        }
    }
}
