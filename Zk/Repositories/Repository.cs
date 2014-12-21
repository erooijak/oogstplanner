using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;

using Zk.Models;

namespace Zk.Repositories
{
	/// <summary>
	///     Repository used for methods that access the database ... and some business logic.
	/// </summary>
	public class Repository
	{
		readonly IZkContext _db; // The interface to Entity Framework database context

		/// <summary>
		///     Initializes a new instance of the <see cref="Repositories.Repository"/>class which
		///     makes use of the real Entity Framework context that connects with the database.
		/// </summary>
		public Repository()
		{
			_db = new ZkContext();
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Repositories.Repository"/> class which
		///     can make use of a "Fake" Entity Framework context for unit testing purposes.
		/// </summary>
		/// <param name="db">Database context.</param>
		public Repository(IZkContext db)
		{
			_db = db;
		}

        public void Update(object entity)
        {
            _db.SetModified(entity);
            _db.SaveChanges();
        }
			
		public Crop GetCrop(int id)
		{
			var crop = _db.Crops.Single(c => c.Id == id);

			return crop;
		}

		public Crop GetCrop(string name)
		{
			var crop = _db.Crops.Single(c => c.Name == name);

			return crop;
		}

		public IEnumerable<Crop> GetAllCrops()
		{
			var crops = _db.Crops.OrderBy(c => c.Id);

			return crops;
		}

        public IEnumerable<FarmingAction> GetFarmingActions(Expression<Func<FarmingAction, bool>> predicate)
        {
            return _db.FarmingActions.Where(predicate).ToList<FarmingAction>();
        }    

        public FarmingAction FindFarmingAction(int id)
        {
            var action = _db.FarmingActions.Find(id);
            if (action == null)
                throw new ArgumentException("Cannot find primary key in database.", "id");

            return action;
        }

        public void AddUser(string userName, string fullName, string email)
        {
            // Not necessary to check if userprofile already exists since forms does this for us.

            //if (!_db.Users.Any(u => u.Name.ToLower() == userName)) {

                // Else insert into the profile table
                var user = new User {
                    Name = userName,
                    FullName = fullName,
                    Email = email
                };

                _db.Users.Add(user);
                _db.SaveChanges();
            //}

        }

        public User GetUser(IPrincipal user)
        {
            return _db.Users.Single(u => u.Name == user.Identity.Name);
        }

        public User GetUserById(int id)
        {
            return _db.Users.Find(id);
        }

	}
}