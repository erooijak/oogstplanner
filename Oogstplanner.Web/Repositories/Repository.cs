using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Security;

using Oogstplanner.Utilities.Helpers;
using Oogstplanner.Models;

namespace Oogstplanner.Repositories
{
    /// <summary>
    ///     Repository used for methods that access the database.
    /// </summary>
    public class Repository
    {

        readonly IOogstplannerContext db; // Entity Framework database context

        /// <summary>
        ///     Initializes a new instance of the <see cref="Repositories.Repository"/> class which
        ///     can make use of a "Fake" Entity Framework context for unit testing purposes.
        /// </summary>
        /// <param name="db">Database context.</param>
        public Repository(IOogstplannerContext db)
        {
            this.db = db;
        }

        public void Update(params object[] entities)
        {
            foreach (var entity in entities) db.SetModified(entity);
        }

        // A "leaky abstraction", why not use context directly?
        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public Crop GetCrop(int id)
        {
            return db.Crops.Single(c => c.Id == id);
        }

        public IEnumerable<Crop> GetAllCrops()
        {
            return db.Crops;
        }

        public IEnumerable<FarmingAction> GetFarmingActions(Expression<Func<FarmingAction, bool>> predicate)
        {
            return db.FarmingActions.Where(predicate).ToList<FarmingAction>();
        }

        public FarmingAction FindFarmingAction(int id)
        {
            var action = db.FarmingActions.Find(id);
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

            var actionType = FarmingActionHelper.GetRelatedActionType(action);
            var month = FarmingActionHelper.GetRelatedMonth(action, cropGrowingTime);

            return db.FarmingActions.SingleOrDefault(fa => fa.Calendar.CalendarId == calendar.CalendarId
                && fa.Action == actionType
                && fa.Crop.Id == crop.Id
                && fa.Month == month);
        }

        public void AddFarmingAction(FarmingAction farmingAction)
        {
            db.FarmingActions.Add(farmingAction);
        }

        public void RemoveFarmingAction(FarmingAction farmingAction)
        {
            db.FarmingActions.Remove(farmingAction);
        }

        public void AddUser(User user)
        {
            // Note: it is not necessary to check if user profile already exists since membership provider
            //       does this for us.

            db.Users.Add(user);
            db.SaveChanges();

        }

        public User GetUserById(int id)
        {
            return db.Users.Find(id);
        }

        public User GetUserByUserName(string name)
        {
            var user = db.Users.SingleOrDefault(u => u.Name == name);

            if (user == null)
                throw new ArgumentException("The user with the specified name does not exist.");

            return user; 
        }

        public int GetUserIdByEmail(string email)
        {
            var user = db.Users.SingleOrDefault(u => u.Email == email);

            if (user == null)
                throw new ArgumentException("The user with the specified email does not exist.");
          
            return user.UserId; 
        }

        public int GetUserIdByName(string name)
        {
            var user = db.Users.SingleOrDefault(u => u.Name == name);

            if (user == null)
                throw new ArgumentException("The user with the specified name does not exist.");

            return user.UserId; 
        }

        public Calendar GetCalendar(int userId)
        {
            return db.Calendars.SingleOrDefault(c => c.User.UserId == userId);
        }

        public Month GetMonthsWithActions(int userId)
        {
            return db.FarmingActions.Where(fa => fa.Calendar.UserId == userId)
                .Select(fa => fa.Month)
                .Distinct()
                .ToList()
                .Aggregate((Month)0, (acc, month) => acc |= month);
        }

        public void CreateCalendar(User user)
        {
            var calendar = new Calendar { User = user };

            db.Calendars.Add(calendar);
            db.SaveChanges();
        }

        public MembershipUser GetMembershipUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            var userName = Membership.GetUserNameByEmail(email);

            return Membership.GetUser(userName);
        }

        public void StoreResetToken(string email, DateTime timeResetRequested, string token)
        {
            var passwordReset = new PasswordResetToken
            {
                Email = email,
                TimeStamp = timeResetRequested,
                Token = token
            };

            db.PasswordResetTokens.Add(passwordReset);
            db.SaveChanges();
        }

        public MembershipUser GetMembershipUserFromToken(string token)
        {
            string email = null;

            var passwordResetInstance = db.PasswordResetTokens.FirstOrDefault(pr => pr.Token == token);
            if (passwordResetInstance != null)
            {
                email = passwordResetInstance.Email;
            }

            return GetMembershipUserByEmail(email);
        }

        public DateTime? GetTokenTimeStamp(string token)
        {
            var tokenInfo = db.PasswordResetTokens.FirstOrDefault(prt => prt.Token == token);

            return tokenInfo != null ? tokenInfo.TimeStamp : (DateTime?)null;
        }

    }
}