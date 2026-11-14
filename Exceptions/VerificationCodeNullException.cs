using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaGoProject.Exceptions
{
    internal class VerificationCodeNullException : Exception
    {
        public VerificationCodeNullException() : base("Verification code cannot be null.")
        {
        }
        public VerificationCodeNullException(string message) : base(message)
        {
        }

    }
}