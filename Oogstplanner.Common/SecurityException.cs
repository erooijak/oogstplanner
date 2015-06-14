using System;

namespace Oogstplanner.Common
{
    public class UserNotFoundException : Exception
    {
        /// <summary>
        /// A <see cref="UserNotFoundException" /> exception is thrown when a user is not found
        /// </summary>
        public UserNotFoundException()
        { }

        public UserNotFoundException(string message) 
            : base(message)
        { }

        public UserNotFoundException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
