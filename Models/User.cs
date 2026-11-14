
using CinemaGoProject.Exceptions;

namespace CinemaGoProject.Models
{
    internal class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        private string _email = "";

        public string Email
        {
            get { return _email; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                {
                    throw new InvalidEmailException("Invalid email address.");
                }

                _email = value;
            }
        }

        public string Password { get; set; } = "";

        public bool IsLogged { get; set; } = false;

        public string VerificationCode { get; set; } = "";

        public bool IsVerified { get; set; } = false;

        public override string ToString()
        {
            return $"{Id}|{Name}|{Email}|{Password}|{IsLogged}|{VerificationCode}|{IsVerified}";
        }
    }
}
