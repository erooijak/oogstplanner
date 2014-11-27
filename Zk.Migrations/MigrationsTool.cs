using System;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Design;
using System.Data.Entity.Migrations.Infrastructure;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text.RegularExpressions;

namespace Zk.Migrations
{
	/// <summary>
	/// 	Class used to generate the code migrations and SQL script based on the Models and update the database.
    ///     (I.e., runs PowerShell's Add-Migration and Update-Database, and creates a PostgreSQL script.)
	///     See: http://stackoverflow.com/questions/20374783/enable-entity-framework-migrations-in-mono#20382226
	/// 
    ///     Usage: run by setting Zk.Migrations as Startup project and pressing play.
    /// 
	/// 	Classes of namespace EntityFramework.PostgreSql obtained from:
	///     https://github.com/darionato/PostgreSqlMigrationSqlGenerator. License is included.
	/// </summary>
	class MigrationsTool
	{
        /// <summary>
        ///     The entry point of the program, where the program control starts and ends.
        /// </summary>
		public static void Main()
		{
            // USER INPUT /////////////////////////////////////////////////////////////////////////////////

            // Always first create a new database migration with DatabaseStep.ADD_MIGRATION,
            // and include the created files in the project and set resource file to EmbeddedResource. 
            // After creating a migration run UPDATE_DATABASE to update the database.

            const DatabaseStep step = DatabaseStep.ADD_MIGRATION;

            // Specify the name of the database migration in case of ADD-MIGRATION.
            // Note: Make sure to create a new name for each new migration.
            //       After creating migration include the files in the folder by right clicking on 
            //       Zk.Migrations and selecting "Add files from folder". Then add the .cs, .resx and
            //       .Designer.cs files with the name specified below.
            //       Last but not least set the .resx file's build action to EmbeddedResource by right
            //       clicking on it.
            // Make sure that the Initial.postgresql script has run manually to create the database user.

            const string MIGRATION_NAME = "Initial";

            // END USER INPUT /////////////////////////////////////////////////////////////////////////////


            // Get executing path from which the location of the Update_Scripts and new Migrations can be determined.
            var executingPath = AppDomain.CurrentDomain.BaseDirectory; 

            // Add a new migration (PowerShell: Add-Migration)
            if (step == DatabaseStep.ADD_MIGRATION) {
    
                // Initialize the wrapper classes around the Entity Framework PowerShell API.
                var config = new Configuration();
                var scaffolder = new MigrationScaffolder(config); 
                var migration = scaffolder.Scaffold(MIGRATION_NAME);
        
                // Place migration code in main project "Migrations" folder and migration scripts in "App_Data"
                var migrationsPath = Regex.Replace(executingPath, "bin/.*", "");
            
                // Write migrations
                File.WriteAllText (migrationsPath + migration.MigrationId + ".cs", migration.UserCode);
                File.WriteAllText (migrationsPath + migration.MigrationId + ".Designer.cs", migration.DesignerCode);

                using (var writer = new ResXResourceWriter (migrationsPath + migration.MigrationId + ".resx")) 
                {
                    foreach (var resource in migration.Resources) 
                    {
                        writer.AddResource(resource.Key, resource.Value);
                    }
                }
                Console.WriteLine("EF code migration {0} written to Migrations folder...\n\n" +
                    "Make sure to include them in the project by right clicking on the project > " +  
                    "\"Add files from folder.\"\n" +
                    "And right click on {0}.resx and set build action to \"EmbeddedResource\""
                    , migration.MigrationId);
            }

            // If a new migration is created the database can be updated. (PowerShell: Update-Database)
            else if (step == DatabaseStep.UPDATE_DATABASE)
            {
                var config = new Configuration();
                var migrator = new DbMigrator(config);

                // Write to database
                migrator.Update();

                // Show which migrations were applied.
                var migrationNames = string.Join(", ", migrator.GetDatabaseMigrations().ToArray());
                Console.WriteLine("Applied migration {0} to database.", migrationNames);
            }
		}

        /// <summary>
        /// Enumeration for specifying the step in the migration.
        /// </summary>
        private enum DatabaseStep 
        {
            ADD_MIGRATION,
            UPDATE_DATABASE
        }

	}

}