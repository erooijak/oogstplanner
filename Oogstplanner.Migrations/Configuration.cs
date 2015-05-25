using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Sql;

using Oogstplanner.Data;

namespace Oogstplanner.Migrations
{
    public class Configuration : DbMigrationsConfiguration<OogstplannerContext>
    {
        public Configuration ()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("Npgsql", new PostgreSqlMigrationSqlGenerator());
        }

    }
}
