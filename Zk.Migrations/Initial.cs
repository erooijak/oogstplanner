namespace Zk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Calendars",
                c => new
                    {
                        CalendarId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CalendarId)
                .ForeignKey("public.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "public.FarmingMonths",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Month = c.Int(nullable: false),
                        Action = c.Int(nullable: false),
                        CropCount = c.Int(nullable: false),
                        Calendar_CalendarId = c.Int(),
                        Crop_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.Calendars", t => t.Calendar_CalendarId)
                .ForeignKey("public.Crops", t => t.Crop_Id)
                .Index(t => t.Calendar_CalendarId)
                .Index(t => t.Crop_Id);
            
            CreateTable(
                "public.Crops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Race = c.String(),
                        Category = c.String(),
                        AreaPerCrop = c.Double(),
                        AreaPerBag = c.Double(),
                        PricePerBag = c.Decimal(precision: 18, scale: 2),
                        SowingMonths = c.Int(nullable: false),
                        HarvestingMonths = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "public.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.Calendars", "UserId", "public.Users");
            DropForeignKey("public.FarmingMonths", "Crop_Id", "public.Crops");
            DropForeignKey("public.FarmingMonths", "Calendar_CalendarId", "public.Calendars");
            DropIndex("public.FarmingMonths", new[] { "Crop_Id" });
            DropIndex("public.FarmingMonths", new[] { "Calendar_CalendarId" });
            DropIndex("public.Calendars", new[] { "UserId" });
            DropTable("public.Users");
            DropTable("public.Crops");
            DropTable("public.FarmingMonths");
            DropTable("public.Calendars");
        }
    }
}
