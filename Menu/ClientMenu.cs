using CinemaGoProject.Models;
using CinemaGoProject.Services;
using System;

namespace CinemaGoProject.Menu
{
    internal class ClientMenu
    {
        private readonly FilmService _filmService;
        private readonly SessionService _sessionService;
        private readonly BookingService _bookingService;

        public ClientMenu(
            FilmService filmService,
            SessionService sessionService,
            BookingService bookingService)
        {
            _filmService = filmService;
            _sessionService = sessionService;
            _bookingService = bookingService;
        }

        public void ShowMainFilmMenu(User user)
        {
            string choice = "";

            Logger.Log($"Customer opened menu | User ID: {user.Id}");

            while (choice != "0")
            {
                Console.WriteLine("\nWelcome to CinemaGo");
                Console.WriteLine("1. Show Films");
                Console.WriteLine("2. Show Sessions");
                Console.WriteLine("3. Book a Seat");
                Console.WriteLine("4. My Bookings");
                Console.WriteLine("5. Cancel Booking");
                Console.WriteLine("0. Log Out");
                Console.Write("Choose: ");

                choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        ShowFilmMenu();
                        break;

                    case "2":
                        ShowSessions();
                        break;

                    case "3":
                        BookSeat(user);
                        break;

                    case "4":
                        ShowBookings(user);
                        break;

                    case "5":
                        CancelBooking(user);
                        break;

                    case "0":
                        Console.WriteLine("Logged out successfully.");
                        Logger.Log($"Customer logged out | User ID: {user.Id}");
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        private void ShowFilmMenu()
        {
            string choice = "";

            while (choice != "0")
            {
                Console.WriteLine("\nCinemaGo Films");
                Console.WriteLine("1. Show All Films");
                Console.WriteLine("2. Filter Films by Genre");
                Console.WriteLine("0. Back");
                Console.Write("Choose: ");

                choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        var films = _filmService.GetAllFilms();

                        if (films.Count == 0)
                        {
                            Console.WriteLine("No films available.");
                            break;
                        }

                        foreach (var film in films)
                        {
                            Console.WriteLine(
                                $"{film.Id}. {film.Name} | Genre: {film.Genre} | Description: {film.Description}");
                        }
                        break;

                    case "2":
                        FilterFilmsByGenre();
                        break;

                    case "0":
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        private void FilterFilmsByGenre()
        {
            var genres = _filmService.GetGenres();

            if (genres.Count == 0)
            {
                Console.WriteLine("No genres available.");
                return;
            }

            Console.WriteLine("\nAvailable Genres:");

            for (int i = 0; i < genres.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {genres[i]}");
            }

            Console.Write("Choose genre number: ");

            if (!int.TryParse(Console.ReadLine(), out int choice) ||
                choice < 1 || choice > genres.Count)
            {
                Console.WriteLine("Invalid genre.");
                return;
            }

            string selectedGenre = genres[choice - 1];

            var films = _filmService.GetFilmsByGenre(selectedGenre);

            Console.WriteLine($"\n{selectedGenre} Films");

            foreach (var film in films)
            {
                Console.WriteLine(
                    $"{film.Id}. {film.Name} | Genre: {film.Genre} | Description: {film.Description}");
            }
        }

        private void ShowSessions()
        {
            var sessions = _sessionService.GetAllSessions();
            var films = _filmService.GetAllFilms();

            Console.WriteLine("\nCinemaGo Sessions");

            if (sessions.Count == 0)
            {
                Console.WriteLine("No sessions available.");
                return;
            }

            foreach (var session in sessions)
            {
                if (session.IsCancelled)
                {
                    continue;
                }

                var film = films.Find(x => x.Id == session.FilmId);
                string filmName = film == null ? "Unknown film" : film.Name;

                Console.WriteLine(
                    $"ID: {session.Id} | Film: {filmName} | " +
                    $"Date: {session.StartTime} | Hall: {session.Hall} | " +
                    $"Standard: {session.StandardPrice} | VIP: {session.VipPrice}");
            }
        }

        private void BookSeat(User user)
        {
            try
            {
                ShowSessions();

                Console.Write("\nEnter session ID: ");
                if (!int.TryParse(Console.ReadLine(), out int sessionId))
                {
                    Console.WriteLine("Invalid session ID.");
                    Logger.Log($"Booking failed | User ID: {user.Id} | Invalid session ID");
                    return;
                }

                Hall hall = _sessionService.GetHall(sessionId);

                if (hall == null)
                {
                    Console.WriteLine("Hall not found.");
                    Logger.Log($"Booking failed | User ID: {user.Id} | Session ID: {sessionId} | Hall not found");
                    return;
                }

                hall.DisplaySeatMap();

                Console.Write("Ticket type (Standard/VIP): ");
                string ticketType = Console.ReadLine() ?? "";

                if (!ticketType.Equals("Standard", StringComparison.OrdinalIgnoreCase) &&
                    !ticketType.Equals("VIP", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Invalid ticket type.");
                    Logger.Log($"Booking failed | User ID: {user.Id} | Invalid ticket type: {ticketType}");
                    return;
                }

                Console.Write("Enter seat row: ");
                string rowInput = Console.ReadLine() ?? "";

                if (rowInput.Length != 1)
                {
                    Console.WriteLine("Invalid row.");
                    Logger.Log($"Booking failed | User ID: {user.Id} | Invalid seat row");
                    return;
                }

                char row = char.ToUpper(rowInput[0]);

                Console.Write("Enter seat number: ");
                if (!int.TryParse(Console.ReadLine(), out int seatNumber))
                {
                    Console.WriteLine("Invalid seat number.");
                    Logger.Log($"Booking failed | User ID: {user.Id} | Invalid seat number");
                    return;
                }

                Booking booking = _bookingService.CreateBooking(
                    user, sessionId, row, seatNumber, ticketType);

                Console.WriteLine("\nBooking successful!");
                Console.WriteLine("Ticket code: " + booking.TicketCode);
                Console.WriteLine("Seat: " + booking.Row + booking.SeatNumber);
                Console.WriteLine("Price: " + booking.Price);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Logger.Log($"Booking failed | User ID: {user.Id} | Error: {ex.Message}");
            }
        }

        private void ShowBookings(User user)
        {
            var bookings = _bookingService.GetUserBookings(user.Id);

            Console.WriteLine("\nMy Bookings");

            if (bookings.Count == 0)
            {
                Console.WriteLine("You have no active bookings.");
                return;
            }

            foreach (var booking in bookings)
            {
                Console.WriteLine(
                    $"Booking ID: {booking.Id} | Session: {booking.SessionId} | " +
                    $"Seat: {booking.Row}{booking.SeatNumber} | " +
                    $"Type: {booking.TicketType} | Price: {booking.Price} | " +
                    $"Ticket: {booking.TicketCode}");
            }
        }

        private void CancelBooking(User user)
        {
            try
            {
                ShowBookings(user);

                Console.Write("\nEnter booking ID to cancel: ");

                if (!int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    Console.WriteLine("Invalid booking ID.");
                    Logger.Log($"Booking cancellation failed | User ID: {user.Id} | Invalid booking ID");
                    return;
                }

                _bookingService.CancelBooking(user.Id, bookingId);

                Console.WriteLine("Booking cancelled successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Logger.Log($"Booking cancellation failed | User ID: {user.Id} | Error: {ex.Message}");
            }
        }
    }
}