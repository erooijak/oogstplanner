using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web.Security;

using Zk.Helpers;
using Zk.Models;

namespace Zk.Repositories
{
    /// <summary>
    ///     Repository used for methods that access the database.
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

        public void Update(params object[] entities)
        {
            foreach (var entity in entities) _db.SetModified(entity);
        }

        // A "leaky abstraction", why not use context directly?
        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public Crop GetCrop(int id)
        {
            return _db.Crops.Single(c => c.Id == id);
        }

        public Crop GetCrop(string name)
        {
            return _db.Crops.Single(c => c.Name == name);
        }

        public IEnumerable<Crop> GetAllCrops()
        {
            return _db.Crops.OrderBy(c => c.Id);
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

        /// <summary>
        ///     This methods finds the action that belongs to the one given as a parameter.
        /// </summary>
        /// <example>
        ///     When an action is passed that says we have to harvest a broccoli in May,
        ///     and a broccoli has a growing time of four months,
        ///     this method returns the sowing action of a broccoli of four months ago
        ///     which belongs to the same calendar and user.
        /// </example>
        /// <returns>The related farming action.</returns>
        /// <param name="action"></param>
        public FarmingAction FindRelatedFarmingAction(FarmingAction action)
        {
            // Arrange values to be found
            var crop = action.Crop;
            var cropGrowingTime = action.Crop.GrowingTime;
            var calendar = action.Calendar;
            ActionType type;
            Month month;

            if (action.Action == ActionType.Harvesting)
            {
                type = ActionType.Sowing;
                month = action.Month.Subtract(cropGrowingTime);
            }
            else
            {
                type = ActionType.Harvesting;
                month = action.Month.Add(cropGrowingTime);
            }

            return _db.FarmingActions.Where(fa => fa.Calendar.CalendarId == calendar.CalendarId
                && fa.Action == type
                && fa.Crop.Id == crop.Id
                && fa.Month == month).FirstOrDefault();
        }

        public void AddFarmingAction(FarmingAction farmingAction)
        {
            _db.FarmingActions.Add(farmingAction);
        }

        public void AddUser(string userName, string fullName, string email)
        {
            // Note: it is not necessary to check if user profile already exists since membership provider
            //       does this for us.

            var user = new User
            {
                Name = userName,
                FullName = fullName,
                Email = email
            };

            _db.Users.Add(user);
            _db.SaveChanges();

        }

        public User GetUser(IPrincipal user)
        {
            return _db.Users.Single(u => u.Name == user.Identity.Name);
        }

        public User GetUserById(int id)
        {
            return _db.Users.Find(id);
        }

        public int GetUserIdByUserName(string name)
        {
            var user = _db.Users.Where(u => u.Name == name).First();

            if (user == null)
                throw new ArgumentException("The user with the specified name does not exist.");
          
            return user.UserId; 
        }

        public MembershipUser GetMembershipUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            var foundUser = _db.Users.Where(u => u.Email == email).FirstOrDefault();

            return foundUser != null
                ? Membership.GetUser(foundUser.Name)
                : null;
        }

        public void StoreResetToken(string email, DateTime timeResetRequested, string token)
        {
            var passwordReset = new PasswordResetToken
            {
                Email = email,
                TimeStamp = timeResetRequested,
                Token = token
            };

            _db.PasswordResetTokens.Add(passwordReset);
            _db.SaveChanges();
        }

        public MembershipUser GetMembershipUserFromToken(string token)
        {
            string email = null;

            var passwordResetInstance = _db.PasswordResetTokens.Where(pr => pr.Token == token).FirstOrDefault();
            if (passwordResetInstance != null)
            {
                email = passwordResetInstance.Email;
            }

            return GetMembershipUserByEmail(email);
        }

        public DateTime? GetTokenTimeStamp(string token)
        {
            var tokenInfo = _db.PasswordResetTokens.Where(prt => prt.Token == token).FirstOrDefault();

            return tokenInfo != null ? tokenInfo.TimeStamp : (DateTime?)null;
        }

    }
}