namespace Zk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Crops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Race = c.String(),
                        Category = c.String(),
                        AreaPerCrop = c.Double(),
                        AreaPerBag = c.Double(),
                        PricePerBag = c.Decimal(precision: 18, scale: 2),
                        SowingMonths = c.Int(nullable: false),
                        HarvestingMonths = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("public.Crops");
        }
    }
}
