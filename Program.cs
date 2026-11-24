using CinemaGoProject.Menu;
using CinemaGoProject.Models;
using CinemaGoProject.Repositories;
using CinemaGoProject.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

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

            CreateAdminAccount(userRepository);

            EmailService emailService = new EmailService(configuration);

            UserService userService = new UserService(
                userRepository, emailService);

            FilmRepository filmRepository = new FilmRepository();
            FilmService filmService = new FilmService(filmRepository);

            SessionRepository sessionRepository = new SessionRepository();
            SeatRepository seatRepository = new SeatRepository();

            SessionService sessionService = new SessionService(
                sessionRepository, seatRepository);

            BookingRepository bookingRepository = new BookingRepository();

            BookingService bookingService = new BookingService(
                bookingRepository,
                sessionService,
                emailService,
                userRepository);

            StatisticsService statisticsService = new StatisticsService(
                bookingRepository,
                sessionService,
                filmService);

            ClientMenu clientMenu = new ClientMenu(
                filmService, sessionService, bookingService);

            AdminMenu adminMenu = new AdminMenu(
                filmService,
                sessionService,
                bookingService,
                statisticsService);

            UserMenu userMenu = new UserMenu(
                userService, clientMenu, adminMenu);

            userMenu.ShowMainMenu();
        }

        private static void CreateAdminAccount(UserRepository userRepository)
        {
            var users = userRepository.GetAllUsers();

            if (users.Any(x =>
                x.Email.Equals("admin@cinemago.ge", StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            User admin = new User
            {
                Id = users.Count > 0 ? users.Max(x => x.Id) + 1 : 1,
                Name = "cinema_admin",
                Email = "admin@cinemago.ge",
                Password = BCrypt.Net.BCrypt.HashPassword("adminPass"),
                Role = "Admin",
                IsLogged = false,
                VerificationCode = "",
                IsVerified = true
            };

            userRepository.SaveUser(admin);
        }
    }
}