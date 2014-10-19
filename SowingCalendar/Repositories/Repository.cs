using System.Linq;
using SowingCalendar.Models;

namespace SowingCalendar.Repositories
{
    /// <summary>
    ///     Repository used for methods that access the database.
    /// </summary>
    public class Repository : IRepository
    {
        readonly ISowingCalendarContext _db; // The interface to Entity Framework database context

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Repositories.Repository"/>class which
        ///     makes use of the real Entity Framework context that connects with the database.
        /// </summary>
        public Repository()
        {
            _db = new SowingCalendarContext();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Repositories.Repository"/> class which
        ///     can make use of a "Fake" Entity Framework context for unit testing purposes.
        /// </summary>
        /// <param name="dbParam">Database context.</param>
        public Repository(ISowingCalendarContext dbParam)
        {
            _db = dbParam;
        }

        #endregion

        #region Crops

        public Crop GetCrop(int id)
        {
            var crop = _db.Crops.Single(c => c.Id == id);
            _db.SaveChanges();

            return crop;
        }

        public Crop GetCrop(string name)
        {
            var crop = _db.Crops.Single(c => c.Name == name);
            _db.SaveChanges();

            return crop;
        }

        #endregion
    }
}

