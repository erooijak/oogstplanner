using System;
using System.Data.Entity.Migrations;
using Zk.Models;

namespace Zk
{
	/// <summary>
	/// 	Class used to generate the SQL script based on the Models (i.e., enable code-first migrations)
	///     See: http://stackoverflow.com/questions/20374783/enable-entity-framework-migrations-in-mono#20382226
	/// </summary>
	public class Configuration : DbMigrationsConfiguration<ZkContext>
	{
		public Configuration ()
		{
		}


	}
}

