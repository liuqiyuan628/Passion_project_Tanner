namespace Formula1DataApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class drivers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        DriverID = c.Int(nullable: false, identity: true),
                        DriverName = c.String(),
                        DriverNumber = c.Int(nullable: false),
                        DriverPoints = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DriverCountry = c.String(),
                    })
                .PrimaryKey(t => t.DriverID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Drivers");
        }
    }
}
