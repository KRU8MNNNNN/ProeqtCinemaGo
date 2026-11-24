using System;

namespace CinemaGoProject.Models
{
    internal class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public char Row { get; set; }
        public int SeatNumber { get; set; }
        public string TicketType { get; set; } = "";
        public decimal Price { get; set; }
        public string TicketCode { get; set; } = "";
        public bool IsCancelled { get; set; }
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return $"{Id}|{UserId}|{SessionId}|{Row}|{SeatNumber}|{TicketType}|{Price}|{TicketCode}|{IsCancelled}|{CreatedAt:O}";
        }
    }
}