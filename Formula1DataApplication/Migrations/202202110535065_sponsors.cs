namespace Formula1DataApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sponsors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sponsors",
                c => new
                    {
                        SponsorID = c.Int(nullable: false, identity: true),
                        SponsorName = c.String(),
                    })
                .PrimaryKey(t => t.SponsorID);
            
            CreateTable(
                "dbo.SponsorDrivers",
                c => new
                    {
                        Sponsor_SponsorID = c.Int(nullable: false),
                        Driver_DriverID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Sponsor_SponsorID, t.Driver_DriverID })
                .ForeignKey("dbo.Sponsors", t => t.Sponsor_SponsorID, cascadeDelete: true)
                .ForeignKey("dbo.Drivers", t => t.Driver_DriverID, cascadeDelete: true)
                .Index(t => t.Sponsor_SponsorID)
                .Index(t => t.Driver_DriverID);
            
            AddColumn("dbo.Drivers", "DriverFirstName", c => c.String());
            AddColumn("dbo.Drivers", "DriverLastName", c => c.String());
            DropColumn("dbo.Drivers", "DriverName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Drivers", "DriverName", c => c.String());
            DropForeignKey("dbo.SponsorDrivers", "Driver_DriverID", "dbo.Drivers");
            DropForeignKey("dbo.SponsorDrivers", "Sponsor_SponsorID", "dbo.Sponsors");
            DropIndex("dbo.SponsorDrivers", new[] { "Driver_DriverID" });
            DropIndex("dbo.SponsorDrivers", new[] { "Sponsor_SponsorID" });
            DropColumn("dbo.Drivers", "DriverLastName");
            DropColumn("dbo.Drivers", "DriverFirstName");
            DropTable("dbo.SponsorDrivers");
            DropTable("dbo.Sponsors");
        }
    }
}
