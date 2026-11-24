namespace CinemaGoProject.Models
{
    internal abstract class Ticket
    {
        public string Code { get; set; } = "";
        public decimal Price { get; set; }
        public int SessionId { get; set; }
        public char Row { get; set; }
        public int SeatNumber { get; set; }

        public abstract decimal CalculatePrice(decimal price);

        public virtual void PrintTicket()
        {
            System.Console.WriteLine("Ticket Code: " + Code);
            System.Console.WriteLine("Session ID: " + SessionId);
            System.Console.WriteLine("Seat: " + Row + SeatNumber);
            System.Console.WriteLine("Price: " + Price);
        }
    }
}