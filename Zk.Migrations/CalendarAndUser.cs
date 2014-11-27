namespace Zk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CalendarAndUser : DbMigration
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
                        SowingPatternAsJson = c.String(),
                        HarvestingPatternAsJson = c.String(),
                        Calendar_CalendarId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.Calendars", t => t.Calendar_CalendarId)
                .Index(t => t.Calendar_CalendarId);
            
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
            DropForeignKey("public.FarmingMonths", "Calendar_CalendarId", "public.Calendars");
            DropIndex("public.FarmingMonths", new[] { "Calendar_CalendarId" });
            DropIndex("public.Calendars", new[] { "UserId" });
            DropTable("public.Users");
            DropTable("public.FarmingMonths");
            DropTable("public.Calendars");
        }
    }
}
