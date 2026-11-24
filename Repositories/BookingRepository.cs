using CinemaGoProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CinemaGoProject.Repositories
{
    internal class BookingRepository
    {
        private readonly string _filePath = Path.Combine(
            Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
                .Parent.Parent.FullName,
            "Data",
            "bookings.txt");

        public List<Booking> GetAllBookings()
        {
            List<Booking> bookings = new List<Booking>();

            if (!File.Exists(_filePath))
            {
                return bookings;
            }

            foreach (string item in File.ReadAllLines(_filePath))
            {
                string[] line = item.Split('|');

                if (line.Length != 10)
                {
                    continue;
                }

                Booking booking = new Booking
                {
                    Id = int.Parse(line[0]),
                    UserId = int.Parse(line[1]),
                    SessionId = int.Parse(line[2]),
                    Row = char.Parse(line[3]),
                    SeatNumber = int.Parse(line[4]),
                    TicketType = line[5],
                    Price = decimal.Parse(line[6]),
                    TicketCode = line[7],
                    IsCancelled = bool.Parse(line[8]),
                    CreatedAt = DateTime.Parse(line[9])
                };

                bookings.Add(booking);
            }

            return bookings;
        }

        public void SaveBooking(Booking booking)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
            File.AppendAllLines(_filePath, new[] { booking.ToString() });
        }

        public void UpdateBooking(Booking booking)
        {
            List<Booking> bookings = GetAllBookings();

            Booking existing = bookings.FirstOrDefault(x => x.Id == booking.Id);

            if (existing == null)
            {
                return;
            }

            existing.IsCancelled = booking.IsCancelled;

            SaveAllBookings(bookings);
        }

        private void SaveAllBookings(List<Booking> bookings)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));

            string[] lines = new string[bookings.Count];

            for (int i = 0; i < bookings.Count; i++)
            {
                lines[i] = bookings[i].ToString();
            }

            File.WriteAllLines(_filePath, lines);
        }
    }
}