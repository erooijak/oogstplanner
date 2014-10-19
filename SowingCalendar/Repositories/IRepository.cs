using System;
using SowingCalendar.Models;

namespace SowingCalendar.Repositories
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

        /// <summary>
        ///     Gets a crop based on the name.
        /// </summary>
        /// <returns>The crop with the specified name.</returns>
        /// <param name="name">Name of the crop.</param>
        Crop GetCrop(string name);

        #endregion
    }
}

