namespace Formula1DataApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class emptytest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sponsors", "SponsorCountry", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sponsors", "SponsorCountry");
        }
    }
}
