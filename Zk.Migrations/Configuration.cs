using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Sql;
using Zk.Models;

namespace Zk.Migrations
{
	public class Configuration : DbMigrationsConfiguration<ZkContext>
	{
		public Configuration ()
		{
			AutomaticMigrationsEnabled = false;
            SetSqlGenerator("Npgsql", new PostgreSqlMigrationSqlGenerator());
		}

	}
}