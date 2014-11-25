using System;
using System.Data.Entity.Migrations;
using Zk.Models;
using System.Data.Entity.Migrations.Design;
using System.IO;
using System.Resources;
using System.Text.RegularExpressions;
using System.Data.Entity.Migrations.Infrastructure;

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
            // USER INPUT

            // While specifiying true here only a new Migration is Added. 
            // Specify false to run Update-Database and create a script.
            const bool ADD_NEW_MIGRATION = true;

            // Specify the name of the database migration
            // Note: make sure to create a new name for each new migration and prefix with
            const string MIGRATION_NAME = "Initial";

            // END USER INPUT


            // Get executing path from which the location of the Update_Scripts and new Migrations can be determined.
            var executingPath = AppDomain.CurrentDomain.BaseDirectory; 

            // Add a new migration (PowerShell: Add-Migration)
            if (ADD_NEW_MIGRATION) {
    
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
                Console.WriteLine("EF code migration {0} written to Migrations folder...\n" +
                    "Make sure to include them in the project by right clicking on the project >" +  
                    "Add files from folder.", migration.MigrationId);
            }

            // If a new migation is created the database can be updated and the PostgreSQL script be created.
            // TODO: This does not work yet.
            else 
            {
                // Write to database (PowerShell: Update-Database)
                var config = new Configuration();
                var migrator = new DbMigrator(config);
                migrator.Update();

                // Now create the PostgreSQL update script.
                var scriptor = new MigratorScriptingDecorator (migrator);
                string script = scriptor.ScriptUpdate (sourceMigration: null, targetMigration: null);

                var updateScriptPath = Regex.Replace (executingPath, "Zk.Migrations/.*", "Zk/App_Data/Update_Scripts");
                File.WriteAllText (updateScriptPath + MIGRATION_NAME + ".postgresql", script);
                Console.WriteLine ("Update script {0} written to App_Data/Update_Scripts folder", MIGRATION_NAME);
            }
		}
	}

}