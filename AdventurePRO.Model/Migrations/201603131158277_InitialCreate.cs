namespace AdventurePRO.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Adventures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Currency = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        FinishDate = c.DateTime(nullable: false),
                        Destination_ID = c.Int(),
                        Home_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Destinations", t => t.Destination_ID)
                .ForeignKey("dbo.Destinations", t => t.Home_ID)
                .Index(t => t.Destination_ID)
                .Index(t => t.Home_ID);
            
            CreateTable(
                "dbo.Destinations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        Country_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Countries", t => t.Country_ID)
                .Index(t => t.Country_ID);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Adventures", "Home_ID", "dbo.Destinations");
            DropForeignKey("dbo.Adventures", "Destination_ID", "dbo.Destinations");
            DropForeignKey("dbo.Destinations", "Country_ID", "dbo.Countries");
            DropIndex("dbo.Destinations", new[] { "Country_ID" });
            DropIndex("dbo.Adventures", new[] { "Home_ID" });
            DropIndex("dbo.Adventures", new[] { "Destination_ID" });
            DropTable("dbo.Countries");
            DropTable("dbo.Destinations");
            DropTable("dbo.Adventures");
        }
    }
}
