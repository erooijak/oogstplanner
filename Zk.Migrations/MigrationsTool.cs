using System;
using System.Data.Entity.Migrations;
using Zk.Models;
using System.Data.Entity.Migrations.Design;
using System.IO;
using System.Resources;

namespace Zk.Migrations
{
	/// <summary>
	/// 	Class used to generate the SQL script based on the Models (i.e., enable code-first migrations)
	///     See: http://stackoverflow.com/questions/20374783/enable-entity-framework-migrations-in-mono#20382226
	/// 
	/// 	Classes of namespace EntityFramework.PostgreSql obtained from:
	///     https://github.com/darionato/PostgreSqlMigrationSqlGenerator. License is included.
	/// </summary>
	class MigrationsTool
	{

		public static void Main (string[] args)
		{
			var config = new Configuration();
			var scaffolder = new MigrationScaffolder(config); 
			// ^^ Not working because of MultipleActiveResultSets which are set to true somewhere unknown...
			var migration = scaffolder.Scaffold("Initial");

			File.WriteAllText(migration.MigrationId + ".cs", migration.UserCode);

			File.WriteAllText(migration.MigrationId + ".Designer.cs", migration.DesignerCode);

			using (var writer = new ResXResourceWriter(migration.MigrationId + ".resx"))
			{
				foreach (var resource in migration.Resources)
				{
					writer.AddResource(resource.Key, resource.Value);
				}
			}
		}
	}

}

