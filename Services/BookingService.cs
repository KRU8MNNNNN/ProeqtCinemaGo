using CinemaGoProject.Models;
using CinemaGoProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaGoProject.Services
{
    internal class BookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly SessionService _sessionService;
        private readonly EmailService _emailService;
        private readonly UserRepository _userRepository;

        private const int BookingsForDiscount = 5;
        private const decimal DiscountRate = 0.10m;

        public BookingService(
            BookingRepository bookingRepository,
            SessionService sessionService,
            EmailService emailService,
            UserRepository userRepository)
        {
            _bookingRepository = bookingRepository;
            _sessionService = sessionService;
            _emailService = emailService;
            _userRepository = userRepository;
        }

        public List<Booking> GetUserBookings(int userId)
        {
            return _bookingRepository.GetAllBookings()
                .Where(x => x.UserId == userId && !x.IsCancelled)
                .ToList();
        }

        public Booking CreateBooking(
            User user, int sessionId, char row, int seatNumber, string ticketType)
        {
            Session session = _sessionService.GetSessionById(sessionId);

            if (session == null || session.IsCancelled)
                throw new Exception("Session is not available.");

            Hall hall = _sessionService.GetHall(sessionId);

            if (hall == null)
                throw new Exception("Hall not found.");

            Seat seat = hall.GetSeat(row, seatNumber);

            if (seat == null)
                throw new Exception("Invalid seat.");

            if (seat.IsBooked)
                throw new Exception("This seat is already booked.");

            if (!seat.Type.Equals(ticketType, StringComparison.OrdinalIgnoreCase))
                throw new Exception("The selected ticket type does not match this seat.");

            decimal basePrice;
            Ticket ticket;

            if (ticketType.Equals("VIP", StringComparison.OrdinalIgnoreCase))
            {
                basePrice = session.VipPrice;
                ticket = new VipTicket();
            }
            else if (ticketType.Equals("Standard", StringComparison.OrdinalIgnoreCase))
            {
                basePrice = session.StandardPrice;
                ticket = new StandardTicket();
            }
            else
            {
                throw new Exception("Invalid ticket type.");
            }

            decimal originalPrice = ticket.CalculatePrice(basePrice);

            int previousBookings = _bookingRepository.GetAllBookings()
                .Count(x => x.UserId == user.Id && !x.IsCancelled);

            bool hasDiscount = previousBookings >= BookingsForDiscount - 1;

            decimal discount = hasDiscount
                ? originalPrice * DiscountRate
                : 0;

            ticket.Code = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            ticket.SessionId = sessionId;
            ticket.Row = row;
            ticket.SeatNumber = seatNumber;
            ticket.Price = originalPrice - discount;

            _sessionService.BookSeat(sessionId, row, seatNumber);

            List<Booking> bookings = _bookingRepository.GetAllBookings();

            int nextId = bookings.Count > 0
                ? bookings.Max(x => x.Id) + 1
                : 1;

            Booking booking = new Booking
            {
                Id = nextId,
                UserId = user.Id,
                SessionId = sessionId,
                Row = row,
                SeatNumber = seatNumber,
                TicketType = ticketType,
                Price = ticket.Price,
                TicketCode = ticket.Code,
                IsCancelled = false,
                CreatedAt = DateTime.Now
            };

            _bookingRepository.SaveBooking(booking);

            Logger.Log(
                $"Booking created | User ID: {user.Id} | Booking ID: {booking.Id} | Session: {sessionId} | Seat: {row}{seatNumber} | Original price: {originalPrice} | Discount: {discount} | Final price: {ticket.Price}");

            try
            {
                string message =
                    "<h2>CinemaGo Ticket</h2>" +
                    $"<p>Ticket code: {ticket.Code}</p>" +
                    $"<p>Session ID: {sessionId}</p>" +
                    $"<p>Seat: {row}{seatNumber}</p>" +
                    $"<p>Ticket type: {ticketType}</p>" +
                    $"<p>Original price: {originalPrice}</p>" +
                    $"<p>Discount: {discount}</p>" +
                    $"<p>Final price: {ticket.Price}</p>";

                _emailService.SendEmail(
                    user.Email,
                    "CinemaGo - Your Ticket",
                    message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ticket saved, but email could not be sent: " + ex.Message);
                Logger.Log($"Ticket email failed | Booking ID: {booking.Id} | Error: {ex.Message}");
            }

            Console.WriteLine($"Original price: {originalPrice}");

            if (hasDiscount)
                Console.WriteLine("Loyalty discount: 10%");

            Console.WriteLine($"Final price: {ticket.Price}");

            return booking;
        }

        public void CancelBooking(int userId, int bookingId)
        {
            List<Booking> bookings = _bookingRepository.GetAllBookings();

            Booking booking = bookings.FirstOrDefault(x =>
                x.Id == bookingId &&
                x.UserId == userId &&
                !x.IsCancelled);

            if (booking == null)
                throw new Exception("Booking not found.");

            _sessionService.CancelSeat(
                booking.SessionId,
                booking.Row,
                booking.SeatNumber);

            booking.IsCancelled = true;
            _bookingRepository.UpdateBooking(booking);

            Logger.Log(
                $"Booking cancelled | User ID: {userId} | Booking ID: {bookingId}");
        }

        public int CancelBookingsForSession(int sessionId)
        {
            List<Booking> bookings = _bookingRepository.GetAllBookings()
                .Where(x => x.SessionId == sessionId && !x.IsCancelled)
                .ToList();

            List<User> users = _userRepository.GetAllUsers();

            foreach (Booking booking in bookings)
            {
                _sessionService.CancelSeat(
                    booking.SessionId,
                    booking.Row,
                    booking.SeatNumber);

                booking.IsCancelled = true;
                _bookingRepository.UpdateBooking(booking);

                User user = users.FirstOrDefault(x => x.Id == booking.UserId);

                Logger.Log(
                    $"Booking automatically cancelled | Booking ID: {booking.Id} | Session: {sessionId}");

                if (user != null && !string.IsNullOrWhiteSpace(user.Email))
                {
                    try
                    {
                        string message =
                            "<h2>CinemaGo Session Cancelled</h2>" +
                            $"<p>Your booking for session {sessionId} has been cancelled.</p>" +
                            $"<p>Seat: {booking.Row}{booking.SeatNumber}</p>" +
                            $"<p>Ticket code: {booking.TicketCode}</p>";

                        _emailService.SendEmail(
                            user.Email,
                            "CinemaGo - Session Cancelled",
                            message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Could not email {user.Email}: {ex.Message}");

                        Logger.Log(
                            $"Cancellation email failed | Booking ID: {booking.Id} | Error: {ex.Message}");
                    }
                }
            }

            Logger.Log(
                $"Session {sessionId}: automatically cancelled {bookings.Count} booking(s)");

            return bookings.Count;
        }
    }
}