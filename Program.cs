
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proeqt.Repositories;
using Proeqt.Services;

namespace Proeqt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserRepository userRepository = new UserRepository();
            UserService userService = new UserService(userRepository);
            UserMenu userMenu = new UserMenu(userService);

            while (true)
            {
                bool loggedIn = userMenu.ShowMainMenu();

                if (!loggedIn)
                {
                    break;
                }

                CinemaGoMenu cinemaGoMenu =
                    new CinemaGoMenu(userMenu.LoggedInUsername);

                cinemaGoMenu.ShowMenu();
            }

            Console.WriteLine("Goodbye!");
            Console.ReadLine();
        }
    }
}
