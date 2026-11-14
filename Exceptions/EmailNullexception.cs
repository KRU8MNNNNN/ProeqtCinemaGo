using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaGoProject.Exceptions
{
    internal class EmailNullexception : Exception
    {
        public EmailNullexception() : base("Email cannot be null.")
        {
        }

        public EmailNullexception(string message) : base(message)
        {
        }
    }
}