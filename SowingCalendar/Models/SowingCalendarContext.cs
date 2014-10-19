using System;
using System.Data.Entity;

namespace SowingCalendar.Models
{
    public class SowingCalendarContext : DbContext, ISowingCalendarContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SowingCalendar.Models.SowingCalendarContext"/> class.
        /// </summary>
        public SowingCalendarContext() : base("SowingCalendarDatabaseConnection")
        {
        }

        #region Database tables

        public IDbSet<Crop> Crops { get; set; } 

        #endregion
    }
}

