using CinemaGoProject.Models;
using System;
using System.IO;
using System.Linq;

namespace CinemaGoProject.Repositories
{
    internal class SeatRepository
    {
        private readonly string _filePath = Path.Combine(
            Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
                .Parent.Parent.FullName,
            "Data",
            "seats.txt");

        public void SaveSeats(int sessionId, Hall hall)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));

            var lines = File.Exists(_filePath)
                ? File.ReadAllLines(_filePath)
                    .Where(x => !x.StartsWith(sessionId + "|")).ToList()
                : new System.Collections.Generic.List<string>();

            foreach (var seat in hall.Seats)
            {
                lines.Add($"{sessionId}|{seat.Row}|{seat.Number}|{seat.Type}|{seat.IsBooked}");
            }

            File.WriteAllLines(_filePath, lines);
        }

        public void LoadSeats(int sessionId, Hall hall)
        {
            if (!File.Exists(_filePath))
            {
                return;
            }

            var lines = File.ReadAllLines(_filePath);

            foreach (var item in lines)
            {
                var line = item.Split('|');

                if (line.Length != 5 || line[0] != sessionId.ToString())
                {
                    continue;
                }

                if (int.TryParse(line[2], out int number) &&
                    bool.TryParse(line[4], out bool isBooked))
                {
                    Seat seat = hall.GetSeat(line[1][0], number);

                    if (seat != null)
                    {
                        seat.Type = line[3];

                        if (isBooked)
                        {
                            seat.Book();
                        }
                    }
                }
            }
        }
    }
}