using CinemaGoProject.Exceptions;
using CinemaGoProject.Repositories;
using CinemaGoProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaGoProject.Services
{
    internal class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly EmailService _emailService;

        public UserService(UserRepository userRepository,
            EmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public void RegisterUser(User user)
        {
            if (user == null)
            {
                Console.WriteLine("User cannot be null.");
                return;
            }

            var users = _userRepository.GetAllUsers();

            if (users.Any(x => x.Email == user.Email))
            {
                throw new UsedEmailException();
            }

            int maxId = users.Count > 0
                ? users.Max(u => u.Id)
                : 0;

            user.Id = ++maxId;

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            Random random = new Random();
            var randomCode = random.Next(1000, 10000);

            user.VerificationCode = randomCode.ToString();

            string welcomeHtmlMessage = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
</head>
<body style='margin:0; padding:30px; background-color:#f4f4f7;
             font-family:Arial,sans-serif;'>

    <div style='max-width:600px; margin:auto; background:white;
                border-radius:10px; padding:30px;'>

        <h1 style='text-align:center; color:#e11d48;'>
            CinemaGo
        </h1>

        <h2 style='color:#333;'>
            Welcome to CinemaGo!
        </h2>

        <p style='font-size:16px; color:#555;'>
            Thank you for joining CinemaGo!
            Your next movie experience is just around the corner.
            To activate your account, use the verification code below:
        </p>

        <div style='text-align:center; margin:35px 0;'>
            <span style='background:#fff1f2;
                         color:#e11d48;
                         padding:15px 35px;
                         font-size:32px;
                         font-weight:bold;
                         letter-spacing:8px;
                         border-radius:8px;'>
                {user.VerificationCode}
            </span>
        </div>

        <p style='color:#777; font-size:14px;'>
            Enter this code in the CinemaGo application
            to verify your account and start exploring movies.
        </p>

        <p style='color:#999; font-size:12px; text-align:center;'>
            If you did not register for CinemaGo,
            please ignore this email.
        </p>

        <hr style='border:none; border-top:1px solid #eee;'>

        <p style='text-align:center; color:#aaa; font-size:12px;'>
            © 2026 CinemaGo. All rights reserved.
        </p>

    </div>
</body>
</html>";

            _emailService.SendEmail(
                user.Email,
                "CinemaGo - Verification Code",
                welcomeHtmlMessage
            );

            _userRepository.SaveUser(user);
        }

        public void VerifyUser(string email, string verificationCode)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new EmailNullexception();
            }

            if (string.IsNullOrWhiteSpace(verificationCode))
            {
                throw new VerificationCodeNullException();
            }

            _userRepository.VerifyUser(email, verificationCode);
        }

        public void LoginUser(string email, string passwiord)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new EmailNullexception();
            }

            if (string.IsNullOrWhiteSpace(passwiord))
            {
                throw new PasswordNullexception();
            }

            var userFromFile = _userRepository.GetUserByEmail(email);

            if (userFromFile != null &&
                BCrypt.Net.BCrypt.Verify(passwiord, userFromFile.Password) &&
                userFromFile.IsVerified)
            {
                Console.WriteLine("Logged In successfully");

                var users = _userRepository.GetAllUsers();

                List<string> userInStrings = new List<string>();

                foreach (var item in users)
                {
                    if (item.Email == email)
                    {
                        item.IsLogged = true;
                    }

                    userInStrings.Add(item.ToString());
                }

                _userRepository.ChangeFile(userInStrings);
            }
            else
            {
                throw new PasswordNotMatchException();
            }
        }
    }
}
