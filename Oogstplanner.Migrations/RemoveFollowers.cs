namespace Oogstplanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFollowers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("public.Followers", "Followed_Id", "public.Users");
            DropForeignKey("public.Followers", "Following_Id", "public.Users");
            DropIndex("public.Followers", new[] { "Followed_Id" });
            DropIndex("public.Followers", new[] { "Following_Id" });
            AddColumn("public.Users", "LastActive", c => c.DateTime(nullable: false));
            DropColumn("public.Users", "CreationDate");
            DropTable("public.Followers");
        }
        
        public override void Down()
        {
            CreateTable(
                "public.Followers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Followed_Id = c.Int(),
                        Following_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("public.Users", "CreationDate", c => c.DateTime(nullable: false));
            DropColumn("public.Users", "LastActive");
            CreateIndex("public.Followers", "Following_Id");
            CreateIndex("public.Followers", "Followed_Id");
            AddForeignKey("public.Followers", "Following_Id", "public.Users", "Id");
            AddForeignKey("public.Followers", "Followed_Id", "public.Users", "Id");
        }
    }
}
