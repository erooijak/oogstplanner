using System.Data.Entity;

namespace SowingCalendar.Models
{
    public class SowingCalendarContext : DbContext, ISowingCalendarContext
    {

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Models.SowingCalendarContext"/> class.
        /// </summary>
        public SowingCalendarContext() : base("SowingCalendarDatabaseConnection")
        {
        }

        #endregion

        #region Database tables

        public IDbSet<Crop> Crops { get; set; } 

        #endregion
    }
}

