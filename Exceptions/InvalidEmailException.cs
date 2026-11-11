
using System;

namespace CinemaGoProject.Exceptions
{
    internal class InvalidEmailException : Exception
    {
        public InvalidEmailException()
        {
        }

        public InvalidEmailException(string message) : base(message)
        {
        }
    }
}
