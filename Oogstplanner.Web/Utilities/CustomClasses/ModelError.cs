namespace Oogstplanner.Utilities.CustomClasses
{
    /// <summary>
    ///     Class used to put in AddModelError.
    /// </summary>
    /// <remarks>
    ///     Used to show an error message on the view and highlight the field.
    /// </remarks>
    public class ModelError
    {
        /// <summary>
        ///     The the name of the property which caused the error.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        ///     The error message.
        /// </summary>
        public string Message { get; set; }
    }
}
    