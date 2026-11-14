using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaGoProject.Exceptions
{
    internal class UsedEmailException : Exception
    {
        public UsedEmailException() : base("Email is already registered")
        {
        }

        public UsedEmailException(string message) : base(message)
        {
        }
    }
}