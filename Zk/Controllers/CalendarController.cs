﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Zk.Models;
using Zk.Repositories;

namespace Zk.Controllers
{
	public class CalendarController : Controller
	{
		readonly Repository _repo;

		/// <summary>
        ///     Initializes a new instance of the <see cref="Controllers.CalendarController"/> class which
		///     makes use of the real Entity Framework context that connects with the database.
		/// </summary>
		public CalendarController()
		{
			_repo = new Repository();
		}

		/// <summary>
        ///     Initializes a new instance of the <see cref="Controllers.CalendarController"/> class which
		///     can make use of a "Fake" Entity Framework context for unit testing purposes.
		/// </summary>
		/// <param name="db">Database context.</param>
		public CalendarController(IZkContext db)
		{
			_repo = new Repository(db);
		}
            		
	}
}