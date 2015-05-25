namespace Oogstplanner.Migrations
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
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "public.FarmingActions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Month = c.Int(nullable: false),
                        Action = c.Int(nullable: false),
                        CropCount = c.Int(nullable: false),
                        Calendar_Id = c.Int(nullable: false),
                        Crop_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.Calendars", t => t.Calendar_Id, cascadeDelete: true)
                .ForeignKey("public.Crops", t => t.Crop_Id)
                .Index(t => t.Calendar_Id)
                .Index(t => t.Crop_Id);
            
            CreateTable(
                "public.Crops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Race = c.String(),
                        Category = c.String(),
                        GrowingTime = c.Int(nullable: false),
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
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FullName = c.String(),
                        Email = c.String(),
                        AuthenticationStatus = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "public.PasswordResetTokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Token = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.Calendars", "User_Id", "public.Users");
            DropForeignKey("public.FarmingActions", "Crop_Id", "public.Crops");
            DropForeignKey("public.FarmingActions", "Calendar_Id", "public.Calendars");
            DropIndex("public.FarmingActions", new[] { "Crop_Id" });
            DropIndex("public.FarmingActions", new[] { "Calendar_Id" });
            DropIndex("public.Calendars", new[] { "User_Id" });
            DropTable("public.PasswordResetTokens");
            DropTable("public.Users");
            DropTable("public.Crops");
            DropTable("public.FarmingActions");
            DropTable("public.Calendars");
        }
    }
}
