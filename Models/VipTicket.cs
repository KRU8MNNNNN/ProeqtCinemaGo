namespace CinemaGoProject.Models
{
    internal class VipTicket : Ticket
    {
        public override decimal CalculatePrice(decimal price)
        {
            return price;
        }

        public override void PrintTicket()
        {
            System.Console.WriteLine("VIP TICKET");
            base.PrintTicket();
        }
    }
}