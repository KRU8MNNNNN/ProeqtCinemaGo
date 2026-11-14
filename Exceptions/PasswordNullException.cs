using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaGoProject.Exceptions
{
    internal class PasswordNullexception : Exception
    {
        public PasswordNullexception() : base("Email cannot be null.")
        {
        }

        public PasswordNullexception(string message) : base(message)
        {
        }
    }
}