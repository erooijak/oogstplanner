namespace Oogstplanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LikesOnDeleteCascade : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("public.Likes", "Calendar_Id", "public.Calendars");
            DropForeignKey("public.Likes", "User_Id", "public.Users");
            DropIndex("public.Likes", new[] { "Calendar_Id" });
            DropIndex("public.Likes", new[] { "User_Id" });
            AlterColumn("public.Likes", "Calendar_Id", c => c.Int(nullable: false));
            AlterColumn("public.Likes", "User_Id", c => c.Int(nullable: false));
            CreateIndex("public.Likes", "Calendar_Id");
            CreateIndex("public.Likes", "User_Id");
            AddForeignKey("public.Likes", "Calendar_Id", "public.Calendars", "Id", cascadeDelete: true);
            AddForeignKey("public.Likes", "User_Id", "public.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("public.Likes", "User_Id", "public.Users");
            DropForeignKey("public.Likes", "Calendar_Id", "public.Calendars");
            DropIndex("public.Likes", new[] { "User_Id" });
            DropIndex("public.Likes", new[] { "Calendar_Id" });
            AlterColumn("public.Likes", "User_Id", c => c.Int());
            AlterColumn("public.Likes", "Calendar_Id", c => c.Int());
            CreateIndex("public.Likes", "User_Id");
            CreateIndex("public.Likes", "Calendar_Id");
            AddForeignKey("public.Likes", "User_Id", "public.Users", "Id");
            AddForeignKey("public.Likes", "Calendar_Id", "public.Calendars", "Id");
        }
    }
}
