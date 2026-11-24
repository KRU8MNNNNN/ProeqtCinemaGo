namespace CinemaGoProject.Models
{
    internal class Customer : User
    {
        public Customer()
        {
            Role = "Customer";
        }

        public override void DisplayMenu()
        {
            System.Console.WriteLine("Customer Menu");
        }
    }
}