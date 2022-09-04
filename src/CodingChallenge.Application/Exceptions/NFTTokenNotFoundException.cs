using System;

namespace CodingChallenge.Application.Exceptions;
public class NFTTokenNotFoundException : Exception
{
    public NFTTokenNotFoundException(string message)
        : base(message)
    {
    }
}
public class TVMazeItemAlreadyExistsException : Exception
{
    public TVMazeItemAlreadyExistsException(string message)
        : base(message)
    {
    }
}
