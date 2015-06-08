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
                "public.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Calendar_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.Calendars", t => t.Calendar_Id)
                .ForeignKey("public.Users", t => t.User_Id)
                .Index(t => t.Calendar_Id)
                .Index(t => t.User_Id);
            
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
                "public.Followers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Followed_Id = c.Int(),
                        Following_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.Users", t => t.Followed_Id)
                .ForeignKey("public.Users", t => t.Following_Id)
                .Index(t => t.Followed_Id)
                .Index(t => t.Following_Id);
            
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
            DropForeignKey("public.Followers", "Following_Id", "public.Users");
            DropForeignKey("public.Followers", "Followed_Id", "public.Users");
            DropForeignKey("public.Calendars", "User_Id", "public.Users");
            DropForeignKey("public.Likes", "User_Id", "public.Users");
            DropForeignKey("public.Likes", "Calendar_Id", "public.Calendars");
            DropForeignKey("public.FarmingActions", "Crop_Id", "public.Crops");
            DropForeignKey("public.FarmingActions", "Calendar_Id", "public.Calendars");
            DropIndex("public.Followers", new[] { "Following_Id" });
            DropIndex("public.Followers", new[] { "Followed_Id" });
            DropIndex("public.Likes", new[] { "User_Id" });
            DropIndex("public.Likes", new[] { "Calendar_Id" });
            DropIndex("public.FarmingActions", new[] { "Crop_Id" });
            DropIndex("public.FarmingActions", new[] { "Calendar_Id" });
            DropIndex("public.Calendars", new[] { "User_Id" });
            DropTable("public.PasswordResetTokens");
            DropTable("public.Followers");
            DropTable("public.Users");
            DropTable("public.Likes");
            DropTable("public.Crops");
            DropTable("public.FarmingActions");
            DropTable("public.Calendars");
        }
    }
}
