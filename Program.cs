using CinemaGoProject.Menu;
using CinemaGoProject.Repositories;
using CinemaGoProject.Services;
using Microsoft.Extensions.Configuration;
using System;

namespace CinemaGoProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            UserRepository userRepository = new UserRepository();
            EmailService emailService = new EmailService(configuration);
            UserService userService = new UserService(userRepository, emailService);

            FilmRepository filmRepository = new FilmRepository();
            FilmService filmService = new FilmService(filmRepository);
            ClientMenu clientMenu = new ClientMenu(filmService);

            UserMenu userMenu = new UserMenu(userService, clientMenu);
            userMenu.ShowMainMenu();
        }
    }
}
