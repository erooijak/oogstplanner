namespace Oogstplanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeFollowersPrimaryKeyName : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("public.Followers");
            AddColumn("public.Followers", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("public.Followers", "Id");
            DropColumn("public.Followers", "FollowerId");
        }
        
        public override void Down()
        {
            AddColumn("public.Followers", "FollowerId", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("public.Followers");
            DropColumn("public.Followers", "Id");
            AddPrimaryKey("public.Followers", "FollowerId");
        }
    }
}
