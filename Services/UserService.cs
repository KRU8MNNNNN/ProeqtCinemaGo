
using System;
using System.Linq;
using Proeqt.Models;
using Proeqt.Repositories;

namespace Proeqt.Services
{
    internal class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool RegisterUser(User user)
        {
            if (user == null ||
                string.IsNullOrWhiteSpace(user.Name) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.Password) ||
                !user.Email.Contains("@") ||
                user.Name.Contains("|") ||
                user.Email.Contains("|") ||
                user.Password.Contains("|"))
            {
                Console.WriteLine("Invalid user data!");
                return false;
            }

            var users = _userRepository.GetAllUsers();

            if (users.Any(u => u.Name.Equals(
                user.Name, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Username already exists!");
                return false;
            }

            if (users.Any(u => u.Email.Equals(
                user.Email, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Email already exists!");
                return false;
            }

            user.Id = users.Count == 0 ? 1 : users.Max(u => u.Id) + 1;

            _userRepository.SaveUser(user);

            Console.WriteLine("Registration successful!");
            return true;
        }

        public bool LoginUser(string username, string password)
        {
            var users = _userRepository.GetAllUsers();

            User user = users.FirstOrDefault(u =>
                u.Name.Equals(username, StringComparison.OrdinalIgnoreCase)
                && u.Password == password);

            if (user == null)
            {
                Console.WriteLine("Invalid username or password!");
                return false;
            }

            Console.WriteLine($"Welcome, {user.Name}!");
            return true;
        }
    }
}
