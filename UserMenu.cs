
using CinemaGoProject.Models;
using CinemaGoProject.Services;
using CinemaGoProject.Exceptions;    
using CinemaGoProject.Models;
using CinemaGoProject.Services;
using CinemaGoProject;
using System;

namespace CinemaGoProject.Menu
{
    internal class UserMenu
    {
        private readonly UserService _userService;

        public UserMenu(UserService userService)
        {
            _userService = userService;
        }

        public void ShowMainMenu()
        {
            string choice = "";

            while (choice != "0")
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to the CinemaGo User Management System");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("0. Exit");
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

            try
            {
                user.Email = Console.ReadLine() ?? "";
            }
            catch (InvalidEmailException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Console.Write("Enter password: ");
            user.Password = Console.ReadLine() ?? "";

            bool registered = _userService.RegisterUser(user);

            if (registered)
            {
                Console.Write("Enter verification code sent to your email: ");
                string code = Console.ReadLine() ?? "";

                if (_userService.VerifyUser(user.Email, code))
                {
                    Console.WriteLine("Your account has been verified!");
                }
            }
        }

        private void ShowLoginMenu()
        {
            Console.Write("Enter email: ");
            string email = Console.ReadLine() ?? "";

            Console.Write("Enter password: ");
            string password = Console.ReadLine() ?? "";

            User user = _userService.LoginUser(email, password);

            if (user != null)
            {
                CinemaGoMenu cinemaGoMenu = new CinemaGoMenu(user.Name);
                cinemaGoMenu.ShowMenu();
            }
            else
            {
                Console.WriteLine("Invalid email or password, or account not verified.");
            }
        }
    }
}
