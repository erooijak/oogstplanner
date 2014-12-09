using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Zk.Helpers;
using Zk.Models;
using Zk.Repositories;

namespace Zk.Controllers
{
    public class FarmingActionController : Controller
    {
        readonly Repository _repo;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Controllers.FarmingActionController"/> class which
        ///     makes use of the real Entity Framework context that connects with the database.
        /// </summary>
        public FarmingActionController()
        {
            _repo = new Repository();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Controllers.FarmingActionController"/> class which
        ///     can make use of a "Fake" Entity Framework context for unit testing purposes.
        /// </summary>
        /// <param name="db">Database context.</param>
        public FarmingActionController(IZkContext db)
        {
            _repo = new Repository(db);
        }

        /// <summary>
        ///     GET: /Edit/{month}
        ///     Returns the selected month.
        /// 
        ///     TODO: Better model binding! Remove JavaScript and just bind data to partial view. (?)
        /// </summary>
        /// <returns></returns>
        /// <param name="month">Requested month.</param>
        public ActionResult Edit(Month month)
        {
            // Get farming actions (TODO: of user))
            var farmingActions = _repo.GetFarmingActions(month);

            // Serialize to JSON with a the month prepended and converting enum values to string.
            var farmingActionsJsonString = JsonConvert.SerializeObject(
                new {month, farmingActions}, new StringEnumConverter());

            return new JsonStringResult(farmingActionsJsonString);
        }

    }
}