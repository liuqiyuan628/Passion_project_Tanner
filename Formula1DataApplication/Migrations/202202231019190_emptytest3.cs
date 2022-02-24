namespace Formula1DataApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class emptytest3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Sponsors", "SponsorCountry");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sponsors", "SponsorCountry", c => c.String());
        }
    }
}
