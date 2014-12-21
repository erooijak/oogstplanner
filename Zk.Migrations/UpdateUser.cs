namespace Zk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Users", "FullName", c => c.String());
            AddColumn("public.Users", "Email", c => c.String());
            AddColumn("public.Users", "Enabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("public.Users", "Enabled");
            DropColumn("public.Users", "Email");
            DropColumn("public.Users", "FullName");
        }
    }
}
