using CinemaGoProject.Models;
using CinemaGoProject.Services;
using Spectre.Console;
using System;
using System.Linq;
using System.Globalization;

namespace CinemaGoProject.Menu
{
    internal class AdminMenu
    {
        private readonly FilmService _filmService;
        private readonly SessionService _sessionService;
        private readonly BookingService _bookingService;
        private readonly StatisticsService _statisticsService;

        public AdminMenu(
            FilmService filmService,
            SessionService sessionService,
            BookingService bookingService,
            StatisticsService statisticsService)
        {
            _filmService = filmService;
            _sessionService = sessionService;
            _bookingService = bookingService;
            _statisticsService = statisticsService;
        }

        public void ShowAdminMenu()
        {
            string choice = "";

            Logger.Log("Admin opened the admin panel");

            while (choice != "0. Log Out")
            {
                choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("CinemaGo Admin Panel")
                    .AddChoices(
                        "1. Show Films",
                        "2. Add Film",
                        "3. Update Film",
                        "4. Delete Film",
                        "5. Show Sessions",
                        "6. Create Session",
                        "7. Cancel Session",
                        "8. Statistics",
                        "0. Log Out"));

                switch (choice)
                {
                    case "1. Show Films":
                        ShowFilmMenu();
                        break;
                    case "2. Add Film":
                        AddFilmMenu();
                        break;
                    case "3. Update Film":
                        UpdateFilmMenu();
                        break;
                    case "4. Delete Film":
                        DeleteFilmMenu();
                        break;
                    case "5. Show Sessions":
                        ShowSessionMenu();
                        break;
                    case "6. Create Session":
                        CreateSessionMenu();
                        break;
                    case "7. Cancel Session":
                        CancelSessionMenu();
                        break;
                    case "8. Statistics":
                        _statisticsService.ShowStatistics();
                        break;
                    case "0. Log Out":
                        Console.WriteLine("Logged out successfully.");
                        Logger.Log("Admin logged out");
                        break;
                }
            }
        }

        private void ShowFilmMenu()
        {
            var films = _filmService.GetAllFilms();
            Console.WriteLine("\nCinemaGo Films");

            if (films.Count == 0)
            {
                Console.WriteLine("No films available.");
                return;
            }

            foreach (var film in films)
                Console.WriteLine(film);
        }

        private void AddFilmMenu()
        {
            Film film = new Film();
            var films = _filmService.GetAllFilms();
            film.Id = films.Count == 0 ? 1 : films.Max(x => x.Id) + 1;

            Console.Write("Enter film name: ");
            film.Name = Console.ReadLine() ?? "";

            Console.Write("Enter film description: ");
            film.Description = Console.ReadLine() ?? "";

            Console.Write("Enter film genre: ");
            film.Genre = Console.ReadLine() ?? "";

            _filmService.AddFilm(film);
            Console.WriteLine("Film added successfully!");

            Logger.Log($"Film added | Film ID: {film.Id} | Name: {film.Name} | Genre: {film.Genre}");
        }

        private void UpdateFilmMenu()
        {
            Console.Write("Enter film ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Film film = _filmService.GetFilmById(id);

            if (film == null)
            {
                Console.WriteLine("Film not found.");
                return;
            }

            Console.Write("Enter new film name: ");
            film.Name = Console.ReadLine() ?? "";

            Console.Write("Enter new film description: ");
            film.Description = Console.ReadLine() ?? "";

            Console.Write("Enter new film genre: ");
            film.Genre = Console.ReadLine() ?? "";

            _filmService.UpdateFilm(film);
            Console.WriteLine("Film updated successfully!");

            Logger.Log($"Film updated | Film ID: {id} | Name: {film.Name} | Genre: {film.Genre}");
        }

        private void DeleteFilmMenu()
        {
            Console.Write("Enter film ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Film film = _filmService.GetFilmById(id);

            if (film == null)
            {
                Console.WriteLine("Film not found.");
                return;
            }

            _filmService.DeleteFilm(id);
            Console.WriteLine("Film deleted successfully!");

            Logger.Log($"Film deleted | Film ID: {id} | Name: {film.Name}");
        }

        private void ShowSessionMenu()
        {
            var sessions = _sessionService.GetAllSessions();

            if (sessions.Count == 0)
            {
                Console.WriteLine("No sessions available.");
                return;
            }

            foreach (var session in sessions)
            {
                Film film = _filmService.GetFilmById(session.FilmId);

                Console.WriteLine(
                    $"{session.Id}. {film?.Name ?? "Unknown"} | " +
                    $"{session.StartTime:dd/MM/yyyy HH:mm} | Hall: {session.Hall} | " +
                    $"Standard: {session.StandardPrice} | VIP: {session.VipPrice} | " +
                    $"{(session.IsCancelled ? "Cancelled" : "Active")}");
            }
        }

        private void CreateSessionMenu()
        {
            var films = _filmService.GetAllFilms();

            if (films.Count == 0)
            {
                Console.WriteLine("Add a film first.");
                return;
            }

            foreach (var film in films)
                Console.WriteLine($"{film.Id}. {film.Name}");

            Console.Write("Enter film ID: ");

            if (!int.TryParse(Console.ReadLine(), out int filmId) ||
                _filmService.GetFilmById(filmId) == null)
            {
                Console.WriteLine("Invalid film ID.");
                return;
            }

            Console.Write("Enter date and time (dd/MM/yyyy HH:mm): ");

            if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                Console.WriteLine("Invalid date.");
                return;
            }

            Console.Write("Enter hall name: ");
            string hall = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(hall))
            {
                Console.WriteLine("Invalid hall name.");
                return;
            }

            Console.Write("Enter Standard price: ");

            if (!decimal.TryParse(Console.ReadLine(), out decimal standardPrice) ||
                standardPrice <= 0)
            {
                Console.WriteLine("Invalid price.");
                return;
            }

            Console.Write("Enter VIP price: ");

            if (!decimal.TryParse(Console.ReadLine(), out decimal vipPrice) ||
                vipPrice <= 0)
            {
                Console.WriteLine("Invalid price.");
                return;
            }

            var sessions = _sessionService.GetAllSessions();

            Session session = new Session
            {
                Id = sessions.Count == 0 ? 1 : sessions.Max(x => x.Id) + 1,
                FilmId = filmId,
                StartTime = date,
                Hall = hall,
                StandardPrice = standardPrice,
                VipPrice = vipPrice,
                IsCancelled = false
            };

            _sessionService.AddSession(session);
            Console.WriteLine("Session created successfully!");

            Logger.Log($"Session created | Session ID: {session.Id} | Film ID: {filmId}");
        }

        private void CancelSessionMenu()
        {
            ShowSessionMenu();

            Console.Write("Enter session ID to cancel: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Session session = _sessionService.GetSessionById(id);

            if (session == null || session.IsCancelled)
            {
                Console.WriteLine("Active session not found.");
                return;
            }

            int cancelledBookings = _bookingService.CancelBookingsForSession(id);

            _sessionService.CancelSession(id);

            Console.WriteLine("Session cancelled successfully!");
            Console.WriteLine($"{cancelledBookings} booking(s) cancelled and seats released.");

            Logger.Log($"Session cancelled | Session ID: {id} | Bookings cancelled: {cancelledBookings}");
        }
    }
}