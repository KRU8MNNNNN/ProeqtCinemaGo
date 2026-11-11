
using CinemaGoProject.Models;
using CinemaGoProject.Repositories;
using CinemaGoProject.Models;
using CinemaGoProject.Repositories;
using System;
using System.Linq;

namespace CinemaGoProject.Services
{
    internal class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly IEmailService _emailService;

        public UserService(UserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public bool RegisterUser(User user)
        {
            if (user == null)
            {
                Console.WriteLine("User cannot be null.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.Name) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.Password))
            {
                Console.WriteLine("All fields are required.");
                return false;
            }

            var users = _userRepository.GetAllUsers();

            if (users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("This email is already registered.");
                return false;
            }

            int maxId = 0;

            if (users.Count != 0)
            {
                maxId = users.Max(u => u.Id);
            }

            user.Id = maxId + 1;
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            Random random = new Random();
            user.VerificationCode = random.Next(1000, 10000).ToString();
            user.IsVerified = false;

            _userRepository.SaveUser(user);

            _emailService.SendEmail(
                user.Email,
                "CinemaGo Email Verification",
                $@"
                <html>
                <body>
                    <h2>Welcome to CinemaGo!</h2>
                    <p>Thank you for registering.</p>
                    <p>Your verification code is:</p>
                    <h1>{user.VerificationCode}</h1>
                    <p>Please enter this code in the application to verify your account.</p>
                </body>
                </html>");

            Console.WriteLine("Registration successful. Check your email for the verification code.");

            return true;
        }

        public bool VerifyUser(string email, string verificationCode)
        {
            return _userRepository.VerifyUser(email, verificationCode);
        }

        public User LoginUser(string email, string password)
        {
            var users = _userRepository.GetAllUsers();

            User user = users.FirstOrDefault(
                u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                return null;
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }

            if (!user.IsVerified)
            {
                Console.WriteLine("Please verify your email first.");
                return null;
            }

            user.IsLogged = true;
            _userRepository.UpdateUser(user);

            return user;
        }
    }
}
