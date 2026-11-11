
using CinemaGoProject.Menu;
using CinemaGoProject.Repositories;
using CinemaGoProject.Services;
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

            UserMenu userMenu = new UserMenu(userService);
            userMenu.ShowMainMenu();
        }
    }
}

