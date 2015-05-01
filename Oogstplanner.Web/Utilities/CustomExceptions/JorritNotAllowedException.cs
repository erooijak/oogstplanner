﻿using System;

namespace Oogstplanner.Utilities.CustomExceptions
{
    public class JorritNotAllowedException : Exception
    {
        public JorritNotAllowedException()
        {
        }
        public JorritNotAllowedException(string message) 
            : base(message)
        {
        }

        public JorritNotAllowedException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}