namespace Zk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UIntGrowingTime : DbMigration
    {
        public override void Up()
        {
            DropColumn("public.Crops", "GrowingTime");
            AddColumn("public.Crops", "GrowingTime", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("public.Crops", "GrowingTime");
        }
    }
}
