using System;
using SowingCalendar.Models;

namespace SowingCalendar.Repository
{
    /// <summary>
    ///     Interface of repository used for methods that access the database.
    /// </summary>
    public interface IRepository
    {
        #region Crop

        /// <summary>
        ///     Gets a crop based on the CropId primary key.
        /// </summary>
        /// <returns>The crop with the specified ID.</returns>
        /// <param name="id">Identifier.</param>
        Crop GetCrop(int id);

        #endregion
    }
}

