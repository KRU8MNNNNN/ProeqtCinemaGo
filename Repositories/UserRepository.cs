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
            Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
                .Parent.Parent.FullName,
            "Data",
            "users.txt");

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

                try
                {
                    User user;

                    if (line.Length == 7)
                    {
                        user = new Customer();

                        user.Id = int.Parse(line[0]);
                        user.Name = line[1];
                        user.Email = line[2];
                        user.Password = line[3];
                        user.IsLogged = bool.Parse(line[4]);
                        user.VerificationCode = line[5];
                        user.IsVerified = bool.Parse(line[6]);
                    }
                    else if (line.Length == 8)
                    {
                        if (line[4].Equals("Admin",
                            StringComparison.OrdinalIgnoreCase))
                        {
                            user = new Admin();
                        }
                        else
                        {
                            user = new Customer();
                        }

                        user.Id = int.Parse(line[0]);
                        user.Name = line[1];
                        user.Email = line[2];
                        user.Password = line[3];
                        user.Role = line[4];
                        user.IsLogged = bool.Parse(line[5]);
                        user.VerificationCode = line[6];
                        user.IsVerified = bool.Parse(line[7]);
                    }
                    else
                    {
                        continue;
                    }

                    users.Add(user);
                }
                catch
                {
                    continue;
                }
            }

            return users;
        }

        public void ChangeFile(List<string> usersInString)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
            File.WriteAllLines(_filePath, usersInString);
        }

        public User GetUserByEmail(string email)
        {
            var users = GetAllUsers();

            return users.FirstOrDefault(
                x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
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