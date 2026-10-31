
using Proeqt.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proeqt.Repositories
{
    internal class UserRepository
    {
        private readonly string _filePath =
            @"C:\Users\595-424-424 Temo\source\repos\Proeqt\Data\users.txt";

        public UserRepository()
        {
            Directory.CreateDirectory(
                Path.GetDirectoryName(_filePath));

            List<string> oldFiles = new List<string>
            {
                Path.Combine(Directory.GetCurrentDirectory(), "Data", "users.txt"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "users.txt"),
                _filePath
            };

            List<User> users = new List<User>();

            foreach (string file in oldFiles.Distinct(
                StringComparer.OrdinalIgnoreCase))
            {
                if (!File.Exists(file))
                {
                    continue;
                }

                foreach (string line in File.ReadAllLines(file))
                {
                    string[] elements = line.Split('|');

                    if (elements.Length != 4 ||
                        !int.TryParse(elements[0], out int id))
                    {
                        continue;
                    }

                    if (users.Any(u =>
                        u.Name.Equals(elements[1],
                            StringComparison.OrdinalIgnoreCase) ||
                        u.Email.Equals(elements[2],
                            StringComparison.OrdinalIgnoreCase)))
                    {
                        continue;
                    }

                    users.Add(new User
                    {
                        Id = id,
                        Name = elements[1],
                        Email = elements[2],
                        Password = elements[3]
                    });
                }
            }

            for (int i = 0; i < users.Count; i++)
            {
                users[i].Id = i + 1;
            }

            File.WriteAllLines(_filePath, users.Select(u =>
                $"{u.Id}|{u.Name}|{u.Email}|{u.Password}"));

            Console.WriteLine("Accounts migrated: " + users.Count);
            Console.WriteLine("Users file: " + _filePath);
        }

        public void SaveUser(User user)
        {
            File.AppendAllLines(_filePath, new[]
            {
                $"{user.Id}|{user.Name}|{user.Email}|{user.Password}"
            });
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            foreach (string line in File.ReadAllLines(_filePath))
            {
                string[] elements = line.Split('|');

                if (elements.Length != 4 ||
                    !int.TryParse(elements[0], out int id))
                {
                    continue;
                }

                users.Add(new User
                {
                    Id = id,
                    Name = elements[1],
                    Email = elements[2],
                    Password = elements[3]
                });
            }

            return users;
        }
    }
}
