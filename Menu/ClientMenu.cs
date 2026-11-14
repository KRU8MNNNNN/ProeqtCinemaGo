
using CinemaGoProject.Models;
using CinemaGoProject.Services;
using Spectre.Console;
using System;
using System.Linq;

namespace CinemaGoProject.Menu
{
    internal class ClientMenu
    {
        private readonly FilmService _filmService;

        public ClientMenu(FilmService filmService)
        {
            _filmService = filmService;
        }

        public void ShowMainFilmMenu()
        {
            string choice = "";

            while (choice != "0. Log Out")
            {
                choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Welcome to CinemaGo")
                    .AddChoices(
                        "1. Show Films",
                        "2. Add Film",
                        "3. Update Film",
                        "4. Delete Film",
                        "0. Log Out"));

                switch (choice)
                {
                    case "1. Show Films":
                        ShowFilmMenu();
                        break;

                    case "2. Add Film":
                        AddFilmMenu();
                        break;

                    case "3. Update Film":
                        UpdateFilmMenu();
                        break;

                    case "4. Delete Film":
                        DeleteFilmMenu();
                        break;

                    case "0. Log Out":
                        Console.WriteLine("Logged out successfully.");
                        break;
                }
            }
        }

        private void ShowFilmMenu()
        {
            var films = _filmService.GetAllFilms();

            Console.WriteLine("\nCinemaGo Films");

            if (films.Count == 0)
            {
                Console.WriteLine("No films available.");
                return;
            }

            foreach (var film in films)
            {
                Console.WriteLine(film);
            }
        }

        private void AddFilmMenu()
        {
            Film film = new Film();

            var films = _filmService.GetAllFilms();

            film.Id = films.Count == 0 ? 1 : films.Max(x => x.Id) + 1;

            Console.Write("Enter film name: ");
            film.Name = Console.ReadLine() ?? "";

            Console.Write("Enter film description: ");
            film.Description = Console.ReadLine() ?? "";

            _filmService.AddFilm(film);

            Console.WriteLine("Film added successfully!");
        }

        private void UpdateFilmMenu()
        {
            Console.Write("Enter film ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Film film = _filmService.GetFilmById(id);

            if (film == null)
            {
                Console.WriteLine("Film not found.");
                return;
            }

            Console.Write("Enter new film name: ");
            film.Name = Console.ReadLine() ?? "";

            Console.Write("Enter new film description: ");
            film.Description = Console.ReadLine() ?? "";

            _filmService.UpdateFilm(film);

            Console.WriteLine("Film updated successfully!");
        }

        private void DeleteFilmMenu()
        {
            Console.Write("Enter film ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Film film = _filmService.GetFilmById(id);

            if (film == null)
            {
                Console.WriteLine("Film not found.");
                return;
            }

            _filmService.DeleteFilm(id);

            Console.WriteLine("Film deleted successfully!");
        }
    }
}
