using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Zk.Repositories;
using Zk.Models;

namespace Zk.Controllers
{
	public class HomeController : Controller
	{

		readonly Repository _repo;

		/// <summary>
		///     Initializes a new instance of the <see cref="Controllers.HomeController"/> class which
		///     makes use of the real Entity Framework context that connects with the database.
		/// </summary>
		public HomeController()
		{
			_repo = new Repository();
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Controllers.HomeController"/> class which
		///     can make use of a "Fake" Entity Framework context for unit testing purposes.
		/// </summary>
		/// <param name="db">Database context.</param>
		public HomeController(IZkContext db)
		{
			_repo = new Repository(db);
		}

		public ActionResult Index ()
		{
			var mvcName = typeof(Controller).Assembly.GetName ();
			var isMono = Type.GetType ("Mono.Runtime") != null;

			ViewData ["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
			ViewData ["Runtime"] = isMono ? "Mono" : ".NET";

			return View ();
		}
	}
}