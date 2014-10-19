using System;
using System.Linq;
using SowingCalendar.Models;
using System.Data.Entity;

namespace SowingCalendar.Repository
{
    /// <summary>
    ///     Repository used for methods that access the database.
    /// </summary>
    public class Repository : IRepository
    {
        private readonly ISowingCalendarContext db; // The interface to Entity Framework database context

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SowingCalendar.Repository.Repository"/> class which
        ///     makes use of the real Entity Framework context that connects with the database.
        /// </summary>
        public Repository()
        {
            db = new SowingCalendarContext();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SowingCalendar.Repository.Repository"/> class which
        ///     can make use of a "Fake" Entity Framework context for unit testing purposes.
        /// </summary>
        /// <param name="dbParam">Database context.</param>
        public Repository(ISowingCalendarContext dbParam)
        {
            db = dbParam;
        }

        #endregion

        #region Crop

        public Crop GetCrop(int id)
        {
            var crop = db.Crops.Single(cr => cr.Id == id);
            db.SaveChanges();

            return crop;
        }

        public Crop GetCrop(string name)
        {
            var crop = db.Crops.Single(cr => cr.Name == name);
            db.SaveChanges();

            return crop;
        }

        #endregion
    }
}

