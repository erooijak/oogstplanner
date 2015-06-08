namespace Oogstplanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLikes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("public.Users", "User_Id", "public.Users");
            DropForeignKey("public.Calendars", "User_Id", "public.Users");
            DropIndex("public.Calendars", new[] { "User_Id" });
            DropIndex("public.Users", new[] { "User_Id" });
            AddColumn("public.Calendars", "User_Id1", c => c.Int(nullable: false));
            AddColumn("public.Users", "Calendar_Id", c => c.Int());
            AlterColumn("public.Calendars", "User_Id", c => c.Int());
            CreateIndex("public.Calendars", "User_Id");
            CreateIndex("public.Calendars", "User_Id1");
            CreateIndex("public.Users", "Calendar_Id");
            AddForeignKey("public.Users", "Calendar_Id", "public.Calendars", "Id");
            AddForeignKey("public.Calendars", "User_Id1", "public.Users", "Id", cascadeDelete: true);
            AddForeignKey("public.Calendars", "User_Id", "public.Users", "Id");
            DropColumn("public.Users", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("public.Users", "User_Id", c => c.Int());
            DropForeignKey("public.Calendars", "User_Id", "public.Users");
            DropForeignKey("public.Calendars", "User_Id1", "public.Users");
            DropForeignKey("public.Users", "Calendar_Id", "public.Calendars");
            DropIndex("public.Users", new[] { "Calendar_Id" });
            DropIndex("public.Calendars", new[] { "User_Id1" });
            DropIndex("public.Calendars", new[] { "User_Id" });
            AlterColumn("public.Calendars", "User_Id", c => c.Int(nullable: false));
            DropColumn("public.Users", "Calendar_Id");
            DropColumn("public.Calendars", "User_Id1");
            CreateIndex("public.Users", "User_Id");
            CreateIndex("public.Calendars", "User_Id");
            AddForeignKey("public.Calendars", "User_Id", "public.Users", "Id", cascadeDelete: true);
            AddForeignKey("public.Users", "User_Id", "public.Users", "Id");
        }
    }
}
