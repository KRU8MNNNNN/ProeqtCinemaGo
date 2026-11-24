namespace CinemaGoProject.Models
{
    internal class Admin : User
    {
        public Admin()
        {
            Role = "Admin";
        }

        public override void DisplayMenu()
        {
            System.Console.WriteLine("Admin Menu");
        }
    }
}