using System;

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

public class GergelyNotAllowedException : Exception
{
    public GergelyNotAllowedException()
    {
    }
    public GergelyNotAllowedException(string message) 
        : base(message)
    {
    }

    public GergelyNotAllowedException(string message, Exception inner)
        : base(message, inner)
    {
    }

}