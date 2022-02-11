namespace Formula1DataApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teams : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamID = c.Int(nullable: false, identity: true),
                        TeamName = c.String(),
                        TeamCountry = c.String(),
                        TeamYear = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TeamID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Teams");
        }
    }
}
