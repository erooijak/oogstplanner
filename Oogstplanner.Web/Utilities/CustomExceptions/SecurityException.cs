using System;

namespace Oogstplanner.Utilities.CustomExceptions
    {
    public class SecurityException : Exception
    {
        /// <summary>
        /// A SecurityException exception is thrown when a caller does not have the permissions required to access a resource.
        /// </summary>
        public SecurityException()
        {
        }
        public SecurityException(string message) 
            : base(message)
        {
        }

        public SecurityException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}
