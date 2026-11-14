
using CinemaGoProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CinemaGoProject.Repositories
{
    internal class UserRepository
    {
        private readonly string _filePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Data", "user.txt");

        public void SaveUser(User user)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
            File.AppendAllLines(_filePath, new[] { user.ToString() });
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            if (!File.Exists(_filePath))
            {
                return users;
            }

            var lines = File.ReadAllLines(_filePath);

            foreach (var item in lines)
            {
                var line = item.Split('|');

                if (line.Length != 7)
                {
                    continue;
                }

                User user = new User();

                user.Id = int.Parse(line[0]);
                user.Name = line[1];
                user.Email = line[2];
                user.Password = line[3];
                user.IsLogged = bool.Parse(line[4]);
                user.VerificationCode = line[5];
                user.IsVerified = bool.Parse(line[6]);

                users.Add(user);
            }

            return users;
        }

        public void ChangeFile(List<string> usersInString)
        {
            File.WriteAllText(_filePath, "");
            File.AppendAllLines(_filePath, usersInString);
        }

        public User GetUserByEmail(string email)
        {
            var users = GetAllUsers();

            var user = users.FirstOrDefault(x => x.Email == email);

            return user;
        }

        public bool VerifyUser(string email, string verificationCode)
        {
            var users = GetAllUsers();

            User user = users.FirstOrDefault(
                x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                Console.WriteLine("User not found.");
                return false;
            }

            if (user.VerificationCode != verificationCode)
            {
                Console.WriteLine("Verification code is not correct.");
                return false;
            }

            user.IsVerified = true;
            user.VerificationCode = "";

            SaveAllUsers(users);

            Console.WriteLine("User verified successfully.");

            return true;
        }

        public void UpdateUser(User user)
        {
            var users = GetAllUsers();

            User existingUser = users.FirstOrDefault(x => x.Id == user.Id);

            if (existingUser == null)
            {
                return;
            }

            existingUser.IsLogged = user.IsLogged;

            SaveAllUsers(users);
        }

        private void SaveAllUsers(List<User> users)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));

            string[] userText = new string[users.Count];

            for (int i = 0; i < users.Count; i++)
            {
                userText[i] = users[i].ToString();
            }

            File.WriteAllLines(_filePath, userText);
        }
    }
}
