namespace Oogstplanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAuthenticationStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Users", "AuthenticationStatus", c => c.Int(nullable: false));
            AddColumn("public.Users", "CreationDate", c => c.DateTime(nullable: false));
            DropColumn("public.Users", "Enabled");
        }
        
        public override void Down()
        {
            AddColumn("public.Users", "Enabled", c => c.Boolean(nullable: false));
            DropColumn("public.Users", "CreationDate");
            DropColumn("public.Users", "AuthenticationStatus");
        }
    }
}
