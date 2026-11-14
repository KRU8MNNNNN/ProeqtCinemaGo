using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaGoProject.Exceptions
{
    internal class InvalidEmailException : Exception
    {
        public InvalidEmailException() : base("Invalid email address.")
        {
        }


        public InvalidEmailException(string message) : base(message)
        {
        }
    }
}