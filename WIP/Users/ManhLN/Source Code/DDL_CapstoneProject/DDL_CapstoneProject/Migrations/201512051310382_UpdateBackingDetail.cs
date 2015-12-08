namespace DDL_CapstoneProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBackingDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BackingDetail", "OrderId", c => c.String());
            AddColumn("dbo.BackingDetail", "TransactionId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BackingDetail", "TransactionId");
            DropColumn("dbo.BackingDetail", "OrderId");
        }
    }
}
