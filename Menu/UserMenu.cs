using CinemaGoProject.Models;
using CinemaGoProject.Services;
using CinemaGoProject.Exceptions;
using System;

namespace CinemaGoProject.Menu
{
    internal class UserMenu
    {
        private readonly UserService _userService;
        private readonly ClientMenu _clientMenu;

        public UserMenu(UserService userService, ClientMenu clientMenu)
        {
            _userService = userService;
            _clientMenu = clientMenu;
        }

        public void ShowMainMenu()
        {
            string choice = "";

            while (choice != "0")
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to CinemaGo");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("0. Exit");s
                Console.Write("Choose: ");

                choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        ShowRegisterMenu();
                        break;

                    case "2":
                        ShowLoginMenu();
                        break;

                    case "0":
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void ShowRegisterMenu()
        {
            User user = new User();

            Console.Write("Enter username: ");
            user.Name = Console.ReadLine() ?? "";

            Console.Write("Enter email: ");
            user.Email = Console.ReadLine() ?? "";

            Console.Write("Enter password: ");
            user.Password = Console.ReadLine() ?? "";

            try
            {
                _userService.RegisterUser(user);

                Console.Write("Enter verification code sent to your email: ");
                string code = Console.ReadLine() ?? "";

                _userService.VerifyUser(user.Email, code);

                Console.WriteLine("Your account has been verified!");

                _userService.LoginUser(user.Email, user.Password);

                _clientMenu.ShowMainFilmMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ShowLoginMenu()
        {
            Console.Write("Enter email: ");
            string email = Console.ReadLine() ?? "";

            Console.Write("Enter password: ");
            string password = Console.ReadLine() ?? "";

            try
            {
                _userService.LoginUser(email, password);
                _clientMenu.ShowMainFilmMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
