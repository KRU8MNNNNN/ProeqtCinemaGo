
using Proeqt.Models;
using Proeqt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proeqt
{
    internal class UserMenu
    {
        private readonly UserService _userService;

        public string LoggedInUsername { get; private set; } = "";

        public UserMenu(UserService userService)
        {
            _userService = userService;
        }

        public bool ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("CinemaGo");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("0. Exit");
                Console.Write("Choose: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        if (Register())
                        {
                            return true;
                        }
                        break;

                    case "2":
                        if (Login())
                        {
                            return true;
                        }
                        break;

                    case "0":
                        return false;

                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
        }

        private bool Register()
        {
            User user = new User();

            Console.Write("Enter username: ");
            user.Name = Console.ReadLine() ?? "";

            Console.Write("Enter email: ");
            user.Email = Console.ReadLine() ?? "";

            Console.Write("Enter password: ");
            user.Password = Console.ReadLine() ?? "";

            if (_userService.RegisterUser(user))
            {
                LoggedInUsername = user.Name;
                return true;
            }

            return false;
        }

        private bool Login()
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Enter password: ");
            string password = Console.ReadLine() ?? "";

            if (_userService.LoginUser(username, password))
            {
                LoggedInUsername = username;
                return true;
            }

            return false;
        }
    }
}
