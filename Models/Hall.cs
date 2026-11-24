using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaGoProject.Models
{
    internal class Hall
    {
        public string Name { get; set; } = "";
        public int Rows { get; set; }
        public int SeatsPerRow { get; set; }
        public int VipRows { get; set; }
        public List<Seat> Seats { get; set; } = new List<Seat>();

        public void CreateSeats()
        {
            Seats.Clear();

            for (int row = 0; row < Rows; row++)
            {
                for (int number = 1; number <= SeatsPerRow; number++)
                {
                    Seat seat = new Seat
                    {
                        Row = (char)('A' + row),
                        Number = number,
                        Type = row < VipRows ? "VIP" : "Standard"
                    };

                    Seats.Add(seat);
                }
            }
        }

        public void DisplaySeatMap()
        {
            Console.WriteLine($"\n{Name}");
            Console.WriteLine("       SCREEN\n");

            for (int row = 0; row < Rows; row++)
            {
                Console.Write($"{(char)('A' + row)}    ");

                for (int number = 1; number <= SeatsPerRow; number++)
                {
                    Seat seat = GetSeat((char)('A' + row), number);
                    Console.Write(seat.GetSymbol() + " ");
                }

                Console.WriteLine();
            }

            Console.WriteLine("\n[O] Available  [X] Occupied  [V] VIP");
        }

        public Seat GetSeat(char row, int number)
        {
            return Seats.FirstOrDefault(x =>
                char.ToUpper(x.Row) == char.ToUpper(row) &&
                x.Number == number);
        }
    }
}