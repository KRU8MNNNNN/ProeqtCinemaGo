using System;

namespace CinemaGoProject.Models
{
    internal class Session
    {
        public int Id { get; set; }
        public int FilmId { get; set; }
        public DateTime StartTime { get; set; }
        public string Hall { get; set; } = "";
        public decimal StandardPrice { get; set; }
        public decimal VipPrice { get; set; }
        public bool IsCancelled { get; set; }
        public int Rows { get; set; } = 5;
        public int SeatsPerRow { get; set; } = 8;
        public int VipRows { get; set; } = 1;

        public override string ToString()
        {
            return $"{Id}|{FilmId}|{StartTime:O}|{Hall}|{StandardPrice}|{VipPrice}|{IsCancelled}|{Rows}|{SeatsPerRow}|{VipRows}";
        }
    }
}