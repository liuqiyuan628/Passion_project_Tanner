namespace Formula1DataApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class driverteam : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "TeamID", c => c.Int(nullable: false));
            CreateIndex("dbo.Drivers", "TeamID");
            AddForeignKey("dbo.Drivers", "TeamID", "dbo.Teams", "TeamID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Drivers", "TeamID", "dbo.Teams");
            DropIndex("dbo.Drivers", new[] { "TeamID" });
            DropColumn("dbo.Drivers", "TeamID");
        }
    }
}
