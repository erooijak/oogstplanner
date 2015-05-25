namespace Oogstplanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFollowers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Followers",
                c => new
                    {
                        FollowerId = c.Int(nullable: false, identity: true),
                        Followed_Id = c.Int(),
                        Following_Id = c.Int(),
                    })
                .PrimaryKey(t => t.FollowerId)
                .ForeignKey("public.Users", t => t.Followed_Id)
                .ForeignKey("public.Users", t => t.Following_Id)
                .Index(t => t.Followed_Id)
                .Index(t => t.Following_Id);
            
            AddColumn("public.Users", "User_Id", c => c.Int());
            CreateIndex("public.Users", "User_Id");
            AddForeignKey("public.Users", "User_Id", "public.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("public.Followers", "Following_Id", "public.Users");
            DropForeignKey("public.Followers", "Followed_Id", "public.Users");
            DropForeignKey("public.Users", "User_Id", "public.Users");
            DropIndex("public.Followers", new[] { "Following_Id" });
            DropIndex("public.Followers", new[] { "Followed_Id" });
            DropIndex("public.Users", new[] { "User_Id" });
            DropColumn("public.Users", "User_Id");
            DropTable("public.Followers");
        }
    }
}
