namespace CinemaGoProject.Models
{
    internal class Seat
    {
        public char Row { get; set; }
        public int Number { get; set; }
        public string Type { get; set; } = "Standard";
        public bool IsBooked { get; private set; }

        public bool Book()
        {
            if (IsBooked)
            {
                return false;
            }

            IsBooked = true;
            return true;
        }

        public bool Cancel()
        {
            if (!IsBooked)
            {
                return false;
            }

            IsBooked = false;
            return true;
        }

        public string GetSymbol()
        {
            if (IsBooked)
            {
                return "[X]";
            }

            if (Type == "VIP")
            {
                return "[V]";
            }

            return "[O]";
        }
    }
}