using CinemaGoProject.Models;
using CinemaGoProject.Repositories;
using System;
using System.Linq;

namespace CinemaGoProject.Services
{
    internal class StatisticsService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly SessionService _sessionService;
        private readonly FilmService _filmService;

        public StatisticsService(
            BookingRepository bookingRepository,
            SessionService sessionService,
            FilmService filmService)
        {
            _bookingRepository = bookingRepository;
            _sessionService = sessionService;
            _filmService = filmService;
        }

        public void ShowStatistics()
        {
            var bookings = _bookingRepository.GetAllBookings();
            var sessions = _sessionService.GetAllSessions();
            var films = _filmService.GetAllFilms();

            var activeBookings = bookings
                .Where(x => !x.IsCancelled)
                .ToList();

            var cancelledBookings = bookings
                .Where(x => x.IsCancelled)
                .ToList();

            decimal totalRevenue = activeBookings.Sum(x => x.Price);

            var filmStatistics =
                from booking in activeBookings
                join session in sessions
                    on booking.SessionId equals session.Id
                join film in films
                    on session.FilmId equals film.Id
                group booking by film.Name into filmGroup
                select new
                {
                    FilmName = filmGroup.Key,
                    BookingCount = filmGroup.Count(),
                    Revenue = filmGroup.Sum(x => x.Price)
                };

            var mostPopularFilm = filmStatistics
                .OrderByDescending(x => x.BookingCount)
                .FirstOrDefault();

            Console.WriteLine("\nCinemaGo Statistics");
            Console.WriteLine("-------------------------");
            Console.WriteLine($"Total bookings: {bookings.Count}");
            Console.WriteLine($"Active bookings: {activeBookings.Count}");
            Console.WriteLine($"Cancelled bookings: {cancelledBookings.Count}");
            Console.WriteLine($"Total revenue: {totalRevenue}");

            Console.WriteLine(
                $"Active sessions: {sessions.Count(x => !x.IsCancelled)}");

            Console.WriteLine(
                $"Cancelled sessions: {sessions.Count(x => x.IsCancelled)}");

            Console.WriteLine("\nFilm Statistics");

            foreach (var film in filmStatistics.OrderByDescending(x => x.BookingCount))
            {
                Console.WriteLine(
                    $"{film.FilmName} | Bookings: {film.BookingCount} | Revenue: {film.Revenue}");
            }

            if (mostPopularFilm != null)
            {
                Console.WriteLine(
                    $"\nMost popular film: {mostPopularFilm.FilmName} " +
                    $"({mostPopularFilm.BookingCount} bookings)");
            }
            else
            {
                Console.WriteLine("\nNo active bookings yet.");
            }
        }
    }
}