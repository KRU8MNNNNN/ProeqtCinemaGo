using CinemaGoProject.Models;
using CinemaGoProject.Services;
using System;

namespace CinemaGoProject.Menu
{
    internal class UserMenu
    {
        private readonly UserService _userService;
        private readonly ClientMenu _clientMenu;
        private readonly AdminMenu _adminMenu;

        public UserMenu(
            UserService userService,
            ClientMenu clientMenu,
            AdminMenu adminMenu)
        {
            _userService = userService;
            _clientMenu = clientMenu;
            _adminMenu = adminMenu;
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

        private string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            while (true)
            {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Substring(0, password.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            }

            return password;
        }

        private void ShowRegisterMenu()
        {
            User user = new User();

            try
            {
                Console.Write("Enter username: ");
                user.Name = Console.ReadLine() ?? "";

                Console.Write("Enter email: ");
                user.Email = Console.ReadLine() ?? "";

                Console.Write("Enter password: ");
                user.Password = ReadPassword();

                _userService.RegisterUser(user);

                Logger.Log("User registered");

                Console.Write("Enter verification code sent to your email: ");
                string code = Console.ReadLine() ?? "";

                _userService.VerifyUser(user.Email, code);

                Logger.Log("User email verified");

                Console.WriteLine("Your account has been verified!");

                User loggedInUser = _userService.LoginUser(
                    user.Email, user.Password);

                Logger.Log($"User logged in | User ID: {loggedInUser.Id}");

                OpenUserMenu(loggedInUser);
            }
            catch (Exception ex)
            {
                Logger.Log($"Registration or verification failed | Error: {ex.Message}");
                Console.WriteLine(ex.Message);
            }
        }

        private void ShowLoginMenu()
        {
            Console.Write("Enter email: ");
            string email = Console.ReadLine() ?? "";

            Console.Write("Enter password: ");
            string password = ReadPassword();

            try
            {
                User loggedInUser = _userService.LoginUser(email, password);

                Logger.Log($"User logged in | User ID: {loggedInUser.Id}");

                OpenUserMenu(loggedInUser);
            }
            catch (Exception ex)
            {
                Logger.Log($"Login failed | Error: {ex.Message}");
                Console.WriteLine(ex.Message);
            }
        }

        private void OpenUserMenu(User user)
        {
            if (user.Role.Equals("Admin",
                StringComparison.OrdinalIgnoreCase))
            {
                _adminMenu.ShowAdminMenu();
            }
            else
            {
                _clientMenu.ShowMainFilmMenu(user);
                Logger.Log($"User logged out | User ID: {user.Id}");
            }
        }
    }
}