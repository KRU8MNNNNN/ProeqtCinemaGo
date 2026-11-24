using System;
using System.IO;

namespace CinemaGoProject.Services
{
    internal class Logger
    {
        private static readonly string _filePath = Path.Combine(
            Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
                .Parent.Parent.FullName,
            "Data",
            "logs.txt");

        public static void Log(string action)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));

            string entry = $"{DateTime.Now:dd/MM/yyyy HH:mm:ss} | {action}";

            File.AppendAllText(_filePath, entry + Environment.NewLine);
        }
    }
}