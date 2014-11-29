namespace Zk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FarmingMonthImproved : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "public.FarmingMonths", name: "Calendar_CalendarId", newName: "CalendarId_CalendarId");
            RenameIndex(table: "public.FarmingMonths", name: "IX_Calendar_CalendarId", newName: "IX_CalendarId_CalendarId");
            AddColumn("public.FarmingMonths", "Action", c => c.Int(nullable: false));
            AddColumn("public.FarmingMonths", "CropCount", c => c.Int(nullable: false));
            AddColumn("public.FarmingMonths", "Crop_Id", c => c.Int());
            CreateIndex("public.FarmingMonths", "Crop_Id");
            AddForeignKey("public.FarmingMonths", "Crop_Id", "public.Crops", "Id");
            DropColumn("public.FarmingMonths", "SowingPatternAsJson");
            DropColumn("public.FarmingMonths", "HarvestingPatternAsJson");
        }
        
        public override void Down()
        {
            AddColumn("public.FarmingMonths", "HarvestingPatternAsJson", c => c.String());
            AddColumn("public.FarmingMonths", "SowingPatternAsJson", c => c.String());
            DropForeignKey("public.FarmingMonths", "Crop_Id", "public.Crops");
            DropIndex("public.FarmingMonths", new[] { "Crop_Id" });
            DropColumn("public.FarmingMonths", "Crop_Id");
            DropColumn("public.FarmingMonths", "CropCount");
            DropColumn("public.FarmingMonths", "Action");
            RenameIndex(table: "public.FarmingMonths", name: "IX_CalendarId_CalendarId", newName: "IX_Calendar_CalendarId");
            RenameColumn(table: "public.FarmingMonths", name: "CalendarId_CalendarId", newName: "Calendar_CalendarId");
        }
    }
}
