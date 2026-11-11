
using System;

namespace CinemaGoProject.Menu
{
    internal class CinemaGoMenu
    {
        private readonly string _username;

        public CinemaGoMenu(string username)
        {
            _username = username;
        }

        public void ShowMenu()
        {
            string choose = "";

            while (choose != "0")
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to CinemaGo");
                Console.WriteLine("Logged in as: " + _username);
                Console.WriteLine("1. View account");
                Console.WriteLine("0. Log out");
                Console.Write("Choose: ");

                choose = Console.ReadLine() ?? "";

                switch (choose)
                {
                    case "1":
                        Console.WriteLine("Username: " + _username);
                        break;

                    case "0":
                        Console.WriteLine("Logged out successfully!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
        }
    }
}
