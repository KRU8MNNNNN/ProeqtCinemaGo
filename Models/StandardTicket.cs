namespace CinemaGoProject.Models
{
    internal class StandardTicket : Ticket
    {
        public override decimal CalculatePrice(decimal price)
        {
            return price;
        }

        public override void PrintTicket()
        {
            System.Console.WriteLine("STANDARD TICKET");
            base.PrintTicket();
        }
    }
}