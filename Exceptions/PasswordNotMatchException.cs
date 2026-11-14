using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaGoProject.Exceptions
{
    internal class PasswordNotMatchException : Exception
    {
        public PasswordNotMatchException() : base("Password does not match")
        {
        }

        public PasswordNotMatchException(string message) : base(message)
        {
        }
    }
}